using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFront.Domain.Entities
{
    public class UserToUpdate
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? TelegramUsername { get; set; }
        public Avatar? Avatar { get; set; }
    }
}
