using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.ProductManagement.Entities
{
    public class ProductEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,StringLength(maximumLength:50,MinimumLength =2)]
        public string Name { get; set; } = default!;
        [Required,DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required, StringLength(maximumLength: 200, MinimumLength = 5)]
        public string Category { get; set; } = default!;
    }

}
