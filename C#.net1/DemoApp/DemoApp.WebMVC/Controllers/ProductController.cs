using DemoApp.Domain.Models.Products;
using DemoApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.WebMVC.Controllers
{
	public class ProductController : Controller

	{

		private readonly IProductService _productService;
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		public IActionResult Detail()
		{
			return View();
		}

		public async Task<IActionResult> ProductListPartial([FromBody] ProductPage model)
		{
			var result = await _productService.GetProducts(model);
			return PartialView(result);
		}
	}
}
