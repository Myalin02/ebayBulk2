using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using EbayBulk_Generator.Models;

namespace EbayBulk_Generator.Services
{
    public interface IListingService
    {
        List<VariationListing> ImportVariations(string path, string parentTitle);
    }

    public class ListingService : IListingService
    {
        public List<VariationListing> ImportVariations(string path, string parentTitle)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                Encoding = new System.Text.UTF8Encoding(true),
                HasHeaderRecord = true
            };
            using var reader = new StreamReader(path, new System.Text.UTF8Encoding(true));
            using var csv = new CsvReader(reader, config);
            var records = new List<VariationListing>();
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var title = parentTitle + " " + csv.GetField("Attribute");
                title = Helpers.TitleShortener.Shorten(title);
                var sku = Helpers.SkuNormalizer.Normalize(csv.GetField("SKU"));
                var preis = decimal.Parse(csv.GetField("Preis"), CultureInfo.InvariantCulture);
                var attrs = csv.GetField("Attribute");
                records.Add(new VariationListing
                {
                    CustomLabel = sku,
                    Title = title,
                    StartPrice = preis,
                    Quantity = 999,
                    Attributes = attrs,
                    Relationship = "Variation",
                    RelationshipDetails = attrs
                });
            }
            return records;
        }
    }
}
