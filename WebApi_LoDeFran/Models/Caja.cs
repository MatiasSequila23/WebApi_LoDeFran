using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Caja
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaApertura { get; set; }

    public DateTime? FechaCierre { get; set; }

    public decimal MontoInicial { get; set; }

    public decimal? MontoFinal { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; set; } = new List<MovimientoCaja>();

    public virtual Usuario Usuario { get; set; } = null!;
}
