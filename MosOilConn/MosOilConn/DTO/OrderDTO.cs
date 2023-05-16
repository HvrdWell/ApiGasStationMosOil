using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MosOilConn
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int IdColumns { get; set; }
        public DateTime Data { get; set; }
        public int IdUser { get; set; }
        public string Status { get; set; }
        public float TotalPrice { get; set; }
    }
}

