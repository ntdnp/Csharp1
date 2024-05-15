using DemoApp.Domain.Abstractions;
using DemoApp.Persistence;
using DemoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IDbContextTransaction> BeginTransactionAsync()
		{
			if (_context.Database.CurrentTransaction != null)
			{
				return null;
			}
			return await _context.Database.BeginTransactionAsync();
		}

		public async Task SaveChangeAsync()
		{
			await _context.SaveChangesAsync();
		}

	}
}
