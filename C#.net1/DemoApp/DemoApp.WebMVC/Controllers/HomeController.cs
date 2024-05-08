using DemoApp.Domain.Enums;
using DemoApp.Domain.Helpers;
using DemoApp.Domain.Services;
using DemoApp.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DemoApp.WebMVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;

		public HomeController(ILogger<HomeController> logger,
			IProductService productService,
			ICategoryService categoryService)
		{
			_logger = logger;
			_productService = productService;
			_categoryService = categoryService;
		}

		public IActionResult Index()
		{
			var model = new ProductListingPageModel();
			model.Categories = _categoryService.GetCategories();
			model.SelectPageSize = new List<int> { 6, 9, 18, 27, 36 };
			model.OrderBys = EnumHelper.GetList(typeof(SortEnum));
			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}