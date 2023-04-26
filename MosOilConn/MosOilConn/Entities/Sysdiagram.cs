using System;
using System.Collections.Generic;

namespace MosOilConn.Entities;

public partial class Sysdiagram
{
    public string? Name { get; set; }

    public string? PrincipalId { get; set; }

    public string? DiagramId { get; set; }

    public string? Version { get; set; }

    public string? Definition { get; set; }
}
