using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class UserGender
{
    public string Gender { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
