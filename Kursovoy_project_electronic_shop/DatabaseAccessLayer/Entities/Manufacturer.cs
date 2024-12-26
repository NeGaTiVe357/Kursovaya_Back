using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Entities
{
    public class Manufacturer
    {
        public int ManufacturerId { get; init; }

        public required Guid ManufacturerUid { get; init; }

        public required string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}