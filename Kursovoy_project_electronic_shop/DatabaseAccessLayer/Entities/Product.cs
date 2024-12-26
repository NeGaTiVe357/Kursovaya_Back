using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Entities
{
    public class Product
    {
        public int ProductId { get; init; }

        public required Guid ProductUid { get; init; }

        public required string Name { get; set; }

        public required int Price { get; set; }

        public string? Image { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Type> Types { get; set; } = new List<Type>();

        public ICollection<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>();
    }
}