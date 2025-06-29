using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class TiposMovimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<MotivosMovimiento> MotivosMovimientos { get; set; } = new List<MotivosMovimiento>();
}
