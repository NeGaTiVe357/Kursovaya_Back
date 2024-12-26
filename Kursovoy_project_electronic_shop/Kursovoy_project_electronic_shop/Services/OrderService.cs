using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Kursovoy_project_electronic_shop.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace Kursovoy_project_electronic_shop.Services
{
    public class OrderService : IOrderService
    {
        private readonly ElectronicShopDbContext _electronicShopDbContext;

        public OrderService(ElectronicShopDbContext electronicShopDbContext)
        {
            _electronicShopDbContext = electronicShopDbContext;
        }

        public bool CreateOrder(Guid userUid, Guid productUid)
        {
            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);
            var product = _electronicShopDbContext.Set<Product>().SingleOrDefault(x => x.ProductUid == productUid);

            if (user == null || product == null ) { return false; }

            var order = new Order
            {
                OrderUid = Guid.NewGuid(),
                User = user,
                Product = product,
                IsPurchased = false,
            };

            _electronicShopDbContext.Add(order);

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Order>? GetAllOrders()
        {
            var orders = _electronicShopDbContext.Set<Order>()
                .Include(x => x.Product)
                .Include(x => x.Product.Manufacturers)
                .Include(x => x.Product.Types)
                .Include(x => x.User)
                .Where(x=> x.IsPurchased == true)
                .ToList();

            if (orders.Count == 0) { return null; }

            return orders.Select(order => new Contracts.Order
            {
                OrderUid = order.OrderUid,
                UserLogin = order.User.Login,
                ProductName = order.Product.Name,
                ProductPrice = order.Product.Price,
                ProductManufacturer = order.Product.Manufacturers.Select(x => x.Name).ToList(),
                ProductType = order.Product.Types.Select(x => x.Name).ToList()

            }).ToList();
        }

        public List<Contracts.UserOrder>? GetUserOrders(Guid userUid)
        {
            var orders = _electronicShopDbContext.Set<Order>()
                .Include(x => x.Product)
                .Include(x => x.Product.Manufacturers)
                .Include(x => x.Product.Types)
                .Include(x => x.User)
                .Where(x => x.User.UserUid == userUid && x.IsPurchased == false)
                .ToList();

            if (orders.Count == 0) { return null; }

            return orders.Select(order => new Contracts.UserOrder
            {
                OrderUid = order.OrderUid,
                ProductImage = order.Product.Image,
                ProductName = order.Product.Name,
                ProductPrice = order.Product.Price,
                ProductManufacturer = order.Product.Manufacturers.Select(x => x.Name).ToList(),
                ProductType = order.Product.Types.Select(x => x.Name).ToList()
            }).ToList();
        }

        public List<Contracts.UserOrder>? GetPurchasedUserOrders(Guid userUid)
        {
            var orders = _electronicShopDbContext.Set<Order>()
                .Include(x => x.Product)
                .Include(x => x.Product.Manufacturers)
                .Include(x => x.Product.Types)
                .Include(x => x.User)
                .Where(x => x.User.UserUid == userUid && x.IsPurchased == true)
                .ToList();

            if (orders.Count == 0) { return null; }

            return orders.Select(order => new Contracts.UserOrder
            {
                OrderUid = order.OrderUid,
                ProductImage = order.Product.Image,
                ProductName = order.Product.Name,
                ProductPrice = order.Product.Price,
                ProductManufacturer = order.Product.Manufacturers.Select(x => x.Name).ToList(),
                ProductType = order.Product.Types.Select(x => x.Name).ToList()
            }).ToList();
        }

        public List<Contracts.Order>? GetProductOrders(Guid productUid)
        {
            var orders = _electronicShopDbContext.Set<Order>()
                .Include(x => x.Product)
                .Include(x => x.Product.Manufacturers)
                .Include(x => x.Product.Types)
                .Include(x => x.User)
                .Where(x => x.Product.ProductUid == productUid)
                .ToList();

            if (orders.Count == 0) { return null; }

            return orders.Select(order => new Contracts.Order
            {
                OrderUid = order.OrderUid,
                UserLogin = order.User.Login,
                ProductName = order.Product.Name,
                ProductPrice = order.Product.Price,
                ProductManufacturer = order.Product.Manufacturers.Select(x => x.Name).ToList(),
                ProductType = order.Product.Types.Select(x => x.Name).ToList()
            }).ToList();
        }
        
        public bool UpdateOrderStatus(Guid orderUid)
        {
            var order = _electronicShopDbContext.Set<Order>().SingleOrDefault(x => x.OrderUid == orderUid);

            if (order == null) { return false; }

            order.IsPurchased = true;

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public bool DeleteOrder(Guid orderUid)
        {
            var order = _electronicShopDbContext.Set<Order>().SingleOrDefault(x => x.OrderUid == orderUid);

            if (order == null) { return false; }

            _electronicShopDbContext.Remove(order);

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public bool IsOrderExists(Guid orderUid)
        {
            var order = _electronicShopDbContext.Set<Order>().SingleOrDefault(x => x.OrderUid == orderUid);

            if (order == null) { return false; }

            return true;
        }

    }
}
