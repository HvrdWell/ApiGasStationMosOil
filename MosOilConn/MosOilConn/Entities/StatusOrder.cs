using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class StatusOrder
{
    public string Status { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
