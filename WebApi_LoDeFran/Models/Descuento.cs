using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Descuento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public decimal Valor { get; set; }

    public bool EsAutomatico { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? EstadoId { get; set; }

    public virtual ICollection<DescuentosProducto> DescuentosProductos { get; set; } = new List<DescuentosProducto>();
}
