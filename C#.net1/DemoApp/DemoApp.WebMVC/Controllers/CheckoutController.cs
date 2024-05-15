using Application.Checkouts;
using DemoApp.Domain.Constants;
using DemoApp.Domain.Enums;
using DemoApp.Domain.Helpers;
using DemoApp.Domain.Models.Checkouts;
using DemoApp.Domain.Models;
using DemoApp.Domain.Services;
using DemoApp.WebMVC.Extension;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DemoApp.WebMVC.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly IBillService _billService;
		public CheckoutController(IBillService billService)
		{
			_billService = billService;
		}
		public IActionResult Index()
		{
			var model = new CheckoutViewModel();
			var cart = HttpContext.Session.GetCart(CommonConstants.Cart);
			model.Items = cart;
			model.PaymentMethod = EnumHelper.GetList(typeof(PaymentMethod));
			return View(model);
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> PlaceOrder(CustomerInfoModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var cart = HttpContext.Session.GetCart(CommonConstants.Cart);
			if (cart == null)
			{
				return Json(new ResponseResult(400, "cart is empty"));
			}
			BillCreateViewModel billModel = new BillCreateViewModel();
			billModel.FirstName = model.FirstName;
			billModel.LastName = model.LastName;
			billModel.Email = model.Email;
			billModel.PhoneNumber = model.PhoneNumber;
			billModel.Address = model.Address;
			billModel.PaymentMethod = model.PaymentMethod;
			billModel.BillDetails = cart.Select(s => new BillDetailCreateViewModel
			{
				Price = s.Price,
				ProductName = s.ProductName,
				Quantity = s.Quantity,
			}).ToList();
			try
			{
				await _billService.CreateBill(billModel);
				var response = new ResponseResult(200, "Place order success");
				HttpContext.Session.Remove(CommonConstants.Cart);
				TempData["checkout"] = JsonConvert.SerializeObject(response);
				return RedirectToAction("Index", "Checkout");
			}
			catch (Exception ex)
			{
				var response = new ResponseResult(400, "Some thing went wrong");
				TempData["checkout"] = JsonConvert.SerializeObject(response);
				return RedirectToAction("Index", "Checkout");
			}


		}
	}
}
