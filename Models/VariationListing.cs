using System.ComponentModel.DataAnnotations;

namespace EbayBulk_Generator.Models
{
    public class VariationListing
    {
        public string Action { get; set; } = "Add";
        [Required]
        public string CustomLabel { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Relationship { get; set; } = "Variation";
        public string RelationshipDetails { get; set; } = string.Empty;
        [Required]
        public decimal StartPrice { get; set; }
        [Required]
        public int Quantity { get; set; } = 999;
        public string? PicURL { get; set; }
        public string? Category { get; set; }
        // Attribute als Dictionary oder String
        public string Attributes { get; set; } = string.Empty;
    }
}
