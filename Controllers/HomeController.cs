using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data;
using ProductManagement.ViewModel;


namespace ProductManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductRepository _productRepository;

        public HomeController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            var totalItems = await _productRepository.GetTotalCount();
            var products = await _productRepository.GetAllProducts(page, pageSize);

            var model = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return View(model);
        }
    }
}