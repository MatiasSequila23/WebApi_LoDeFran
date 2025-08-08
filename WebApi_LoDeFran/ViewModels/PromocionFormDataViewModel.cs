namespace WebApi_LoDeFran.ViewModels
{
    public class PromocionFormDataViewModel
    {
        public List<TipoDescuentoViewModel> TiposDescuento { get; set; } = new();
        public List<PromocionAplicacionViewModel> Aplicaciones { get; set; } = new();
        public List<DiaViewModel> Dias { get; set; } = new();
        //public List<EstadoPromocionViewModel> Estados { get; set; } = new();
        public List<TipoPromocionViewModel> TiposPromocion { get; set; } = new();
    }
}
