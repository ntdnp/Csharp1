using DemoApp.Domain.Models.Carts;

namespace Application.Checkouts
{
	public class CheckoutViewModel
	{
		public List<CartItemViewModel> Items { get; set; }
		public Dictionary<int, string> PaymentMethod { get; set; }
	}
}