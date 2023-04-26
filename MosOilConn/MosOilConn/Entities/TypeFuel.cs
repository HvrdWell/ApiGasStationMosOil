using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class TypeFuel
{
    public int IdTypeFuel { get; set; }

    public string NameTypeFuel { get; set; } = null!;

    public float Price { get; set; }

    public virtual ICollection<Storage> Storages { get; } = new List<Storage>();
}
