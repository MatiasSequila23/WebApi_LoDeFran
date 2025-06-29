using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Calle
{
    public int Id { get; set; }

    public int? Codigo { get; set; }

    public string? NomOficial { get; set; }

    public int? AltIzqIni { get; set; }

    public int? AltIzqFin { get; set; }

    public int? AltDerIni { get; set; }

    public int? AltDerFin { get; set; }

    public string? NomAnter { get; set; }

    public string? NomMapa { get; set; }

    public string? TipoC { get; set; }

    public string? Bicisenda { get; set; }

    public string? RedJerarq { get; set; }

    public string? TipoFfcc { get; set; }

    public int? Comuna { get; set; }

    public int? ComPar { get; set; }

    public int? ComImpar { get; set; }

    public string? Barrio { get; set; }

    public string? BarrioPar { get; set; }

    public string? BarrioImp { get; set; }

    public string? Geometry { get; set; }
}
