using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class SubcategoriasProducto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int CategoriaId { get; set; }

    public virtual CategoriasProducto Categoria { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
