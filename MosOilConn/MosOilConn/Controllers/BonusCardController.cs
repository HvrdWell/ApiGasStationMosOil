using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosOilConn.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MosOilConn
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusCardController : ControllerBase
    {
        private readonly TestdbContext TestdbContext;

        public BonusCardController(TestdbContext testdbContext)
        {
            this.TestdbContext = TestdbContext;

        }

    }
}

