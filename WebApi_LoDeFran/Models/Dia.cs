using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Dia
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Codigo { get; set; }

    public virtual ICollection<PromocionDia> PromocionDia { get; set; } = new List<PromocionDia>();
}
