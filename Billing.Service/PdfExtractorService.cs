using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto.General;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;
using MoveEnergia.Billing.Data.Context;
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
        private readonly ILeituraFaturaPdfLogRepository _leituraFaturaLogRep;
        private readonly ILeituraFaturaPdfProcessoRepository _leituraFaturaProcessoRep;
        private readonly ApplicationDbContext _dbContext;

        public PdfExtractorService(ILogger<PdfExtractorService> logger,
                ILeituraFaturaPdfRepository leituraFaturaRep,
                ILeituraFaturaPdfLogRepository leituraFaturaLogRep,
                ILeituraFaturaPdfProcessoRepository leituraFaturaProcessoRep,
                ApplicationDbContext dbContext)
        {
            _logger = logger;
            _leituraFaturaRep = leituraFaturaRep;
            _leituraFaturaLogRep = leituraFaturaLogRep;
            _leituraFaturaProcessoRep = leituraFaturaProcessoRep;
            _dbContext = dbContext;
        }
        public async Task<FaturaPdfData> StartProcess(int Id)
        {
            FaturaPdfData pdfData = new FaturaPdfData();

            LeituraFaturaPdfProcesso processo = new LeituraFaturaPdfProcesso();
            processo.Logs = new List<LeituraFaturaPdfLog>();
            processo.Inicio = DateTime.Now;

            //string sourcePath = @"C:\Temp\moveenergia-faturas\2025-10";
            //string sourcePath = @"C:\Temp\moveenergia-faturas\2025-10-2";
            string sourcePath = @"C:\Users\Administrator\MOVE Energia\GD - Operação - 05. Fatura\2025.10";

            if (!String.IsNullOrWhiteSpace(sourcePath) && !sourcePath.EndsWith(@"\")) sourcePath += @"\";
            Log.Debug("Pasta Origem:" + sourcePath + "//Directory.Exists:" + Directory.Exists(sourcePath));
            if (Directory.Exists(sourcePath))
            {
                int i = 0, processados = 0 ;
                DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
                
                //ASC DATE
                FileInfo[] fileEntries = dirInfo.GetFiles().OrderBy(p => p.LastWriteTime).ToArray();

                foreach (FileInfo fileInfo in fileEntries)
                {
                    i += 1;
                    string fileName = fileInfo.FullName;
                    Log.Debug("Idx{2}::Processados{3}::Arquivo:{0}: Criado em:{1}", fileName, fileInfo.LastWriteTime, i, processados);

                    //if (fileName.IndexOf("3510590-copel") < 0) continue;

                    string md5 = PdfExtractorUtils.GetMD5Checksum(fileName);
                    Log.Debug(md5);
                    LeituraFaturaPdf? faturaDomain = await _dbContext.LeituraFaturaPdfs
                            .Where(u => u.FileMD5 == md5)
                            .FirstOrDefaultAsync();
                    
                    if (faturaDomain != null) continue;

                    using (PdfDocument document = PdfDocument.Open(fileName))
                    {
                        Page page = document.GetPage(1);
                        string TextOut = ContentOrderTextExtractor.GetText(page, true);
                        Log.Debug(TextOut);

                        if (TextOut == null || String.IsNullOrEmpty(TextOut.Trim()))
                        {
                            Log.Debug("PDF sem conteudo");
                            continue;
                        }

                        IPdfExtractorByDistr? extractor = null;

                        if (TextOut.IndexOf("Beneficiário: Celesc Distribuição") >= 0)
                        {
                            extractor = new PdfExtractorCelesc();
                        } else if (TextOut.IndexOf("Copel Distribuição") >= 0)
                        {
                            extractor = new PdfExtractorCopel();
                        } else if (TextOut.IndexOf("RGE SUL DISTRIBUIDORA DE ENERGIA") >= 0)
                        {
                            extractor = new PdfExtractorRGE();
                        }
                        else if (TextOut.IndexOf("COOPERZEM COOPERATIVA DE DISTRIBUIÇÃO DE ENERGIA ELETRICA") >= 0)
                        {
                            Log.Debug("COOPERZEM");
                            continue;
                        }
                        else if (TextOut.IndexOf("COMPANHIA PAULISTA DE FORÇA E LUZ") >= 0)
                        {
                            extractor = new PdfExtractorCpfl();
                        }
                        else if (TextOut.IndexOf("CNPJ 02.328.280/0001-97") >= 0)
                        {
                            Log.Debug("neoenergia/elektro");
                            continue;
                        }
                        else if (TextOut.IndexOf("COMPANHIA ESTADUAL DE DISTRIBUICAO DE ENERGIA ELETRICA") >= 0)
                        {
                            extractor = new PdfExtractorCEEE();
                        }
                        else if (TextOut.IndexOf("Ampla Energia e Serviços S. A.") >= 0)
                        {
                            Log.Debug("Ampla Energia");
                            continue;
                        }
                        else if (TextOut.IndexOf("COMPANHIA ENERGÉTICA DE PERNAMBUCO") >= 0)
                        {
                            //Log.Debug("Ampla Energia");
                            continue;
                        }
                        else if (TextOut.IndexOf("COOP ELETRIFICACAO DE BRACO DO NORTE") >= 0)
                        {
                            //Log.Debug("Ampla Energia");
                            continue;
                        }
                        else if (TextOut.IndexOf("COMPANHIA JAGUARI DE ENERGIA") >= 0)
                        {
                            //Log.Debug("Ampla Energia");
                            continue;
                        }
                        else
                        {
                            //Documento Ignorado
                            Log.Debug("DISTRNAOMAPEADA");
                            continue;
                        }
                        if (extractor != null)
                        {
                            FaturaPdfData data = await extractor.ExtractInfo(document);
                            if (data != null && String.IsNullOrEmpty(data.ErrorMessage))
                            {
                                LeituraFaturaPdf fatura = new LeituraFaturaPdf();
                                fatura.FileName = fileInfo.Name;
                                fatura.FolderName = fileInfo.DirectoryName;
                                fatura.FileMD5 = md5;
                                fatura.NomeDistribuidora = data.NomeDistribuidora;

                                fatura.UC = data.UC;
                                fatura.NomeCliente = data.NomeCliente;
                                fatura.CpfCnpj = data.CpfCnpj != null ? data.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "") : null;
                                fatura.MesReferencia = data.MesRef;
                                fatura.Valor = data.Valor;
                                fatura.DataEmissao = data.DataEmissao;
                                fatura.Vencimento = data.Vcto;

                                fatura.LeituraAnterior = data.LeituraAnterior;
                                fatura.LeituraAtual = data.LeituraAtual;

                                fatura.EnergiaConsumida = data.EnergiaConsumida;
                                fatura.EnergiaCompensada = data.EnergiaCompensada;
                                fatura.EnergiaSaldo = data.EnergiaSaldo;

                                fatura.TarifaConsumo = data.TarifaConsumo;
                                fatura.TarifaCompensada = data.TarifaCompensada;

                                fatura.FlagPainelSolar = data.EnergiaPainel;

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
                            else
                            {
                                Log.Debug(data.ErrorMessage);

                                LeituraFaturaPdfLog faturaLog = new LeituraFaturaPdfLog();
                                faturaLog.Processo = processo.Id;
                                faturaLog.FileName = fileInfo.Name;
                                faturaLog.FolderName = fileInfo.DirectoryName;
                                faturaLog.FileMD5 = md5;
                                faturaLog.NomeDistribuidora = extractor.GetDistribuidora();
                                faturaLog.DataHora = DateTime.Now;

                                if (data == null)
                                {
                                    faturaLog.Mensagem = "Erro não identificado na extração.(DATARET_NULL)";
                                } else
                                {
                                    faturaLog.Mensagem = data.ErrorMessage;
                                }

                                processo.Logs.Add(faturaLog);
                                //await _leituraFaturaLogRep.CreateAsync(faturaLog);
                                //await _leituraFaturaLogRep.SaveAsync();


                            }
                            processados++;
                        }

                        document.Dispose();
                    }

                    if (processados == 500) break;
                }
            }

            processo.Termino = DateTime.Now;
            await _leituraFaturaProcessoRep.CreateAsync(processo);
            await _leituraFaturaProcessoRep.SaveAsync();

            return pdfData;
        }

        
    }
}
