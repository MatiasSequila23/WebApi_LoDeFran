using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int CategoriaId { get; set; }

    public int? EstadoId { get; set; }

    public int? CategoriaProductoId { get; set; }

    public int? InsumoId { get; set; }

    public bool TieneInsumos { get; set; }

    public int? SubcategoriaProductoId { get; set; }

    public bool TieneVariante { get; set; }

    public virtual CategoriasProducto? CategoriaProducto { get; set; }

    public virtual ICollection<CombosItem> CombosItems { get; set; } = new List<CombosItem>();

    public virtual ICollection<DescuentosProducto> DescuentosProductos { get; set; } = new List<DescuentosProducto>();

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();

    public virtual EstadosProducto? Estado { get; set; }

    public virtual ICollection<InsumosProducto> InsumosProductos { get; set; } = new List<InsumosProducto>();

    public virtual ICollection<PedidoComboItem> PedidoComboItems { get; set; } = new List<PedidoComboItem>();

    public virtual ICollection<ProductosVariante> ProductosVariantes { get; set; } = new List<ProductosVariante>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual SubcategoriasProducto? SubcategoriaProducto { get; set; }
}
