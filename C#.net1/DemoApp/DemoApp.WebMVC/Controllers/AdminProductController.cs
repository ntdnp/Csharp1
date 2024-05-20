using DemoApp.Domain.Models;
using DemoApp.Domain.Models.Products;
using DemoApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DemoApp.WebMVC.Controllers
{
    //[Authorize(Roles ="admin")]
    public class AdminProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private const string ImageFolder = "product-images";

        public AdminProductController(IWebHostEnvironment webHostEnvironment, IProductService productService, ICategoryService categoryService)
        {
            _webHostEnvironment = webHostEnvironment;
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = _categoryService.GetCategories();
            return View(categories);
        }

        public IActionResult Create()
        {
            var categories = _categoryService.GetCategories();
            return View(categories);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.ImageUrls = await SaveImage(model.Images);
            try
            {
                await _productService.CreateProduct(model);
                TempData["response"] = JsonConvert.SerializeObject(new ResponseResult(200, $"Create the {model.ProductName} sucessfully"));
            }
            catch (Exception)
            {
                TempData["response"] = JsonConvert.SerializeObject(new ResponseResult(400, $"Some thing when wrong"));
            }


            return RedirectToAction("Index", "AdminProduct");

        }

        public async Task<IActionResult> ProductListPartial([FromBody] ProductPage model)
        {
            var result = await _productService.GetProducts(model);
            return PartialView(result);
        }

        private async Task<List<string>> SaveImage(List<IFormFile> images)
        {
            var imageLinks = new List<string>();
            foreach (var image in images)
            {
                string sWebRootFolder = _webHostEnvironment.WebRootPath;
                string directory = Path.Combine(sWebRootFolder, ImageFolder);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string fileName = $"{Guid.NewGuid()}-{image.FileName}";
                string fileUrl = $"{Request.Scheme}://{Request.Host}/{ImageFolder}/{fileName}";
                using var stream = new FileStream(Path.Combine(directory, fileName), FileMode.Create);
                await image.CopyToAsync(stream);
                imageLinks.Add(fileUrl);
            }
            return imageLinks;
        }
    }
}
