namespace WebApi_LoDeFran.ViewModels
{
    public class DiaViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public int Codigo { get; set; } // 1 = Lunes, 2 = Martes, ..., 7 = Domingo
    }
}
