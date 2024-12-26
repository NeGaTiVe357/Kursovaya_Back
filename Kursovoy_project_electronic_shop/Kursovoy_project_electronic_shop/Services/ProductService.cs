using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Kursovoy_project_electronic_shop.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.RegularExpressions;

namespace Kursovoy_project_electronic_shop.Services
{
    public class ProductService : IProductService
    {
        private readonly ElectronicShopDbContext _electronicShopDbContext;

        public ProductService(ElectronicShopDbContext electronicShopDbContext)
        {
            _electronicShopDbContext = electronicShopDbContext;
        }

        public bool CreateProduct(Contracts.ProductInfo productInfo)
        {
            var product = new Product
            {
                ProductUid = Guid.NewGuid(),
                Name = productInfo.Name,
                Price = productInfo.Price,
                Image = productInfo.Image,
            };

            AddManufacturers(productInfo.Manufacturers, product);
            AddTypes(productInfo.Types, product);

            _electronicShopDbContext.Add(product);

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        private void AddManufacturers(List<string> manufacturerNames, Product product)
        {
            foreach (var manufacturerName in manufacturerNames)
            {
                var manufacturer = _electronicShopDbContext.Set<Manufacturer>().SingleOrDefault(x => x.Name == manufacturerName);

                if (manufacturer == null)
                {
                    manufacturer = new Manufacturer
                    {
                        ManufacturerUid = Guid.NewGuid(),
                        Name = manufacturerName
                    };

                    _electronicShopDbContext.Add(manufacturer);
                }

                product.Manufacturers.Add(manufacturer);
            }
        }

        private void AddTypes(List<string> typeNames, Product product)
        {
            foreach (var typeName in typeNames)
            {
                var type = _electronicShopDbContext.Set<DatabaseAccessLayer.Entities.Type>().SingleOrDefault(x => x.Name == typeName);

                if (type == null)
                {
                    type = new DatabaseAccessLayer.Entities.Type
                    {
                        TypeUid = Guid.NewGuid(),
                        Name = typeName
                    };

                    _electronicShopDbContext.Add(type);
                }

                product.Types.Add(type);
            }
        }

        public List<Contracts.Product>? GetAllProducts()
        {
            var products = _electronicShopDbContext.Set<Product>()
                .Include(x => x.Manufacturers)
                .Include(x => x.Types)
                .ToList();

            if (products.Count == 0) { return null; }

            return products.Select(product => new Contracts.Product
            {
                ProductUid = product.ProductUid,
                Name = product.Name,
                Price = product.Price,
                Manufacturers = product.Manufacturers.Select(x => x.Name).ToList(),
                Types = product.Types.Select(x => x.Name).ToList(),
            }).ToList();
        }

        public Contracts.Product? GetSingleProduct(string productName)
        {
            var product = _electronicShopDbContext.Set<Product>()
                .Include(x => x.Manufacturers)
                .Include(x => x.Types)
                .SingleOrDefault(x => x.Name.ToLower() == productName.ToLower());

            if (product == null) { return null; }

            return new Contracts.Product
            {
                ProductUid = product.ProductUid,
                Name = product.Name,
                Price = product.Price,
                Manufacturers = product.Manufacturers.Select(x => x.Name).ToList(),
                Types = product.Types.Select(x => x.Name).ToList(),
            };
        }

        public List<Contracts.ProductInfo>? GetProductsInfo()
        {
            var products = _electronicShopDbContext.Set<Product>()
                .Include(x => x.Manufacturers)
                .Include(x => x.Types)
                .ToList();

            if (products.Count == 0) { return null; }

            return products.Select(product => new Contracts.ProductInfo
            {
                ProductUid = product.ProductUid,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Manufacturers = product.Manufacturers.Select(x => x.Name).ToList(),
                Types = product.Types.Select(x => x.Name).ToList(),
            }).ToList();
        }

        public bool UpdateProduct(Guid productUid, Contracts.ProductInfo productInfo)
        {
            var product = _electronicShopDbContext.Set<Product>()
                .Include(x => x.Manufacturers)
                .Include(x => x.Types)
                .SingleOrDefault(x => x.ProductUid == productUid);

            if (product == null) { return false; }

            product.Name = productInfo.Name;
            product.Price = productInfo.Price;

            product.Manufacturers.Clear();
            product.Types.Clear();

            AddManufacturers(productInfo.Manufacturers, product);
            AddTypes(productInfo.Types, product);

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public bool DeleteProduct(Guid productUid)
        {
            var product = _electronicShopDbContext.Set<Product>().SingleOrDefault(x => x.ProductUid == productUid);

            if (product == null) { return false; }

            _electronicShopDbContext.Remove(product);

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public bool IsProductExists(Guid productUid)
        {
            var product = _electronicShopDbContext.Set<Product>().SingleOrDefault(x => x.ProductUid == productUid);

            if (product == null) { return false; }

            return true;
        }

        public bool CheckProductName(string productName)
        {
            var product = _electronicShopDbContext.Set<Product>().SingleOrDefault(x => x.Name == productName);

            if (product == null) { return false; }

            return true;
        }

        public bool CheckProductInfo(Contracts.ProductInfo productInfo)
        {
            var product = _electronicShopDbContext.Set<Product>()
                .SingleOrDefault(x => x.Name == productInfo.Name && x.Price == productInfo.Price );

            if (product == null) { return false; };

            return true;
        }

        public bool CheckProductInfo(Guid productUid, Contracts.ProductInfo productInfo)
        {
            var product = _electronicShopDbContext.Set<Product>()
                .SingleOrDefault(x => x.ProductUid != productUid && x.Name == productInfo.Name && x.Price == productInfo.Price );

            if (product == null) { return false; };

            return true;
        }

        public bool CheckRegex(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я0-9. -]{1,}$");

            if (!regex.IsMatch(name))
            {
                return false;
            }

            return true;
        }

        public bool CheckRegexList(List<string> list)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я -]{1,}$");

            foreach (var item in list)
            {
                if (!regex.IsMatch(item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
