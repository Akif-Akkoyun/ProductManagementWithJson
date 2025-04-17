using App.ProductManagement.Entities;
using App.ProductManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdcutsController(ProductService _productService) : ControllerBase
    {
        [HttpGet("/product-list")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            if (products is null || !products.Any())
            {
                return NotFound("Listelenecek ürün bulunamadı...");
            }
            return Ok(products);
        }
        [HttpGet("/get-product-by/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound("Belirtilen ID ile ürün bulunamadı.");
            }
            return Ok(product);
        }
        [HttpPost("/create-product")]
        public async Task<IActionResult> Add(ProductEntity product)
        {
            try
            {
                await _productService.AddAsync(product);
                return Ok("Ekleme işlemi başarılı...");
            }
            catch (Exception)
            {

                throw new InvalidOperationException("Ekleme işlemi başarısz...");
            }
        }
        [HttpPut("/update-product")]
        public async Task<IActionResult> Update(int id, ProductEntity product)
        {
            var result = await _productService.UpdateAsync(id, product);
            if (!result)
            {
                return NotFound("Belirtilen ID ile ürün bulunamadı.");
            }
            return Ok("Güncelleme işlemi başarılı.");
        }
        [HttpDelete("/remove-product-by/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
            {
                return NotFound("Belirtilen ID ile ürün bulunamadı.");
            }
            return Ok("Silme işlemi başarılı.");
        }
        #region Api Data Seed
        [HttpPost("/data-seed")]
        public async Task<IActionResult> SeedProduct()
        {
            var rnd = new Random();
            var productList = new List<ProductEntity>();

            for (int i = 1; i <= 10000; i++)
            {
                productList.Add(new ProductEntity
                {
                    Name = $"Product {i}",
                    Price = rnd.Next(10, 1000),
                    Category = $"Category {i}."
                });
            }
            await _productService.AddBulkAsync(productList);
            return Ok("10.000 adet sahte data eklendi.");
        }
        #endregion
    }
}
