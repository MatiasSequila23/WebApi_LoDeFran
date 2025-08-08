using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class PedidoCombo
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int ComboId { get; set; }

    public int Cantidad { get; set; }

    public virtual Combo Combo { get; set; } = null!;

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual ICollection<PedidoComboItem> PedidoComboItems { get; set; } = new List<PedidoComboItem>();
}
