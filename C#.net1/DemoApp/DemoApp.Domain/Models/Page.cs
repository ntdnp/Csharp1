using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Domain.Models
{
    public class Page
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int SkipNumber
        {
            get
            {
                return (PageIndex - 1) * PageSize;
            }
        }
    }
}
