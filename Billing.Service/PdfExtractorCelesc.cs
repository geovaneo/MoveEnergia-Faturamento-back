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
    public class PdfExtractorCelesc : IPdfExtractorByDistr
    {
        public PdfExtractorCelesc()
        {
        }

        public string GetDistribuidora()
        {
            return "CELESC-DIS";
        }

        public async Task<FaturaPdfData> ExtractInfo(PdfDocument document)
        {
            Log.Debug("extract celesc");
            FaturaPdfData pdfData = new FaturaPdfData();
            pdfData.NomeDistribuidora = GetDistribuidora();

            processPDF(document, pdfData);

            return pdfData;
        }

        private bool FindUC(Page page, FaturaPdfData pdfData)
        {

            //encontra a âncora para a UC. Primeira seção, tem a palavra Cliente abaixo da UC
            Word anchor = PdfExtractorUtils.FindWordByText(page, "Cliente:", new PdfRectangle(200, 600, page.Width / 2, page.Height));
            if (anchor != null)
            {
                Log.Debug("Anchor:" + anchor.Text);
                pdfData.UC = PdfExtractorUtils.FindOneTextByAnchor(page, anchor, -20, 60, -40, -5);
            } else
            {
                pdfData.ErrorMessage = "UC não localizada no documento";
                return false;
            }

            return true;
        }

        private bool GetDadosCliente(Page page, FaturaPdfData pdfData)
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

            Word anchorNome = PdfExtractorUtils.FindWordByText(page, "NOME:", new PdfRectangle(0, 600, page.Width / 2, page.Height));
            if (anchorNome != null)
            {
                Log.Debug("Anchor:" + anchorNome.Text);
                pdfData.NomeCliente = PdfExtractorUtils.FindMultipleTextByAnchor(page, anchorNome, 10, 200, -3, 10);
            }
            else
            {
                pdfData.ErrorMessage = "NOME não localizado no documento";
                return false;
            }

            Word anchorCpf = PdfExtractorUtils.FindWordByText(page, "CPF/CNPJ:", new PdfRectangle(0, 600, page.Width / 2, page.Height));
            if (anchorCpf != null)
            {
                Log.Debug("Anchor:" + anchorCpf.Text);
                pdfData.CpfCnpj = PdfExtractorUtils.FindOneTextByAnchor(page, anchorCpf, 10, 150, -5, 10);
            }
            else
            {
                pdfData.ErrorMessage = "CPF/CNPJ não localizado no documento";
                return false;
            }

            return true;

        }

        private bool GetDadosLeitura(Page page, FaturaPdfData pdfData, double anchorPagina)
        {
            var areaWithoutBorders = new PdfRectangle(0, anchorPagina - 300, page.Width * 0.75, anchorPagina);
            //Log.Debug($"x1: {0} y1: {600} x2:{page.Width / 2} y2:{page.Height} height:{page.Height}");

            var words = page.GetWords().Where(w => areaWithoutBorders.Contains(w.BoundingBox)).ToList();

            List<Word> sortedWords = words
                .OrderBy(w => page.Height - w.BoundingBox.Bottom) // Sort by the bottom edge of the bounding box
                .ThenBy(w => w.BoundingBox.Left)    // Then by the left edge of the bounding box
                .ToList();

            var pageText = string.Join(" ", sortedWords);
            Log.Debug("GetDadosLeitura");
            Log.Debug(pageText);

            Match match = Regex.Match(pageText, @"(\d{2}\/\d{2}\/\d{4}) (\d{2}\/\d{2}\/\d{4}) (\d{2}\/\d{2}\/\d{4})*");
            if (match.Success)
            {
                Log.Debug($"First match: {match.Value} at index {match.Index}");
                string[] infos = match.Value.Split(" ");

                pdfData.LeituraAnterior = PdfExtractorUtils.ParseStringToDate(infos[0]);
                pdfData.LeituraAtual = PdfExtractorUtils.ParseStringToDate(infos[1]);
            }
            else
            {
                pdfData.ErrorMessage = "Leitura Anterior e Atual não identificados";
                return false;
            }

            /*match = Regex.Match(pageText, @"Energia \d{1,3}.\d{1,3} \d{1,3}.\d{1,3} \d{1,3},\d{1,5} \d{1,3},\d{1,3} (\d{1,3}\.)*\d{1,3}");
            if (match.Success)
            {
                Log.Debug($"First match: {match.Value} at index {match.Index}");
                string[] infos = match.Value.Split(" ");

                pdfData.EnergiaConsumida = PdfExtractorUtils.ParseStringToInt(infos[5]);
            }
            else
            {
                pdfData.ErrorMessage = "Energ. Consumida não identificada";
                return false;
            }*/

            match = Regex.Match(pageText, @"Energia injetada \d{1,3}.\d{1,3} \d{1,3}.\d{1,3} \d{1,3},\d{1,5} \d{1,3},\d{1,3} (\d{1,3}\.)*\d{1,3}");
            if (match.Success)
            {
                Log.Debug($"First match: {match.Value} at index {match.Index}");
                string[] infos = match.Value.Split(" ");

                pdfData.EnergiaPainel = PdfExtractorUtils.ParseStringToInt(infos[6]);
            }            

            return true;
        }

        private void GetLinhasFatura(Page page, FaturaPdfData pdfData, double anchorPagina)
        {
            pdfData.Linhas = new List<FaturaPdfLinha>();
            Log.Debug("************ LINHAS");
            var areaWithoutBorders = new PdfRectangle(0, anchorPagina - 400, page.Width * 0.75, anchorPagina);
            //Log.Debug($"x1: {0} y1: {600} x2:{page.Width / 2} y2:{page.Height} height:{page.Height}");

            var words = page.GetWords().Where(w => areaWithoutBorders.Contains(w.BoundingBox)).ToList();

            List<Word> sortedWords = words
                .OrderBy(w => page.Height - w.BoundingBox.Bottom) // Sort by the bottom edge of the bounding box
                .ThenBy(w => w.BoundingBox.Left)    // Then by the left edge of the bounding box
                .ToList();

            var pageText = string.Join(" ", sortedWords);
            Log.Debug("LINHAS");
            Log.Debug(pageText);

            Decimal energiaCompensada = 0;
            int energiaConsumida = 0;
            Decimal tarifaConsumo = 0;
            Decimal tarifaCompensada = 0;
            string texto = pageText.Trim();
            MatchCollection matches = Regex.Matches(pageText, @"KWH (\d{1,3}[.])*\d{1,3},\d{1,6} [-]*\d{1,2},\d{1,6} [-]*(\d{1,3}[.])*\d{1,3},\d{1,2} [-]*\d{1,3},\d{1,2} [-]*(\d{1,3}[.])*\d{1,3},\d{1,2} \d{1,3},\d{1,2} [-]*\d{1,3},\d{1,2} \d{1,3},\d{1,6}");
            foreach (Match match in matches)
            {

                Log.Debug($"First match: {match.Value} at index {match.Index}");
                string[] infos = match.Value.Split(" ");
                //Unid - Qtd - Preço Unit c/trib - Valor(R$) - COFINS/PIS - Base Calc ICMS - Aliq ICMS - ICMS - Tarifa Unit
                //KWH 1.828,000 0,408884 747,44 31,89 747,44 17,00 127,06 0,321930
                //KWH 1.828,000 0,408884 747,44 31,89 747,44 17,00 127,06 0,321930 (0D) Consumo TE >> precisa pegar essa ultima descricao tb

                FaturaPdfLinha linha = new FaturaPdfLinha();
                linha.Unidade = infos[0];
                linha.Qtd = PdfExtractorUtils.ParseStringToDecimal(infos[1]);
                linha.PrecoUnit = PdfExtractorUtils.ParseStringToDecimal(infos[2]);
                linha.Valor = PdfExtractorUtils.ParseStringToDecimal(infos[3]);
                linha.CofinsPIS = PdfExtractorUtils.ParseStringToDecimal(infos[4]);
                linha.ICMSBaseCalc = PdfExtractorUtils.ParseStringToDecimal(infos[5]);
                linha.ICMSAliq = PdfExtractorUtils.ParseStringToDecimal(infos[6]);
                linha.ICMS = PdfExtractorUtils.ParseStringToDecimal(infos[7]);
                linha.TarifaUnit = PdfExtractorUtils.ParseStringToDecimal(infos[8]);

                texto = texto.Substring(texto.IndexOf(match.Value) + match.Value.Length);
                Log.Debug(texto);
                int nextOrEnd = PdfExtractorUtils.IndexOfAny(texto, "KWH", "SUBTOTAL");
                if (nextOrEnd >= 0)
                {
                    Log.Debug("Nome:" + texto.Substring(0, nextOrEnd));
                    linha.Descricao = texto.Substring(0, nextOrEnd).Trim();

                    if (linha.Descricao.Replace(" ", string.Empty).IndexOf("ConsumoTE") >= 0)
                    {
                        energiaConsumida += Convert.ToInt32(linha.Qtd);
                        tarifaConsumo += (decimal)(Convert.ToInt32(linha.Qtd) * (linha.PrecoUnit != null ? linha.PrecoUnit : 0));
                    }
                    else if (linha.Descricao.Replace(" ", string.Empty).IndexOf("EnergiaInjet.TE") >= 0)
                    {
                        energiaCompensada += (decimal)(linha.Qtd != null ? linha.Qtd : 0);
                        tarifaCompensada = (decimal)linha.PrecoUnit;
                    }
                }
                else linha.Descricao = "NAO IDENT";

                pdfData.Linhas.Add(linha);
            }

            pdfData.EnergiaConsumida = energiaConsumida;
            pdfData.EnergiaCompensada = energiaCompensada - (pdfData.EnergiaPainel);
            pdfData.TarifaConsumo = Math.Round(tarifaConsumo / energiaConsumida, 6);
            pdfData.TarifaCompensada = tarifaCompensada;

        }

        private void processPDF(PdfDocument document, FaturaPdfData pdfData)
        {

            Page page = document.GetPage(1);
            if (!FindUC(page, pdfData)){

            }

            string TextOut1 = ContentOrderTextExtractor.GetText(page, true);
            Log.Debug(TextOut1);

            Word anchorEmissao = PdfExtractorUtils.FindWordByText(page, "Emissao:", new PdfRectangle(page.Width*0.75, 600, page.Width, page.Height));
            if (anchorEmissao != null)
            {
                Log.Debug("Anchor:" + anchorEmissao.Text);
                string emissao = PdfExtractorUtils.FindOneTextByAnchor(page, anchorEmissao, 0, 50, 20, -10);
                if (!String.IsNullOrEmpty(emissao)) pdfData.DataEmissao = PdfExtractorUtils.ParseStringToDate(emissao);
            }
            else pdfData.ErrorMessage = "Data de Emissao não localizada no documento";

            Match match = Regex.Match(TextOut1, @"\d{48}|(\d{5}.\d{10}.\d{11}.\d{21})");
            if (match.Success)
            {
                pdfData.CodBarras = match.Value.Replace(".", "");
            }

                int index = 0;
            bool secoesSuperiores = false;
            bool secaoLinhas = false;
            Log.Debug(">>>>>>>>>>>>>>> INICIO LINHAS e CABECALHO");
            foreach (Word word in page.GetWords())
            {
                Log.Debug($"Word: '{word.Text}' '{word.Text.ToLower()}' [{"comunicado".Equals(word.Text.ToLower())}], BoundingBox: {word.BoundingBox}");

                if ("comunicado".Equals(word.Text.ToLower()))
                {
                    //Log.Debug($"Word: '{word.Text}', BoundingBox: {word.BoundingBox}");
                    Word nextWord = page.GetWords().ElementAt(index + 1);
                    if (!secoesSuperiores && "importante".Equals(nextWord.Text.ToLower()))
                    {
                        //encontrou seção divisora importante
                        //Log.Debug("Fatiar seção dados do cliente");
                        GetDadosCliente(page, pdfData);
                        GetDadosLeitura(page, pdfData, word.BoundingBox.Top);
                        secoesSuperiores = true;
                    }

                } else if (!secoesSuperiores && "reaviso".Equals(word.Text.ToLower()))
                {
                    Word nextWord = page.GetWords().ElementAt(index + 1);
                    if ("de".Equals(nextWord.Text.ToLower()))
                    {
                        Word nextWord2 = page.GetWords().ElementAt(index + 2);
                        if ("débito".Equals(nextWord2.Text.ToLower()))
                        {
                            //encontrou seção divisora importante
                            GetDadosCliente(page, pdfData);
                            GetDadosLeitura(page, pdfData, word.BoundingBox.Top);
                            secoesSuperiores = true;
                        }
                    }

                }
                else if (!secaoLinhas && "consumo".Equals(word.Text.ToLower()))
                {
                    Word nextWord = page.GetWords().ElementAt(index + 1);
                    if ("faturado".Equals(nextWord.Text.ToLower()))
                    {
                        GetLinhasFatura(page, pdfData, word.BoundingBox.Top);
                        secaoLinhas = true;
                    } else if ("te".Equals(nextWord.Text.ToLower()))
                    {
                        GetLinhasFatura(page, pdfData, word.BoundingBox.Top + 30);
                        secaoLinhas = true;
                    }

                }
                index++;
                if (secoesSuperiores && secaoLinhas) break;
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
            } else if (document.NumberOfPages > 2)
            {
                Page pageLast = document.GetPage(document.NumberOfPages);
                string TextOutLast = ContentOrderTextExtractor.GetText(pageLast, true);
                textoNormalizado = TextOutLast.Normalize(NormalizationForm.FormD);
                textoSemAcentos = Regex.Replace(textoNormalizado, @"\p{Mn}+", "", RegexOptions.None);
                Log.Debug(textoSemAcentos.Trim());

                match = Regex.Match(textoSemAcentos, @"Saldo Final Beneficiaria \d{1,7}");
                if (match.Success)
                {
                    Log.Debug("SALDO>>>>" + match.Value);
                    match = Regex.Match(match.Value, @"\d{1,7}");
                    if (match.Success)
                    {
                        pdfData.EnergiaSaldo = PdfExtractorUtils.ParseStringToInt(match.Value);
                    }
                }
            }
        }

    }
}
