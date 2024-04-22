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
    
    [Table("Categories")]
    public class Category : BaseEntity<Guid>
    {
        [Column(TypeName = "nvarchar(1000)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string Image { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
