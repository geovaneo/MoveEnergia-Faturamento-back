using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto.General;
using MoveEnergia.Billing.Core.Interface.Service;
using Serilog;
using System.Globalization;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry;

namespace MoveEnergia.Billing.Extractor.Service
{
    public class PdfExtractorCelesc : IPdfExtractorByDistr
    {
        private readonly ILogger<PdfExtractorCelesc> _logger;
        public PdfExtractorCelesc(ILogger<PdfExtractorCelesc> logger)
        {
            _logger = logger;
        }
        public async Task<FaturaPdfData> ExtractInfo(int Id)
        {
            Log.Debug("extract1");
            FaturaPdfData pdfData = new FaturaPdfData();
            pdfData.ErrorMessage = "Teste";

            using (PdfDocument document = PdfDocument.Open(@"C:\\Temp\\moveenergia-faturas\\2025-10\7609-cekesc.pdf"))
            {
                processPDF(document, pdfData);
                document.Dispose();
            }

            return pdfData;
        }

        private bool FindUC(Page page, FaturaPdfData pdfData)
        {

            //encontra a âncora para a UC. Primeira seção, tem a palavra Cliente abaixo da UC
            Word anchor = PdfExtractorUtils.FindWordByText(page, "Cliente:", new PdfRectangle(200, 600, page.Width / 2, page.Height));
            if (anchor != null)
            {
                Log.Debug("Anchor:" + anchor.Text);
                pdfData.UC = PdfExtractorUtils.FindOneTextByAnchor(page, anchor, 20, 100, -50, -5);
            } else
            {
                pdfData.ErrorMessage = "UC não localizada no documento";
                return false;
            }

            return true;
        }

        private void GetDadosCliente(Page page, FaturaPdfData pdfData)
        {
            var areaWithoutBorders = new PdfRectangle(0, 600, page.Width/2, page.Height);
            Log.Debug($"x1: {0} y1: {600} x2:{page.Width / 2} y2:{page.Height} height:{page.Height}");

            var words = page.GetWords().Where(w => areaWithoutBorders.Contains(w.BoundingBox)).ToList();

            List<Word> sortedWords = words
                .OrderBy(w => page.Height - w.BoundingBox.Bottom) // Sort by the bottom edge of the bounding box
                .ThenBy(w => w.BoundingBox.Left)    // Then by the left edge of the bounding box
                .ToList();

            var pageText = string.Join(" ", sortedWords);
            Log.Debug(pageText);

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


        private void processPDF(PdfDocument document, FaturaPdfData pdfData)
        {

            int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
            /*string strX1 = getOneInfoDetailValueAsStr(cmd, "x1");
            if (!String.IsNullOrEmpty(strX1)) x1 = Convert.ToInt32(strX1);
            string strX2 = getOneInfoDetailValueAsStr(cmd, "x2");
            if (!String.IsNullOrEmpty(strX2)) x2 = Convert.ToInt32(strX2);
            string strY1 = getOneInfoDetailValueAsStr(cmd, "y1");
            if (!String.IsNullOrEmpty(strY1)) y1 = Convert.ToInt32(strY1);
            string strY2 = getOneInfoDetailValueAsStr(cmd, "y2");
            if (!String.IsNullOrEmpty(strY2)) y2 = Convert.ToInt32(strY2);
            Log.Debug("####X1:" + x1 + "//X2:" + x2 + "//Y1:" + y1 + "//Y2:" + y2);*/

            Page page = document.GetPage(1);
            if (!FindUC(page, pdfData)){

            }

            //string TextOut = ContentOrderTextExtractor.GetText(page, true);
            //Log.Debug(TextOut);

            //if (page != null)
            //foreach (Page page in document.GetPages())
            //{

            int index = 0;
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

                }
                index++;
            }

            /*string pageText = page.Text;
            if (!String.IsNullOrEmpty(TextOut)) TextOut += Environment.NewLine;
            TextOut += ContentOrderTextExtractor.GetText(page, true);*/

            //Log.Debug(TextOut);
            /*foreach (Word word in page.GetWords())
            {
                Log.Debug("Palavra:" + word.Text + "//" + word.BoundingBox.ToString());
                if (cmd.type1 == "WORDS")
                {
                    if (filterByPosition)
                    {
                        Log.Debug("####Left:" + word.BoundingBox.Left + "//Right:" + word.BoundingBox.Right + "//Top:" + word.BoundingBox.Top + "//Bot:" + word.BoundingBox.Bottom);
                        if (word.BoundingBox.Left >= x1 && word.BoundingBox.Right <= x2 && word.BoundingBox.Top >= y1 && word.BoundingBox.Bottom <= y2)
                        {
                            Log.Debug(">>>>>>Palavra:" + word.Text);
                            words.Add(word.Text);
                        }
                    }
                    else words.Add(word.Text);
                }
            }*/
            //}
            //Log.Debug(TextOut.Trim());
        }

    }
}
