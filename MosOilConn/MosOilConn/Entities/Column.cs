using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class Column
{
    public int IdColumn { get; set; }

    public string Number { get; set; } = null!;

    public int IdStorage { get; set; }

    public virtual Storage IdStorageNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
