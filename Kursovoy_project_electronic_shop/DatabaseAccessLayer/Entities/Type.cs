using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Entities
{
    public class Type
    {
        public int TypeId { get; init; }

        public required Guid TypeUid { get; init; }

        public required string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}