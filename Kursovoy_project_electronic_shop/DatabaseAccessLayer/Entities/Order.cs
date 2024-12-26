using DatabaseAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Entities
{
    public class Order
    {
        public int OrderId { get; init; }

        public required Guid OrderUid { get; init; }

        public required User User { get; set; }

        public required Product Product { get; set; }

        public required bool IsPurchased { get; set; } = false;
    }
}
