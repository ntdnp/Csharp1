using DemoApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Persistence
{
	public class ApplicationDbContext : IdentityDbContext
	{

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<Bill> Bills { get; set; }
		public DbSet<BillDetail> BillDetails { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}

	}
}
