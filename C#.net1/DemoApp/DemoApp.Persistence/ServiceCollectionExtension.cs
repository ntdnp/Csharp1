using DemoApp.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Persistence
{
	public static class ServiceCollectionExtension
	{
		public static void AddRepositoryUnitOfWork(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
			serviceCollection.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
		}

		public static void AddEfCore(this IServiceCollection serviceCollection, IConfiguration configuration)
		{
			var assembly = typeof(ApplicationDbContext).Assembly.GetName().Name;

			serviceCollection.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration["DefaultConnection"], b => b.MigrationsAssembly(assembly)));
		}
	}
}
