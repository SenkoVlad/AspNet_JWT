using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet_JWT_Client.Models
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
    }
}
