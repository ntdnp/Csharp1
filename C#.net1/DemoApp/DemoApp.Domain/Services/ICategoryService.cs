using DemoApp.Domain.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Domain.Services
{
	public interface ICategoryService
	{
		List<CategoryViewModel> GetCategories();
	}
}
