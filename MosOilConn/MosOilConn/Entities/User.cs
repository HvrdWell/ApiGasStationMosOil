using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class User
{
    public int IdUser { get; set; }

    public int IdCard { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public DateTime Data { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string StatusUser { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? AuthToken { get; set; }

    public virtual UserGender GenderNavigation { get; set; } = null!;

    public virtual Bonuscard IdCardNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual UserRole RoleNavigation { get; set; } = null!;

    public virtual UserStatus StatusUserNavigation { get; set; } = null!;
}
