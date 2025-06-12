using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Piso
{
    public int IdPiso { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
