using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Stock
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public int CantidadDisponible { get; set; }

    public int CantidadEntrada { get; set; }

    public int CantidadSalida { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
