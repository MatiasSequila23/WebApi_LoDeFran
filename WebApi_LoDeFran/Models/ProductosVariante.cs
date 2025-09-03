using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class ProductosVariante
{
    public int IdVariante { get; set; }

    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int? Stock { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
