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
    [ApiController]
    [Route("api/[controller]")]
    public class FuelController : Controller
    {
        private readonly TestdbContext TestdbContext;
        // GET: /<controller>/
        public FuelController(TestdbContext TestdbContext)
        {
            this.TestdbContext = TestdbContext;
        }


        [HttpGet("GetFuels")]
        public async Task<ActionResult<List<TypeFuelDTO>>> Get()
        {
            var List  = await TestdbContext.TypeFuels.Select(
                    s => new TypeFuelDTO
                    {
                        idTypeFuel = s.IdTypeFuel,
                        nameTypeFuel = s.NameTypeFuel,
                        price = s.Price
                    }
                 ).ToListAsync();
            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        [HttpGet("column/{columnNumber}")]
        public IActionResult GetFuelNamesForColumn(string columnNumber)
        {
            var typeFuels = TestdbContext.TypeFuels
                .Where(tf => TestdbContext.Storages
                    .Any(s => s.IdTypeFuel == tf.IdTypeFuel && TestdbContext.Columns
                        .Any(c => c.IdStorage == s.IdStorage && c.Number == columnNumber)))
                .Select(tf => new { tf.NameTypeFuel, tf.Price })
                .ToList();

            return Ok(typeFuels); 
        }
        [HttpGet("column/all")]
        public IActionResult GetAllColumnsWithFuel()
        {
            var fuelColumns = TestdbContext.Columns
            .Join(TestdbContext.Storages,
                c => c.IdStorage,
                s => s.IdStorage,
                (c, s) => new { c.Number, s.IdTypeFuel })
            .Join(TestdbContext.TypeFuels,
                cs => cs.IdTypeFuel,
                tf => tf.IdTypeFuel,
                (cs, tf) => new { cs.Number, TypeFuel = new { tf.IdTypeFuel, tf.NameTypeFuel, tf.Price } })
            .GroupBy(cs => cs.Number)
             .Select(g => new FuelColumn
             {
                 ColumnNumber = g.Key,
                 TypeFuels = g.Select(cs => new TypeFuel
                 {
                     IdTypeFuel = cs.TypeFuel.IdTypeFuel,
                     NameTypeFuel = cs.TypeFuel.NameTypeFuel,
                     Price = cs.TypeFuel.Price
                 }).ToList()
             })
            .ToList();

            return Ok(fuelColumns);
        }

        [HttpGet("column/check")]
        public IActionResult GetAllFuelsByColumn()
        {
            var columns = TestdbContext.Columns
            .Join(
                TestdbContext.Storages,
                c => c.IdStorage,
                s => s.IdStorage,
                (c, s) => new { Column = c, Storage = s }
            )
            .Join(
                TestdbContext.TypeFuels,
                cs => cs.Storage.IdTypeFuel,
                tf => tf.IdTypeFuel,
                (cs, tf) => new {
                    Column = cs.Column,
                    Storage = cs.Storage,
                    TypeFuel = tf
                }
            )
            .OrderBy(cs => cs.Column.Number)
            .Select(cs => new {
                ColumnNumber = cs.Column.Number,
                FuelName = cs.TypeFuel.NameTypeFuel
            })
            .ToList();

            return Ok(columns);

        }
    }
}

