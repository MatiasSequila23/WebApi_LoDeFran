using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class EstadosMesa
{
    public int IdEstado { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
