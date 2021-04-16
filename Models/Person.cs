using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet_JWT.Models
{
    public class Person
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
