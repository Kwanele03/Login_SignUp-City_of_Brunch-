﻿using Login_SignUp.Data.Service;
using Login_SignUp.Data.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login_SignUp.Controllers
{
    public class ProductController : Controller
    {
            private readonly IProductService _service;
            public ProductController(IProductService service)
            {
                _service = service;
            }

            [AllowAnonymous]
            public async Task<IActionResult> Filter(string searchString)
            {
                var allProducts = await _service.GetAllAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    var filteredResult = allProducts.Where(n => n.name.ToLower().Contains(searchString.ToLower()) || n.Category.ToLower().Contains(searchString.ToLower())).ToList();

                    var filteredResultNew = allProducts.Where(n => string.Equals(n.name, searchString, StringComparison.CurrentCultureIgnoreCase) || string.Equals(n.Category, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                    if (filteredResultNew.Count == 0)
                    {
                        return View("NotFound");
                    }

                    return View("Index", filteredResultNew);
                }

                return View("Index", allProducts);
            }

            [AllowAnonymous]
            public async Task<IActionResult> Index()
            {
                var data = await _service.GetAllProductAsync();
                return View(data);
            }

            public IActionResult Create()
            {
                return View();
            }

           /* [HttpPost]
            public async Task<IActionResult> Create([Bind("Logo, Item, Category, Specification, Price")] Product product)
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                await _service.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }*/


            [AllowAnonymous]
            public async Task<IActionResult> Details(int id)
            {
                var productDetails = await _service.GetByIdAsync(id);

                if (productDetails == null) return View("NotFound");
                return View(productDetails);
            }


            public async Task<IActionResult> Edit(int id)
            {
                var productDetails = await _service.GetByIdAsync(id);
                if (productDetails == null) return View("NotFound");
                return View(productDetails);
            }

         /*   [HttpPost]
            public async Task<IActionResult> Edit(int id, [Bind("Logo, Item, Category, Specification, Price")] Product product)
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                await _service.UpdateProductAsync(id, product);
                return RedirectToAction(nameof(Index));
            }*/


            public async Task<IActionResult> Delete(int id)
            {
                var productDetails = await _service.GetByIdAsync(id);
                if (productDetails == null) return View("NotFound");
                return View(productDetails);
            }

            [HttpPost, ActionName("Delete")]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var productDetails = await _service.GetByIdAsync(id);
                if (productDetails == null) return View("NotFound");

                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
    }
}
