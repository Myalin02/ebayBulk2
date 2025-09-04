using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using EbayBulk_Generator.Models;

namespace EbayBulk_Generator.Services
{
    public interface ITemplateService
    {
        (List<string> headers, int headerLine, List<string> metaLines) LoadTemplate(string path);
    }

    public class TemplateService : ITemplateService
    {
        public (List<string> headers, int headerLine, List<string> metaLines) LoadTemplate(string path)
        {
            var lines = File.ReadAllLines(path);
            int headerLine = Helpers.HeaderFinder.FindHeaderLine(lines);
            if (headerLine < 0) throw new InvalidDataException("Kein gültiger Header gefunden.");
            var headers = lines[headerLine].Split(';').ToList();
            var metaLines = lines.Take(headerLine).ToList();
            return (headers, headerLine, metaLines);
        }
    }

    public interface ICsvExportService
    {
        void ExportEbayCsv(string path, ParentListing parent, IEnumerable<VariationListing> variations, List<string> templateHeaders);
    }

    public class CsvExportService : ICsvExportService
    {
        public void ExportEbayCsv(string path, ParentListing parent, IEnumerable<VariationListing> variations, List<string> templateHeaders)
        {
            using var writer = new StreamWriter(path, false, new System.Text.UTF8Encoding(true));
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false,
                Encoding = new System.Text.UTF8Encoding(true)
            });
            // Parent zuerst
            WriteRow(csv, parent, templateHeaders);
            // Dann alle Children
            foreach (var v in variations)
                WriteRow(csv, v, templateHeaders);
        }

        private void WriteRow(CsvWriter csv, object obj, List<string> headers)
        {
            foreach (var h in headers)
            {
                var prop = obj.GetType().GetProperties().FirstOrDefault(p =>
                    string.Equals(p.Name, h.Trim('*'), System.StringComparison.OrdinalIgnoreCase));
                var value = prop?.GetValue(obj)?.ToString() ?? string.Empty;
                csv.WriteField(value);
            }
            csv.NextRecord();
        }
    }
}
