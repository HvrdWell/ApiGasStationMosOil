using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class FoodBucket
{
    public int IdOrder { get; set; }

    public int IdProduct { get; set; }

    public int IdFoodBucket { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
