using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class CombosItem
{
    public int Id { get; set; }

    public int ComboId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public string? CategoriaGrupo { get; set; }

    public bool EsOpcional { get; set; }

    public virtual Combo Combo { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
