using DemoApp.Domain.Abstractions;
using DemoApp.Domain.Entities;
using DemoApp.Domain.Models.Checkouts;
using DemoApp.Domain.Services;

namespace DemoApp.Application.Services
{

	public class BillService : IBillService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<Bill, Guid> _billRepository;
		private readonly IGenericRepository<BillDetail, Guid> _billDetailRepository;

		public BillService(IUnitOfWork unitOfWork, IGenericRepository<Bill, Guid> billRepository, IGenericRepository<BillDetail, Guid> billDetailRepository)
		{
			_unitOfWork = unitOfWork;
			_billRepository = billRepository;
			_billDetailRepository = billDetailRepository;
		}
		public async Task CreateBill(BillCreateViewModel model)
		{
			using var transaction = await _unitOfWork.BeginTransactionAsync();
			if (transaction == null)
			{
				return;
			}
			try
			{
				var bill = new Bill
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Address = model.Address,
					Email = model.Email,
					Telephone = model.PhoneNumber,
					TotalAmount = model.BillDetails.Sum(s => s.Quantity * s.Price),
					PaymentMethod = model.PaymentMethod,
					Id = Guid.NewGuid(),
					CreatedDate = DateTime.Now,
					Status = Domain.Enums.EntityStatus.Active,

				};
				_billRepository.Add(bill);
				await _unitOfWork.SaveChangeAsync();
				foreach (var item in model.BillDetails)
				{
					var billDetail = new BillDetail
					{
						Id = Guid.NewGuid(),
						BillId = bill.Id,
						CreatedDate = bill.CreatedDate,
						Status = Domain.Enums.EntityStatus.Active,
						ProductName = item.ProductName,
						UnitPrice = item.Price,
						Quantity = item.Quantity,

					};
					_billDetailRepository.Add(billDetail);
					await _unitOfWork.SaveChangeAsync();
				}
				await transaction.CommitAsync();

			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				throw ex;
			}

		}
	}
}
