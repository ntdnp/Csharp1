using DemoApp.Domain.Enums;

namespace DemoApp.Domain.Models.Checkouts
{
    public class BillCreateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<BillDetailCreateViewModel> BillDetails { get; set; }
    }
    public class BillDetailCreateViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
