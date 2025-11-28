using Serilog;
using System.Globalization;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry;

namespace MoveEnergia.Billing.Extractor.Service
{
    public class PdfExtractorUtils 
    {

        public static Word FindWordByText(Page page, string wordToFind, PdfRectangle area)
        {

            var words = page.GetWords().Where(w => area.Contains(w.BoundingBox)).ToList();

            foreach (Word word in words)
            {
                Log.Debug("Palavra:" + word.Text + "//" + word.BoundingBox.ToString());
                if (wordToFind.ToLower().Equals(word.Text.ToLower())) return word;
                
            }
            return null;
        }

        public static string FindOneTextByAnchor(Page page, Word anchor, double offsetX1, double offsetX2, double offsetY1, double offsetY2)
        {
            Log.Debug($"Anchor:{anchor.BoundingBox}");

            double center = anchor.BoundingBox.Left + (anchor.BoundingBox.Width / 2);

            double x1 = center + offsetX1;
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
            if (words != null && words.Count > 0) return words.ElementAt(0).Text;

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
