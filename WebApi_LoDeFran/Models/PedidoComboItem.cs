using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class PedidoComboItem
{
    public int Id { get; set; }

    public int PedidoComboId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public virtual PedidoCombo PedidoCombo { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
