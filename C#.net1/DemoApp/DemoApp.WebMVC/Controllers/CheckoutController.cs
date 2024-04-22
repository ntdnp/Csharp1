using Microsoft.AspNetCore.Mvc;

namespace DemoApp.WebMVC.Controllers
{
	public class CheckoutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

