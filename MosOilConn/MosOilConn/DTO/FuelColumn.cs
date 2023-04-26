using System;
using MosOilConn.Entities;

namespace MosOilConn
{
    public class FuelColumn
    {
        public string ColumnNumber { get; set; }
        public List<TypeFuel> TypeFuels { get; set; }
    }
}

