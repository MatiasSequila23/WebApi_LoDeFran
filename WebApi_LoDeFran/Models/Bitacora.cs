using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Bitacora
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? Fecha { get; set; }

    public string Accion { get; set; } = null!;

    public string? Detalle { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
