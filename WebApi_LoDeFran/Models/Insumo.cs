using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Insumo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal? Costo { get; set; }

    public decimal CantidadDisponible { get; set; }

    public int? ProveedorId { get; set; }

    public DateTime? FechaUltimoIngreso { get; set; }

    public decimal CantidadMinima { get; set; }

    public int? EstadoId { get; set; }

    public int? UnidadMedidaId { get; set; }

    public virtual EstadosInsumo? Estado { get; set; }

    public virtual ICollection<InsumosProducto> InsumosProductos { get; set; } = new List<InsumosProducto>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual Proveedore? Proveedor { get; set; }

    public virtual UnidadMedidum? UnidadMedida { get; set; }
}
