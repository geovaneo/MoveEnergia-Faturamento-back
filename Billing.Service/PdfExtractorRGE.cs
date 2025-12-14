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
    public class PdfExtractorRGE : IPdfExtractorByDistr
    {
        public PdfExtractorRGE()
        {
        }
        public async Task<FaturaPdfData> ExtractInfo(PdfDocument document)
        {
            Log.Debug("extract rge");
            FaturaPdfData pdfData = new FaturaPdfData();
            pdfData.NomeDistribuidora = "RGE";

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

        private void GetDadosCliente(Page page, FaturaPdfData pdfData)
        {
            var areaWithoutBorders = new PdfRectangle(0, 600, page.Width/2, page.Height);
            //Log.Debug($"x1: {0} y1: {600} x2:{page.Width / 2} y2:{page.Height} height:{page.Height}");

            var words = page.GetWords().Where(w => areaWithoutBorders.Contains(w.BoundingBox)).ToList();

            List<Word> sortedWords = words
                .OrderBy(w => page.Height - w.BoundingBox.Bottom) // Sort by the bottom edge of the bounding box
                .ThenBy(w => w.BoundingBox.Left)    // Then by the left edge of the bounding box
                .ToList();

            var pageText = string.Join(" ", sortedWords);
            //Log.Debug(pageText);

            Match match = Regex.Match(pageText, "(\\d{0,3}[.])*\\d{1,3},\\d{2} R\\$ \\d{1,2}\\/\\d{4} \\d{2}\\/\\d{2}\\/\\d{4}");
            if (match.Success)
            {
                Log.Debug($"First match: {match.Value} at index {match.Index}");
                string[] infos = match.Value.Split(" ");
                
                string valor = infos[0].Replace(".", "");
                valor = valor.Replace(",", ".");
                decimal decimalFromDot;
                Decimal.TryParse(valor, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out decimalFromDot);
                pdfData.Valor = decimalFromDot;

                pdfData.MesRef = infos[2];
                
                DateTime dt;
                bool isSuccess = DateTime.TryParseExact(infos[3], "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dt);
                pdfData.Vcto = dt;
            }


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

                pdfData.LeituraAnterior = PdfExtractorUtils.ParseStringToDate(infos[0]);
                pdfData.LeituraAtual = PdfExtractorUtils.ParseStringToDate(infos[1]);
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

                Log.Debug($"First match: {match.Value} at index {match.Index}");
                string[] infos = match.Value.Split(" ");
                //Unid - Qtd - Preço Unit c/trib - Valor(R$) - COFINS/PIS - Base Calc ICMS - Aliq ICMS - ICMS - Tarifa Unit
                //KWH 1.828,000 0,408884 747,44 31,89 747,44 17,00 127,06 0,321930
                //KWH 1.828,000 0,408884 747,44 31,89 747,44 17,00 127,06 0,321930 (0D) Consumo TE >> precisa pegar essa ultima descricao tb

                string texto = pageText.Substring(0, pageText.IndexOf(match.Value));
                Log.Debug(texto);
                string name = texto.Substring(texto.LastIndexOf("\n") + 1);
                Log.Debug("*********************************" + name);

                FaturaPdfLinha linha = new FaturaPdfLinha();
                linha.Descricao = name;

                linha.Unidade = infos[0];
                if (infos.Length == 10)
                {
                    //Linha Completa
                    //NOME - unid medida - qtd - tarifa aneel - tarifa com trib - valor - base icms - aliq icms - icms - pis - cofins
                    //Consumo - TE OUT/25 kWh 870,0000 0,30445000 0,39382759 342,63 342,63 17,00 58,25 3,50 16,01
                    linha.Qtd = PdfExtractorUtils.ParseStringToDecimal(infos[1]);
                    
                    //linha.PrecoUnit = PdfExtractorUtils.ParseStringToDecimal(infos[2]);
                    linha.Valor = PdfExtractorUtils.ParseStringToDecimal(infos[4]);
                    //linha.TarifaUnit = PdfExtractorUtils.ParseStringToDecimal(infos[8]);

                    linha.ICMSBaseCalc = PdfExtractorUtils.ParseStringToDecimal(infos[5]);
                    linha.ICMSAliq = PdfExtractorUtils.ParseStringToDecimal(infos[6]);
                    linha.ICMS = PdfExtractorUtils.ParseStringToDecimal(infos[7]);
                    linha.PIS = PdfExtractorUtils.ParseStringToDecimal(infos[8]);
                    linha.Cofins = PdfExtractorUtils.ParseStringToDecimal(infos[9]);
                } else if (linha.Descricao.IndexOf("Energ Atv Inj") >= 0)
                    {
                    linha.Qtd = PdfExtractorUtils.ParseStringToDecimal(infos[1]);

                    //linha.PrecoUnit = PdfExtractorUtils.ParseStringToDecimal(infos[2]);
                    linha.Valor = PdfExtractorUtils.ParseStringToDecimal(infos[4]);
                    //linha.TarifaUnit = PdfExtractorUtils.ParseStringToDecimal(infos[8]);
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

            Page page = document.GetPage(1);

            string TextOut1 = ContentOrderTextExtractor.GetText(page, true);
            TextOut1 = Regex.Replace(TextOut1.Trim(), @"[ ]+", " ");
            TextOut1 = TextOut1.Normalize(NormalizationForm.FormD);
            TextOut1 = Regex.Replace(TextOut1, @"\p{Mn}+", "", RegexOptions.None);

            Log.Debug(TextOut1);

            if (!FindUC(TextOut1, pdfData)) { }
            GetDadosLeitura(TextOut1, pdfData);

            /*Word anchorEmissao = PdfExtractorUtils.FindWordByText(page, "Emissao:", new PdfRectangle(page.Width*0.75, 600, page.Width, page.Height));
            if (anchorEmissao != null)
            {
                Log.Debug("Anchor:" + anchorEmissao.Text);
                string emissao = PdfExtractorUtils.FindOneTextByAnchor(page, anchorEmissao, 0, 50, 20, -10);
                if (!String.IsNullOrEmpty(emissao)) pdfData.DataEmissao = PdfExtractorUtils.ParseStringToDate(emissao);
            }
            else pdfData.ErrorMessage = "Data de Emissao não localizada no documento";*/

            Match match = Regex.Match(TextOut1, @"\d{48}|(\d{5}.\d{10}.\d{11}.\d{21})|(\d{12}[ ]\d{12}[ ]\d{12}[ ]\d{12})");
            if (match.Success)
            {
                pdfData.CodBarras = match.Value.Replace(".", "").Replace(" ", "");
            }

            GetLinhasFatura(pdfData, TextOut1);

                /*int index = 0;
            foreach (Word word in page.GetWords())
            {
                //Log.Debug($"Word: '{word.Text}' '{word.Text.ToLower()}' [{"comunicado".Equals(word.Text.ToLower())}], BoundingBox: {word.BoundingBox}");

                if ("comunicado".Equals(word.Text.ToLower()))
                {
                    //Log.Debug($"Word: '{word.Text}', BoundingBox: {word.BoundingBox}");
                    Word nextWord = page.GetWords().ElementAt(index + 1);
                    if ("importante".Equals(nextWord.Text.ToLower()))
                    {
                        //encontrou seção divisora importante
                        //Log.Debug("Fatiar seção dados do cliente");
                        GetDadosCliente(page, pdfData);
                    }

                } else if ("consumo".Equals(word.Text.ToLower()))
                {
                    Word nextWord = page.GetWords().ElementAt(index + 1);
                    if ("faturado".Equals(nextWord.Text.ToLower()))
                    {
                        GetLinhasFatura(page, pdfData, word.BoundingBox.Top);
                    }

                }
                index++;
            }

            Page page2 = document.GetPage(2);
            string TextOut2 = ContentOrderTextExtractor.GetText(page2, true);
            string textoNormalizado = TextOut2.Normalize(NormalizationForm.FormD);
            string textoSemAcentos = Regex.Replace(textoNormalizado, @"\p{Mn}+", "", RegexOptions.None);
            Log.Debug(textoSemAcentos.Trim());

            match = Regex.Match(textoSemAcentos, @"Saldo Final Beneficiaria \d{1,7}");
            if (match.Success)
            {
                Log.Debug("SALDO>>>>"+match.Value);
                match = Regex.Match(match.Value, @"\d{1,7}");
                if (match.Success)
                {
                    pdfData.EnergiaSaldo = PdfExtractorUtils.ParseStringToInt(match.Value);
                }
            }*/
        }

    }
}
