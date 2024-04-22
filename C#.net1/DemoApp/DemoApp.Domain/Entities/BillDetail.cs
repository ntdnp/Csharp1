using DemoApp.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Domain.Entities
{
    [Table("BillDetails")]
    public class BillDetail : BaseEntity<Guid>
    {
        [Column(TypeName = "nvarchar(1000)")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        public Guid BillId { get; set; }

        [ForeignKey(nameof(BillId))]
        public Bill Bill { get; set; }

    }
}
