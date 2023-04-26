using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class UserStatus
{
    public string StatusUser { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
