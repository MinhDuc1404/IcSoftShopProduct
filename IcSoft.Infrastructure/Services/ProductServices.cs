using IcSoft.Infrastructure.Migrations;
using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services
{
    public class ProductServices : IProductServices
    {
        private readonly ApplicationDbContext _Context;
        public ProductServices(ApplicationDbContext context)
        {
            _Context = context;
        }
        public async Task<Product> AddProduct(Product product)
        {
            _Context.Add(product);
            await _Context.SaveChangesAsync();
            return product;
        }
		public async Task DeleteProduct(Product product)
		{
			_Context.Remove(product);
			await _Context.SaveChangesAsync();
		}
		public async Task<Product> UpdateProduct(Product product)
		{
			_Context.Update(product);
			await _Context.SaveChangesAsync();
			return product;
		}
		public async Task<ProductImage> AddProductImage(ProductImage productImage)
        {
            _Context.Add(productImage);
            await _Context.SaveChangesAsync();
            return productImage;
        }
		public async Task<ProductImage> UpdateProductImage(ProductImage productImage)
		{
			_Context.Update(productImage);
			await _Context.SaveChangesAsync();
			return productImage;
		}
        public async Task<ProductImage> FindProductImageByImageId(int id)
        {
            return await _Context.ProductImages.FirstOrDefaultAsync(pi => pi.ImageId == id);
        }
        public async Task DeleteProductImages(int productId)
        {
            var productImages = _Context.ProductImages.Where(pc => pc.ProductId == productId).ToList();

            _Context.ProductImages.RemoveRange(productImages);

            await _Context.SaveChangesAsync();

        }

        public async Task<ProductColor> AddProductColor(ProductColor productColor)
        {
            _Context.Add(productColor);
            await _Context.SaveChangesAsync();
            return productColor;
        }

		public async Task DeleteProductColor(int productId)
		{
			var productColors = _Context.ProductColors.Where(pc => pc.ProductId == productId).ToList();

			_Context.ProductColors.RemoveRange(productColors);

			await _Context.SaveChangesAsync();

		}
		public async Task<ProductSize> AddProductSize(ProductSize productSize)
        {
            _Context.Add(productSize);
            await _Context.SaveChangesAsync();
            return productSize;
        }

		public async Task DeleteProductSize(int productId)
		{
			var productSizes = _Context.ProductSizes.Where(pc => pc.ProductId == productId).ToList();

			_Context.ProductSizes.RemoveRange(productSizes);

			await _Context.SaveChangesAsync();

		}
		public async Task<List<Product>> GetListProduct()
        {
            return await _Context.Products.Select(e => new Product
            {
                ProductId = e.ProductId,
                ProductName = e.ProductName,
                ProductPrice = e.ProductPrice,
                ProductImage = e.ProductImage,
                CreatedDate = e.CreatedDate,
                ProductSale = e.ProductSale,
                ProductQuantity = e.ProductQuantity,
                Category = e.Category,
                CategoryID = e.CategoryID,
                CollectionID = e.CollectionID,
                Collection = e.Collection
            }).ToListAsync();
        }

        public async Task<List<Product>> GetListProductByFilter(string? priceRange, string? sortOption)
        {

            var query = _Context.Products.AsQueryable();


            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange)
                {
                    case "under-100":
                        query = query.Where(p => p.ProductPrice < 100000);
                        break;
                    case "100-500":
                        query = query.Where(p => p.ProductPrice >= 100000 && p.ProductPrice <= 500000);
                        break;
                    case "500-1000":
                        query = query.Where(p => p.ProductPrice >= 500000 && p.ProductPrice <= 1000000);
                        break;
                    case "above-1000":
                        query = query.Where(p => p.ProductPrice > 1000000);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(sortOption))
            {
                switch (sortOption)
                {
                    case "name-asc":
                        query = query.OrderBy(p => p.ProductName);
                        break;
                    case "name-desc":
                        query = query.OrderByDescending(p => p.ProductName);
                        break;
                    case "price-asc":
                        query = query.OrderBy(p => p.ProductPrice);
                        break;
                    case "price-desc":
                        query = query.OrderByDescending(p => p.ProductPrice);
                        break;
                    case "new-item":
                        query = query.OrderByDescending(p => p.CreatedDate);
                        break;
                }
            }

            return await query.Select(e => new Product
            {
                ProductId = e.ProductId,
                ProductName = e.ProductName,
                ProductPrice = e.ProductPrice,
                ProductImage = e.ProductImage,
                CreatedDate = e.CreatedDate,
                ProductSale = e.ProductSale,
                ProductQuantity = e.ProductQuantity
            }).ToListAsync();
        }


        public async Task<List<Product>> GetListProductByCategory(int categoryid)
        {
            return await _Context.Products.Where(p => p.CategoryID == categoryid)
                .Select(e => new Product
            {
                ProductName = e.ProductName,
                ProductId = e.ProductId,
                ProductPrice = e.ProductPrice,
                ProductImage = e.ProductImage,
                CreatedDate = e.CreatedDate,
                ProductSale = e.ProductSale,
                ProductQuantity = e.ProductQuantity
                }).ToListAsync();
        }
        public async Task<List<Product>> GetListProductByCollection(int collectionid)
        {
            return await _Context.Products.Where(p => p.CollectionID == collectionid)
                .Select(e => new Product
                {
                    ProductName = e.ProductName,
                    ProductId = e.ProductId,
                    ProductPrice = e.ProductPrice,
                    ProductImage = e.ProductImage,
                    CreatedDate = e.CreatedDate,
                    ProductSale = e.ProductSale,
                    ProductQuantity = e.ProductQuantity
                }).ToListAsync();
        }

        public async Task<List<Product>> GetListProductByCategoryName(string categoryname)
        {
            return await _Context.Products.Where(p => p.Category.CategoryName == categoryname)
                .Select(e => new Product
                {
                    ProductName = e.ProductName,
                    ProductId = e.ProductId,
                    ProductPrice = e.ProductPrice,
                    ProductImage = e.ProductImage,
                    CreatedDate = e.CreatedDate,
                    ProductSale = e.ProductSale,
                    ProductQuantity = e.ProductQuantity
                }).ToListAsync();
        }
        public async Task<List<Product>> GetListProductByCollectionName(string collectionname)
        {
            return await _Context.Products.Where(p => p.Collection.CollectionName == collectionname)
                .Select(e => new Product
                {
                    ProductName = e.ProductName,
                    ProductId = e.ProductId,
                    ProductPrice = e.ProductPrice,
                    ProductImage = e.ProductImage,
                    CreatedDate = e.CreatedDate,
                    ProductSale = e.ProductSale,
                    ProductQuantity = e.ProductQuantity
                }).ToListAsync();
        }
   
        public async Task<ProductImage> GetUrlHeaderImage(int id)
        {
            return await _Context.ProductImages
                .Where(p => p.ProductId == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Product> GetProductByName(string name)
        {
            return await _Context.Products.Where(p => p.ProductName == name).Select(p => new Product
            {
                ProductName = p.ProductName,
                ProductId = p.ProductId,
                ProductPrice = p.ProductPrice,
                CreatedDate = p.CreatedDate,
                CategoryID = p.CategoryID,
                ProductSale = p.ProductSale,
                CollectionID = p.CollectionID,
                ProductSizeImage = p.ProductSizeImage,
                ProductImage = p.ProductImage,
                ProductQuantity = p.ProductQuantity,
                ProductColors = p.ProductColors.Select(pc => new ProductColor
                {
                    Color = pc.Color,
                    ColorId = pc.ColorId
                }).ToList(),
                ProductSizes = p.ProductSizes.Select(ps => new ProductSize
                {
                    Size = ps.Size,
                    SizeId = ps.SizeId
                }).ToList(),
                ProductDescription = p.ProductDescription
            }).FirstOrDefaultAsync();
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _Context.Products.Where(p => p.ProductId == id).Select(p => new Product
            {
                ProductName = p.ProductName,
                ProductId = p.ProductId,
                ProductPrice = p.ProductPrice,
                CreatedDate = p.CreatedDate,
                CategoryID = p.CategoryID,
                ProductSale = p.ProductSale,
                CollectionID = p.CollectionID,
                ProductImage = p.ProductImage,
                ProductQuantity = p.ProductQuantity,
                Category = p.Category,
                Collection = p.Collection,
                ProductSizeImage = p.ProductSizeImage,
                ProductColors = p.ProductColors.Select(pc => new ProductColor
                {
                    Color = pc.Color,
                    ColorId = pc.ColorId
                }).ToList(),
                ProductSizes = p.ProductSizes.Select(ps => new ProductSize
                {
                    Size = ps.Size,
                    SizeId = ps.SizeId
                }).ToList(),
                ProductDescription = p.ProductDescription
            }).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetListProductByQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
                return new List<Product>();


            var characters = query.Where(c => !char.IsWhiteSpace(c)).Select(c => c.ToString()).ToList();


            var queryable = _Context.Products.AsQueryable();


            foreach (var character in characters)
            {
                queryable = queryable.Where(p => p.ProductName.Contains(character));
            }

            return await queryable.Select(p => new Product()
            {
                ProductName = p.ProductName,
                ProductImage = p.ProductImage,
                ProductPrice = p.ProductPrice,
                ProductQuantity = p.ProductQuantity,
                ProductSale = p.ProductSale
            }).ToListAsync();
        }


        public async Task<List<ProductImage>> GetListProductImages(int id)
        {
            return await _Context.ProductImages.Where(pi => pi.ProductId == id).ToListAsync();
        }

        public async Task DeleteProductImageById(int id)
        {
            var productImages = _Context.ProductImages.Where(pc => pc.ImageId == id).ToList();

            _Context.ProductImages.RemoveRange(productImages);

            await _Context.SaveChangesAsync();
        }

    }
}
