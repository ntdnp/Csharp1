using DemoApp.Domain.Enums;

namespace DemoApp.Domain.Models.Checkouts
{
    public class CustomerInfoModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}
