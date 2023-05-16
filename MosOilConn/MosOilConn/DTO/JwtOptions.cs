using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MosOilConn.DTO
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
    }
}

