using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class Product
{
    public int IdProduct { get; set; }

    public string Name { get; set; } = null!;

    public string Discription { get; set; } = null!;

    public float Price { get; set; }

    public int Count { get; set; }

    public byte[]? Photo { get; set; }

    public virtual ICollection<FoodBucket> FoodBuckets { get; } = new List<FoodBucket>();
}
