using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace DatabaseAccessLayer.Entities

{
    public class User
    {
        public int UserId { get; init; }

        public required Guid UserUid { get; init; }

        public required string Name { get; set; }

        public required string Login { get; set; }

        public required string Password { get; set; }

        public string? Email { get; set; }

        public required bool IsAdmin { get; set; } = false;

        public ICollection<Order> Orders { get; set; }
    }
}