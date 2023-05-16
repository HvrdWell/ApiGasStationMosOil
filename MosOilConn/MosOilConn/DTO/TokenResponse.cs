using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MosOilConn
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}

