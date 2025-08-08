using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class TipoPromocion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();
}
