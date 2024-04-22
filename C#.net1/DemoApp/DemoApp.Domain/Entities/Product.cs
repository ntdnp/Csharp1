using DemoApp.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Domain.Entities
{

    [Table("Products")]
    public class Product : BaseEntity<Guid>
    {
        [Column(TypeName = "nvarchar(1000)")]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 0;

        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        public Guid CategoryId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountPrice { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
