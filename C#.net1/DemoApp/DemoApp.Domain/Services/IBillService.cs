using DemoApp.Domain.Models.Checkouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Domain.Services
{
    public interface IBillService
    {
        Task CreateBill(BillCreateViewModel model);
    }
}
