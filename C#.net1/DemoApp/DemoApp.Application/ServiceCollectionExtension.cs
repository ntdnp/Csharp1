using Application.Products;
using DemoApp.Application.Services;
using DemoApp.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Application
{
	public static class ServiceCollectionExtension
	{
		public static void AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IBillService, BillService>();
		}
	}
}