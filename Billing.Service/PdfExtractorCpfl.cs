using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto.General;
using MoveEnergia.Billing.Core.Interface.Service;
using Serilog;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig.Geometry;

namespace MoveEnergia.Billing.Extractor.Service
{
    public class PdfExtractorCpfl : IPdfExtractorByDistr
    {
        public PdfExtractorCpfl()
        {
        }

        public string GetDistribuidora()
        {
            return "CPFL";
        }
        public async Task<FaturaPdfData> ExtractInfo(PdfDocument document)
        {
            Log.Debug("extract rge");
            FaturaPdfData pdfData = new FaturaPdfData();
            pdfData.NomeDistribuidora = GetDistribuidora();

            processPDF(document, pdfData);

            return pdfData;
        }

        private bool FindUC(string pageText, FaturaPdfData pdfData)
        {
            Log.Debug("Procurando UC");
            pageText = Regex.Replace(pageText.Trim(), @"\s+", " ");
            Match match = Regex.Match(pageText, @"\d{4,11} NOTA FISCAL");
            if (match.Success)
            {
                Log.Debug($"First match: {match.Value} at index {match.Index}");
                match = Regex.Match(match.Value, @"\d{4,11}");
                if (match.Success) pdfData.UC = match.Value;
            }

            return true;
        }

        private bool GetDadosCliente(string? pageText, FaturaPdfData pdfData)
        {
            Log.Debug("GetDadosCliente");
            if (String.IsNullOrWhiteSpace(pageText))
            {
                pdfData.ErrorMessage = "GetDadosCliente - Sem conteúdo texto na página";
                return false;
            }

            Match match = Regex.Match(pageText, @"\w{3}\/\d{4} \d{2}\/\d{2}\/\d{4} R\$ (\d{0,3}[.])*\d{1,3},\d{2}");
            if (match.Success)
            {
                Log.Debug($"GetDadosCliente: {match.Value} at index {match.Index}");
                string[] infos = match.Value.Split(" ");
                
                pdfData.Valor = PdfExtractorUtils.ParseStringToDecimal(infos[3]);
                pdfData.MesRef = PdfExtractorUtils.ParseMonthStringToNumber(infos[0]);                
                pdfData.Vcto = PdfExtractorUtils.ParseStringToDate(infos[1]);
                pdfData.ErrorMessage = null;
            }
            else
            {
                pdfData.ErrorMessage = "Valor, MesRef e Vcto não identificados";
                return false;
            }
            return true;


        }

        private void GetDadosLeitura(string pageText, FaturaPdfData pdfData)
        {
            pageText = Regex.Replace(pageText.Trim(), @"\s+", " ");
            Log.Debug(">> GetDadosLeitura");
            Match match = Regex.Match(pageText, @"(\d{2}\/\d{2}\/\d{4}) (\d{2}\/\d{2}\/\d{4}) \d{1,2} Proxima leitura (\d{2}\/\d{2}\/\d{4})");
            if (match.Success)
            {
                Log.Debug($"Datas Leitura: {match.Value} at index {match.Index}");
                string[] infos = match.Value.Split(" ");

                pdfData.LeituraAtual = PdfExtractorUtils.ParseStringToDate(infos[0]);
                pdfData.LeituraAnterior = PdfExtractorUtils.ParseStringToDate(infos[1]);
            }
            else Log.Debug(">>>> Não encontrou datas de leitura");

        }

        private void GetLinhasFatura(FaturaPdfData pdfData, string pageText)
        {
            pdfData.Linhas = new List<FaturaPdfLinha>();
            
            int energiaCompensada = 0;
            int energiaConsumida = 0;
            MatchCollection matches = Regex.Matches(pageText, @"kWh (\d{1,3}[.])*\d{1,3},\d{1,6} \d{1,3},\d{1,8} \d{1,3},\d{1,8} (\d{1,3}[.])*\d{1,3},\d{1,2}[-]* ([-]*\d{1,3},\d{1,2})*([ ]*\d{1,3}[,\.]\d{1,2})*( \d{1,3},\d{1,2})*( [-]*\d{1,3},\d{1,2})*( [-]*\d{1,3},\d{1,2})*");
            foreach (Match match in matches)
            {

                //Log.Debug($"First match: {match.Value} at index {match.Index}");
                string[] infos = match.Value.Split(" ");

                string texto = pageText.Substring(0, pageText.IndexOf(match.Value));
                //Log.Debug(texto);
                string name = texto.Substring(texto.LastIndexOf("\n") + 1);
                //Log.Debug("*********************************" + name);

                FaturaPdfLinha linha = new FaturaPdfLinha();
                linha.Descricao = name;

                linha.Unidade = infos[0];
                if (infos.Length == 10)
                {
                    //Linha Completa
                    //NOME - unid medida - qtd - tarifa aneel - tarifa com trib - valor - base icms - aliq icms - icms - pis - cofins
                    //Consumo - TE OUT/25 kWh 870,0000 0,30445000 0,39382759 342,63 342,63 17,00 58,25 3,50 16,01
                    linha.Qtd = PdfExtractorUtils.ParseStringToDecimal(infos[1]);

                    linha.TarifaUnit = PdfExtractorUtils.ParseStringToDecimal(infos[2]);
                    linha.PrecoUnit = PdfExtractorUtils.ParseStringToDecimal(infos[3]);
                    linha.Valor = PdfExtractorUtils.ParseStringToDecimal(infos[4]);

                    linha.ICMSBaseCalc = PdfExtractorUtils.ParseStringToDecimal(infos[5]);
                    linha.ICMSAliq = PdfExtractorUtils.ParseStringToDecimal(infos[6]);
                    linha.ICMS = PdfExtractorUtils.ParseStringToDecimal(infos[7]);
                    linha.PIS = PdfExtractorUtils.ParseStringToDecimal(infos[8]);
                    linha.Cofins = PdfExtractorUtils.ParseStringToDecimal(infos[9]);
                } else if (linha.Descricao.IndexOf("Energ Atv Inj") >= 0)
                    {
                    linha.Qtd = PdfExtractorUtils.ParseStringToDecimal(infos[1]);

                    linha.TarifaUnit = PdfExtractorUtils.ParseStringToDecimal(infos[2]);
                    linha.PrecoUnit = PdfExtractorUtils.ParseStringToDecimal(infos[3]);
                    linha.Valor = PdfExtractorUtils.ParseStringToDecimal(infos[4]);
                }
                else if (linha.Descricao.IndexOf("Adicional de Bandeira") >= 0)
                {
                    linha.Valor = PdfExtractorUtils.ParseStringToDecimal(infos[1]);
                    linha.ICMSBaseCalc = PdfExtractorUtils.ParseStringToDecimal(infos[2]);
                    linha.ICMSAliq = PdfExtractorUtils.ParseStringToDecimal(infos[3]);
                    linha.ICMS = PdfExtractorUtils.ParseStringToDecimal(infos[4]);
                    linha.PIS = PdfExtractorUtils.ParseStringToDecimal(infos[5]);
                    linha.Cofins = PdfExtractorUtils.ParseStringToDecimal(infos[6]);
                }

                if (linha.Descricao.IndexOf("Consumo - TE") >= 0)
                {
                    energiaConsumida += Convert.ToInt32(linha.Qtd);
                } else if (linha.Descricao.IndexOf("Energ Atv Inj. oUC mPT - TE") >= 0)
                {
                    energiaCompensada += Convert.ToInt32(linha.Qtd);
                }

                pdfData.EnergiaCompensada = energiaCompensada;
                pdfData.EnergiaConsumida = energiaConsumida;

                pdfData.Linhas.Add(linha);
            }


        }

        private void processPDF(PdfDocument document, FaturaPdfData pdfData)
        {

            int pages = document.GetPages().Count();

            Page page1 = document.GetPage(1);

            var (pageText1, pageText1NoBreakLines) = PdfExtractorUtils.GetTextFromPage(page1);
            string? pageText2 = null;
            string? pageText2NoBreakLines = null;
            if (pages > 1)
            {
                (pageText2, pageText2NoBreakLines) = PdfExtractorUtils.GetTextFromPage(document.GetPage(2));
            }

            Log.Debug(pageText1);

            if (!FindUC(pageText1, pdfData)) { }
            if (!GetDadosCliente(pageText1, pdfData))
            {
                if (pages > 1 && !GetDadosCliente(pageText2, pdfData)) return;
            }
            GetDadosLeitura(pageText1, pdfData);

            Log.Debug("EMISSAO 0");
            Match match = Regex.Match(pageText1NoBreakLines, @"EMISSAO: \d{2}\/\d{2}\/\d{4}");
            if (match.Success)
            {
                Log.Debug("EMISAAO1:" + match.Value);
                match = Regex.Match(match.Value, @"\d{2}\/\d{2}\/\d{4}");
                Log.Debug("EMISAAO2:" + match.Value);
                pdfData.DataEmissao = PdfExtractorUtils.ParseStringToDate(match.Value);
            }

            match = Regex.Match(pageText1, PdfExtractorUtils.REGEX_CODBARRAS);
            if (match.Success)
            {
                pdfData.CodBarras = match.Value.Replace(".", "").Replace(" ", "");
            }
            else
            {
                if (pages > 1 && !String.IsNullOrEmpty(pageText2))
                {
                    match = Regex.Match(pageText2, PdfExtractorUtils.REGEX_CODBARRAS);
                    if (match.Success)
                    {
                        pdfData.CodBarras = match.Value.Replace(".", "").Replace(" ", "");
                    }
                }
            }

            match = Regex.Match(pageText1NoBreakLines, @"Saldo em Energia da Instalacao:([ ]*\w+[ ]*)(\d{1,3}.)*\d{1,3},\d{1,12} kWh");
            if (match.Success)
            {
                Log.Debug("SALDO>>>>" + match.Value);
                match = Regex.Match(match.Value, @"(\d{1,3}.)*\d{1,3},\d{1,12}");
                if (match.Success)
                {
                    pdfData.EnergiaSaldo = PdfExtractorUtils.ParseStringToDecimal(match.Value);
                }
            }

            GetLinhasFatura(pdfData, pageText1);

        }

    }
}
