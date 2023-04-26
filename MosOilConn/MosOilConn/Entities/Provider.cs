using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class Provider
{
    public int IdProvider { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Storage> Storages { get; } = new List<Storage>();
}
