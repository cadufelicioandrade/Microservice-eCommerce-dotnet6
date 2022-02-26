using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.Web.Models;
using ShoppingStore.Web.Services.Interfaces;
using ShoppingStore.Web.Utils;

namespace ShoppingStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        public async Task<IActionResult> IndexProduct()
        {
            IEnumerable<ProductViewModel> products = null;

            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("") ?? "";
                products = await _productService.GetAllProduct(accessToken);
            }

            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
                var response = await _productService.CreateProduct(model, accessToken);

                if (response != null)
                    return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> UpdateProduct(long id)
        {
            ProductViewModel model = null;

            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
                model = await _productService.GetProductById(id, accessToken);
            }

            if (model != null)
                return View(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
                var response = await _productService.UpdateProduct(model, accessToken);

                if (response != null)
                    return RedirectToAction(nameof(IndexProduct));
            }

            return View();
        }

        public async Task<IActionResult> DeleteProduct(long id)
        {
            ProductViewModel model = null;

            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
                model = await _productService.GetProductById(id, accessToken);
            }

            if (model != null)
                return View(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteProduct(ProductViewModel model)
        {
            if(model == null) 
                return RedirectToAction(nameof(IndexProduct));

            var accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
            var status = await _productService.DeleteProductId(model.Id, accessToken);

            if (status)
                return RedirectToAction(nameof(IndexProduct));

            return View(model);
        }
    }
}
