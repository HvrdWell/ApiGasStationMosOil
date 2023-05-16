using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class Bonuscard
{
    public int IdCard { get; set; }

    public int ScoresCard { get; set; }

    public int numberOfQR { get; set; } 

    public virtual ICollection<User> Users { get; } = new List<User>();
}
