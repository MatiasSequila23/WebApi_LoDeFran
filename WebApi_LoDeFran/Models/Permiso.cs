using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Permiso
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Role> Rols { get; set; } = new List<Role>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
