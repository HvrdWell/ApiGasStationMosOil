using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class UserRole
{
    public string Role { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
