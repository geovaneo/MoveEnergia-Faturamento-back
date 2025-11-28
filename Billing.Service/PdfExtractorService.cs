using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto.General;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;
using MoveEnergia.Billing.Data.Repository;
using Serilog;
using System.Reflection.PortableExecutable;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace MoveEnergia.Billing.Extractor.Service
{
    public class PdfExtractorService : IPdfExtractorService
    {
        private readonly ILogger<PdfExtractorService> _logger;
        private readonly ILeituraFaturaPdfRepository _leituraFaturaRep;
        private readonly ILeituraFaturaPdfProcessoRepository _leituraFaturaProcessoRep;

        public PdfExtractorService(ILogger<PdfExtractorService> logger,
                ILeituraFaturaPdfRepository leituraFaturaRep,
                ILeituraFaturaPdfProcessoRepository leituraFaturaProcessoRep)
        {
            _logger = logger;
            _leituraFaturaRep = leituraFaturaRep;
            _leituraFaturaProcessoRep = leituraFaturaProcessoRep;
        }
        public async Task<FaturaPdfData> StartProcess(int Id)
        {
            Log.Debug("extract1");
            FaturaPdfData pdfData = new FaturaPdfData();
            pdfData.ErrorMessage = "Teste";

            LeituraFaturaPdfProcesso processo = new LeituraFaturaPdfProcesso();
            processo.Inicio = DateTime.Now;

            string sourcePath = @"C:\Temp\moveenergia-faturas\2025-10-2";
            //string sourcePath = @"C:\Users\Administrator\MOVE Energia\GD - Operação - 05. Fatura\2025.10";

            if (!String.IsNullOrWhiteSpace(sourcePath) && !sourcePath.EndsWith(@"\")) sourcePath += @"\";
            Log.Debug("Pasta Origem:" + sourcePath + "//Directory.Exists:" + Directory.Exists(sourcePath));
            if (Directory.Exists(sourcePath))
            {
                int i = 0;
                DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
                
                //ASC DATE
                FileInfo[] fileEntries = dirInfo.GetFiles().OrderBy(p => p.LastWriteTime).ToArray();

                Log.Debug("FileEntries:" + fileEntries);
                foreach (FileInfo fileInfo in fileEntries)
                {
                    string fileName = fileInfo.FullName;
                    Log.Debug("Arquivo:{0}: Criado em:{1}", fileName, fileInfo.LastWriteTime);

                    //if (fileName.IndexOf("3510590-copel") < 0) continue;

                    string md5 = PdfExtractorUtils.GetMD5Checksum(fileName);

                    using (PdfDocument document = PdfDocument.Open(fileName))
                    {

                        Page page = document.GetPage(1);
                        string TextOut = ContentOrderTextExtractor.GetText(page, true);
                        //Log.Debug(TextOut);

                        IPdfExtractorByDistr? extractor = null;

                        if (TextOut.IndexOf("Beneficiário: Celesc Distribuição") >= 0)
                        {
                            extractor = new PdfExtractorCelesc();
                        } else if (TextOut.IndexOf("Copel Distribuição") >= 0)
                        {
                            extractor = new PdfExtractorCopel();
                        }
                        else
                        {
                            extractor = new PdfExtractorRGE();
                            //Documento Ignorado
                        }
                        if (extractor != null)
                        {
                            i += 1;
                            FaturaPdfData data = await extractor.ExtractInfo(document);
                            if (data != null && String.IsNullOrEmpty(data.ErrorMessage))
                            {
                                LeituraFaturaPdf fatura = new LeituraFaturaPdf();
                                fatura.FileName = fileInfo.Name;
                                fatura.FolderName = fileInfo.DirectoryName;
                                fatura.FileMD5 = md5;
                                fatura.NomeDistribuidora = data.NomeDistribuidora;

                                fatura.UC = data.UC;
                                fatura.MesReferencia = data.MesRef;
                                fatura.Valor = data.Valor;
                                fatura.DataEmissao = data.DataEmissao;
                                fatura.Vencimento = data.Vcto;

                                fatura.LeituraAnterior = data.LeituraAnterior;
                                fatura.LeituraAtual = data.LeituraAtual;

                                fatura.EnergiaConsumida = data.EnergiaConsumida;
                                fatura.EnergiaCompensada = data.EnergiaCompensada;
                                fatura.EnergiaSaldo = data.EnergiaSaldo;

                                fatura.CodBarras = data.CodBarras;

                                if (data.Linhas != null && data.Linhas.Count > 0)
                                {
                                    fatura.Linhas = new List<LeituraFaturaLinha>();
                                    foreach (var linha in data.Linhas)
                                    {
                                        LeituraFaturaLinha faturaLinha = new LeituraFaturaLinha();
                                        faturaLinha.Descricao = linha.Descricao;
                                        faturaLinha.Unidade = linha.Unidade;
                                        faturaLinha.Qtd = linha.Qtd;
                                        faturaLinha.PrecoUnit = linha.PrecoUnit;
                                        faturaLinha.Valor = linha.Valor;
                                        faturaLinha.CofinsPIS = linha.CofinsPIS;
                                        faturaLinha.ICMSBaseCalc = linha.ICMSBaseCalc;
                                        faturaLinha.ICMSAliq = linha.ICMSAliq;
                                        faturaLinha.ICMS = linha.ICMS;
                                        faturaLinha.TarifaUnit = linha.TarifaUnit;

                                        fatura.Linhas.Add(faturaLinha);
                                    }
                                }

                                await _leituraFaturaRep.CreateAsync(fatura);
                                await _leituraFaturaRep.SaveAsync();

                            }
                            else Log.Debug(data.ErrorMessage);
                        }

                        document.Dispose();
                    }

                    if (i == 100) break;
                }
            }

            processo.Termino = DateTime.Now;
            await _leituraFaturaProcessoRep.CreateAsync(processo);
            await _leituraFaturaProcessoRep.SaveAsync();

            return pdfData;
        }

        
    }
}
