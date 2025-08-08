using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class DescuentosProducto
{
    public int Id { get; set; }

    public int DescuentoId { get; set; }

    public int ProductoId { get; set; }

    public virtual Descuento Descuento { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
