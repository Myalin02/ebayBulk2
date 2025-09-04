using System.ComponentModel.DataAnnotations;

namespace EbayBulk_Generator.Models
{
    public class ParentListing
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        public string? ConditionID { get; set; }
        public string Relationship { get; set; } = "Variations";
        public string RelationshipDetails { get; set; } = string.Empty;
        // Weitere Felder nach Bedarf
    }
}
