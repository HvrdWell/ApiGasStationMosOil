using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class Storage
{
    public int IdStorage { get; set; }

    public int IdTypeFuel { get; set; }

    public int IdProvider { get; set; }

    public string Remainder { get; set; } = null!;

    public virtual ICollection<Column> Columns { get; } = new List<Column>();

    public virtual Provider IdProviderNavigation { get; set; } = null!;

    public virtual TypeFuel IdTypeFuelNavigation { get; set; } = null!;
}
