using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class UnidadMedidum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Abreviatura { get; set; } = null!;

    public virtual ICollection<Insumo> Insumos { get; set; } = new List<Insumo>();
}
