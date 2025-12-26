using Serilog;
using System.Globalization;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig.Geometry;

namespace MoveEnergia.Billing.Extractor.Service
{
    public class PdfExtractorUtils 
    {

        public static string REGEX_CODBARRAS = @"\d{48}|(\d{5}.(\d{5} \d{5}|\d{10}).(\d{6} \d{5}|\d{11}).(\d{6} \d{1} \d{14}|\d{21}))|(\d{12}[ ]\d{12}[ ]\d{12}[ ]\d{12})";

        public static (string, string) GetTextFromPage(Page page)
        {
            string pageText1 = ContentOrderTextExtractor.GetText(page, true);
            pageText1 = Regex.Replace(pageText1.Trim(), @"[ ]+", " ");
            pageText1 = pageText1.Normalize(NormalizationForm.FormD);
            pageText1 = Regex.Replace(pageText1, @"\p{Mn}+", "", RegexOptions.None);
            string pageText1NoBreakLines = Regex.Replace(pageText1.Trim(), @"\s+", " ");

            return (pageText1, pageText1NoBreakLines);
        }

        public static Word FindWordByText(Page page, string wordToFind, PdfRectangle area)
        {

            var words = page.GetWords().Where(w => area.Contains(w.BoundingBox)).ToList();

            foreach (Word word in words)
            {
                Log.Debug("Palavra:" + word.Text + "//" + word.BoundingBox.ToString());
                Log.Debug("Palavra:" + word.Text + "//####Left:" + word.BoundingBox.Left + "//Right:" + word.BoundingBox.Right + "//Top:" + word.BoundingBox.Top + "//Bot:" + word.BoundingBox.Bottom);
                if (wordToFind.ToLower().Equals(word.Text.ToLower())) return word;
                
            }
            return null;
        }

        public static string FindOneTextByAnchor(Page page, Word anchor, double offsetX1, double offsetX2, double offsetY1, double offsetY2)
        {
            Log.Debug($"Anchor:{anchor.BoundingBox}");

            double center = anchor.BoundingBox.Left + (anchor.BoundingBox.Width / 2);
            Log.Debug("anchor center:" + anchor.BoundingBox.Left + "//" + anchor.BoundingBox.Width+"//center:"+center);
            double x1 = center + offsetX1;
            Log.Debug("anchor x1:" + x1);
            double x2 = center + offsetX2;
            double y1 = anchor.BoundingBox.Top - offsetY1;
            double y2 = anchor.BoundingBox.Top - offsetY2;

            Log.Debug($"y1:{anchor.BoundingBox.Top} // {offsetY1} // {y1}");
            Log.Debug($"y2:{anchor.BoundingBox.Top} // {offsetY2} // {y2}");
            PdfRectangle searchArea = new PdfRectangle(x1, y1, x2, y2);
            Log.Debug($"searchArea:{searchArea}");

            var words = page.GetWords().Where(w => searchArea.Contains(w.BoundingBox)).ToList();
            foreach (Word word in words)
            {
                Log.Debug("Palavra:" + word.Text + "//####Left:" + word.BoundingBox.Left + "//Right:" + word.BoundingBox.Right + "//Top:" + word.BoundingBox.Top + "//Bot:" + word.BoundingBox.Bottom);
                /*if (word.BoundingBox.Left >= x1 && word.BoundingBox.Right <= x2)
                {
                    if (word.BoundingBox.Top <= y1 && word.BoundingBox.Bottom >= y2)
                    {
                        Log.Debug(">>>>>>Palavra:" + word.Text);
                        return word.Text;

                    }
                }*/
            }
            if (words != null && words.Count > 0)
            {
                Log.Debug(">>>>>>>>>>>>> Encontrou:"+ words.ElementAt(0).Text);
                return words.ElementAt(0).Text;
            }
            Log.Debug("########## Não encontrou a palavra");
            return null;
        }

        public static string FindMultipleTextByAnchor(Page page, Word anchor, double offsetX1, double offsetX2, double offsetY1, double offsetY2)
        {
            Log.Debug($"Anchor:{anchor.BoundingBox}");

            double center = anchor.BoundingBox.Left + (anchor.BoundingBox.Width / 2);
            Log.Debug("anchor center:" + anchor.BoundingBox.Left + "//" + anchor.BoundingBox.Width + "//center:" + center);
            double x1 = center + offsetX1;
            Log.Debug("anchor x1:" + x1);
            double x2 = center + offsetX2;
            double y1 = anchor.BoundingBox.Top - offsetY1;
            double y2 = anchor.BoundingBox.Top - offsetY2;

            Log.Debug($"y1:{anchor.BoundingBox.Top} // {offsetY1} // {y1}");
            Log.Debug($"y2:{anchor.BoundingBox.Top} // {offsetY2} // {y2}");
            PdfRectangle searchArea = new PdfRectangle(x1, y1, x2, y2);
            Log.Debug($"searchArea:{searchArea}");

            var words = page.GetWords().Where(w => searchArea.Contains(w.BoundingBox)).ToList();
            foreach (Word word in words)
            {
                Log.Debug("Palavra:" + word.Text + "//####Left:" + word.BoundingBox.Left + "//Right:" + word.BoundingBox.Right + "//Top:" + word.BoundingBox.Top + "//Bot:" + word.BoundingBox.Bottom);
            }
            if (words != null && words.Count > 0)
            {
                Log.Debug(">>>>>>>>>>>>> Encontrou:" + String.Join(" ", words));
                return String.Join(" ", words);
            }
            Log.Debug("########## Não encontrou a palavra");
            return null;
        }

        public static List<string> FindWordsByAnchor(Page page, Word anchor, double offsetX1, double offsetX2, double offsetY1, double offsetY2)
        {
            List<string> words = new List<string>();

            double center = anchor.BoundingBox.Left + (anchor.BoundingBox.Width / 2);

            double x1 = center + offsetX1;
            double x2 = center + offsetX2;
            double y1 = anchor.BoundingBox.Top - offsetY1;
            double y2 = anchor.BoundingBox.Top - offsetY2;

            foreach (Word word in page.GetWords())
            {
                Log.Debug("Palavra:" + word.Text + "//####Left:" + word.BoundingBox.Left + "//Right:" + word.BoundingBox.Right + "//Top:" + word.BoundingBox.Top + "//Bot:" + word.BoundingBox.Bottom);
                if (word.BoundingBox.Left >= x1 && word.BoundingBox.Right <= x2)
                {
                    if (word.BoundingBox.Top <= y1 && word.BoundingBox.Bottom >= y2)
                    {
                        Log.Debug(">>>>>>Palavra:" + word.Text);
                        words.Add(word.Text);
                        
                    }
                }
            }
            return words;
        }


        public static string GetMD5Checksum(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static DateTime ParseStringToDate(string texto)
        {
            DateTime dt;
            bool isSuccess = DateTime.TryParseExact(texto, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dt);
            return dt;
        }

        public static string ParseMonthStringToNumber(string dateRef)
        {
            string[] refs = dateRef.Split("/");
            string month = refs[0].ToUpper();
            if ("JAN".Equals(month)) return "01/" + refs[1];
            else if ("FEV".Equals(month)) return "02/" + refs[1];
            else if ("MAR".Equals(month)) return "03/" + refs[1];
            else if ("ABR".Equals(month)) return "04/" + refs[1];
            else if ("MAI".Equals(month)) return "05/" + refs[1];
            else if ("JUN".Equals(month)) return "06/" + refs[1];
            else if ("JUL".Equals(month)) return "07/" + refs[1];
            else if ("AGO".Equals(month)) return "08/" + refs[1];
            else if ("SET".Equals(month)) return "09/" + refs[1];
            else if ("OUT".Equals(month)) return "10/" + refs[1];
            else if ("NOV".Equals(month)) return "11/" + refs[1];
            else if ("DEZ".Equals(month)) return "12/" + refs[1];
            return "";
        }

        public static Decimal ParseStringToDecimal(string texto)
        {
            string valor = texto.Replace(".", "");
            valor = valor.Replace(",", ".");
            decimal decimalFromDot;
            Decimal.TryParse(valor, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out decimalFromDot);
            return decimalFromDot;
        }

        public static int ParseStringToInt(string texto)
        {
            string valor = texto.Replace(".", "");
            return Convert.ToInt32(valor);
        }

        public static int IndexOfAny(string source, params string[] valuesToFind)
        {
            int minIndex = -1;

            foreach (string c in valuesToFind)
            {
                int currentIndex = source.ToLower().IndexOf(c.ToLower());
                if (currentIndex != -1)
                {
                    if (minIndex == -1 || currentIndex < minIndex)
                    {
                        minIndex = currentIndex;
                    }
                }
            }
            return minIndex;
        }

    }
}
