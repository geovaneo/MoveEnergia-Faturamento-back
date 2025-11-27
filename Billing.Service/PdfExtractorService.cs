using Azure;
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
    public class PdfExtractorService : IPdfExtractorService
    {
        private readonly ILogger<PdfExtractorService> _logger;
        public PdfExtractorService(ILogger<PdfExtractorService> logger)
        {
            _logger = logger;
        }
        public async Task<FaturaPdfData> StartProcess(int Id)
        {
            Log.Debug("extract1");
            FaturaPdfData pdfData = new FaturaPdfData();
            pdfData.ErrorMessage = "Teste";

            string sourcePath = @"C:\Temp\moveenergia-faturas\2025-10";
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
                    i += 1;
                    Log.Debug("Arquivo[{0}]:{1}:Criado em:{2}", i, fileName, fileInfo.LastWriteTime);

                    break;
                }
            }


/*            using (PdfDocument document = PdfDocument.Open(@"C:\\Temp\\moveenergia-faturas\\2025-10\7609-cekesc.pdf"))
            {
                processPDF(document, pdfData);
                document.Dispose();
            }*/

            return pdfData;
        }

        
    }
}
