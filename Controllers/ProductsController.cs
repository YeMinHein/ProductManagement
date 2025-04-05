using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.ViewModel;

namespace ProductManagement.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductRepository _productRepository;

        public ProductsController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
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

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateProduct(product);
                    int lastPage = TempData["LastPage"] != null ? (int)TempData["LastPage"] : 1;
                    return RedirectToAction(nameof(Index), new { page = lastPage });
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes. Please try again.");
                }
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteProduct(id);
            int lastPage = TempData["LastPage"] != null ? (int)TempData["LastPage"] : 1;
            return RedirectToAction(nameof(Index), new { page = lastPage });
        }
    }
}