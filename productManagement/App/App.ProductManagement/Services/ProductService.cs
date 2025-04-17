using App.ProductManagement.Entities;
using System.Text.Json;

namespace App.ProductManagement.Services
{
    public class ProductService
    {
        private readonly string _filePath = "Data/products.json";

        private async Task<List<ProductEntity>> ReadFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<ProductEntity>();
            }
            using StreamReader reader = new StreamReader(_filePath);
            string json = await reader.ReadToEndAsync();
            var products = JsonSerializer.Deserialize<List<ProductEntity>>(json);
            return products ?? new();
        }
        private async Task WriteToFileAsync(List<ProductEntity> products)
        {
            using FileStream stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await JsonSerializer.SerializeAsync(stream, products, new JsonSerializerOptions { WriteIndented = true });
        }
        public async Task<List<ProductEntity>> GetAllAsync()
        {
            return await ReadFromFileAsync();
        }
        public async Task<ProductEntity?> GetByIdAsync(int id)
        {
            var products = await ReadFromFileAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }
        public async Task AddAsync(ProductEntity product)
        {
            var products = await ReadFromFileAsync();
            product.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            products.Add(product);
            await WriteToFileAsync(products);
        }
        public async Task<bool> UpdateAsync(int id, ProductEntity updatedProduct)
        {
            var products = await ReadFromFileAsync();
            var index = products.FindIndex(p => p.Id == id);
            if (index == -1) 
            {
                return false;
            }            
            updatedProduct.Id = id;
            products[index] = updatedProduct;
            await WriteToFileAsync(products);
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var products = await ReadFromFileAsync();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;
            products.Remove(product);
            await WriteToFileAsync(products);
            return true;
        }
        #region TEST
        public async Task AddBulkAsync(IEnumerable<ProductEntity> newProducts)
        {
            var products = await ReadFromFileAsync();
            int maxId = products.Any() ? products.Max(p => p.Id) : 0;
            foreach (var product in newProducts)
            {
                product.Id = ++maxId;
                products.Add(product);
            }
            await WriteToFileAsync(products);
        }
        #endregion
    }
}