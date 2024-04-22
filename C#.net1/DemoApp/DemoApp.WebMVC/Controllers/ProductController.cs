using Microsoft.AspNetCore.Mvc;

namespace DemoApp.WebMVC.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult Detail()
		{
			return View();
		}
	}
}
