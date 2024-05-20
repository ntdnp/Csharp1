using DemoApp.Domain.Abstractions;
using DemoApp.Domain.Entities;
using DemoApp.Domain.Enums;
using DemoApp.Domain.Models;
using DemoApp.Domain.Models.Products;
using DemoApp.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Products
{
	public class ProductService : IProductService
	{
		private readonly IGenericRepository<Product, Guid> _productRepository;
		private readonly IGenericRepository<Category, Guid> _categoryRepository;
		private readonly IGenericRepository<Review, Guid> _reviewRepository;
		private readonly IGenericRepository<ProductImage, Guid> _imageRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(
			IGenericRepository<Product, Guid> productRepository,
			IGenericRepository<Category, Guid> categoryRepository,
			IGenericRepository<Review, Guid> reviewRepository,
			IGenericRepository<ProductImage, Guid> imageRepository,
			IUnitOfWork unitOfWork)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
			_reviewRepository = reviewRepository;
			_imageRepository = imageRepository;
			_unitOfWork = unitOfWork;
		}
		public async Task<GenericData<ProductViewModel>> GetProducts(ProductPage filter)
		{
			var data = new GenericData<ProductViewModel>();
			var products = _productRepository.FindAll();
			var categories = _categoryRepository.FindAll();

			var result = (from p in products
						  join c in categories
						  on p.CategoryId equals c.Id
						  select new ProductViewModel
						  {
							  ProductId = p.Id,
							  ProductName = p.Name,
							  Price = p.Price,
							  DiscountPrice = p.DiscountPrice,
							  CategoryName = c.Name,
							  CategoryId = c.Id,
						  });

			if (!string.IsNullOrEmpty(filter.CategoryId) && Guid.TryParse(filter.CategoryId, out Guid categoryId))
			{
				result = result.Where(s => s.CategoryId == categoryId);
			}

			if (filter.FromPrice.HasValue)
			{
				result = result.Where(s => s.Price >= filter.FromPrice.Value);
			}

			if (filter.ToPrice.HasValue)
			{
				result = result.Where(s => s.Price <= filter.ToPrice.Value);
			}


			if (!string.IsNullOrEmpty(filter.KeyWord))
			{
				result = result.Where(s => s.ProductName.Contains(filter.KeyWord)
									  || s.CategoryName.Contains(filter.KeyWord));
			}
			if (filter.SortBy.Equals(SortEnum.Price))
			{
				result = result.OrderBy(s => s.Price);
			}
			else
			{
				result = result.OrderBy(s => s.ProductName);
			}

			// lấy ra số lượng product để tính số trang
			data.Count = await result.CountAsync();

			// lấy ra danh sách product ứng với PageIndex truyền vào (lúc đầu là 1)
			var productViewModels = await result.Skip(filter.SkipNumber).Take(filter.PageSize).ToListAsync();

			// lấy ra imageurl và rating
			var images = _imageRepository.FindAll();
			var reviews = _reviewRepository.FindAll();


			foreach (var item in productViewModels)
			{
				var image = (await images.FirstOrDefaultAsync(s => s.ProductId == item.ProductId))?.ImageLink;
				item.ImageUrl = string.IsNullOrEmpty(image) ? string.Empty : image;

				var productReviews = reviews.Where(s => s.ProductId == item.ProductId);
				if (productReviews != null && await productReviews.AnyAsync())
				{
					item.Rating = await productReviews.MaxAsync(s => s.Rating);
				}
			}

			//gán danh sách product vào data
			data.Data = productViewModels;
			return data;
		}

		public async Task<ProductDetailViewModel> GetProductDetail(Guid productId)
		{
			var product = await _productRepository.FindById(productId);
			if (product == null)
			{
				return null;
			}
			var result = new ProductDetailViewModel();
			result.Id = product.Id;
			result.Name = product.Name;
			result.Description = product.Description;
			result.Quantity = product.Quantity;
			result.Price = product.Price;
			result.DiscountPrice = product.DiscountPrice;
			result.CategoryId = product.CategoryId;
			result.CategoryName = await GetCategory(product.CategoryId);
			result.Reviews = await GetReviewModels(productId);
			result.Images = await GetImageModels(productId);
			return result;
		}

		public async Task CreateProduct(ProductCreateViewModel model)
		{
			using var transaction = await _unitOfWork.BeginTransactionAsync();
			try
			{
				var product = new Product
				{
					Name = model.ProductName,
					Description = model.Description,
					Detail = model.Detail,
					Price = model.Price,
					DiscountPrice = model.DiscountPrice,
					Quantity = model.Quantity,
					CategoryId = model.CategoryId,
					Id = Guid.NewGuid(),
					CreatedDate = DateTime.Now,
					Status = EntityStatus.Active,
				};
				_productRepository.Add(product);
				await _unitOfWork.SaveChangeAsync();
				foreach (var item in model.ImageUrls)
				{
					var image = new ProductImage
					{
						Id = Guid.NewGuid(),
						ImageLink = item,
						CreatedDate = product.CreatedDate,
						Status = EntityStatus.Active,
						ProductId = product.Id,
						Alt = product.Name

					};
					_imageRepository.Add(image);
					await _unitOfWork.SaveChangeAsync();
				}
				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				throw new Exception(ex.Message);
			}

		}


		private async Task<string> GetCategory(Guid categoryId)
		{
			var category = await _categoryRepository.FindById(categoryId);
			return category != null ? category.Name : string.Empty;
		}

		private async Task<List<ReviewModel>> GetReviewModels(Guid productId)
		{
			var result = await _reviewRepository.FindAll()
				.Where(s => s.ProductId == productId)
				.Select(x => new ReviewModel
				{
					Id = x.Id,
					Content = x.Content,
					ReviewerName = x.ReviewerName,
					Email = x.Email,
					Rating = x.Rating

				}).ToListAsync();
			return result;
		}

		private async Task<List<ImageViewModel>> GetImageModels(Guid productId)
		{

			var result = await _imageRepository.FindAll()
				.Where(s => s.ProductId == productId)
				.Select(x => new ImageViewModel
				{
					Id = x.Id,
					ImageLink = x.ImageLink,
					Alt = x.Alt,
				}).ToListAsync();
			return result.ToList();
		}



	}
}
