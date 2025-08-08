using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class EstadosFactura
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
