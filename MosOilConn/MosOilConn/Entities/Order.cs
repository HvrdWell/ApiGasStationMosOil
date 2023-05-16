using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int IdColumns { get; set; }

    public DateTime Data { get; set; } = DateTime.Now;

    public int IdUser { get; set; }

    public string Status { get; set; } = null!;

    public float TotalPrice { get; set; }

    public virtual ICollection<FoodBucket> FoodBuckets { get; } = new List<FoodBucket>();

    public virtual Column IdColumnsNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual StatusOrder StatusNavigation { get; set; } = null!;
}
