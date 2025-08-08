namespace WebApi_LoDeFran.ViewModels
{
    public class PedidoComboViewModel
    {
        public int ComboId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        // Items opcionalmente, si querés mostrar o usar sus detalles
        public List<PedidoComboItemViewModel> Items { get; set; } = new();
    }
}
