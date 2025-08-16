namespace WebApi_LoDeFran.Utlis.ClassAux
{
    public class ActualizarComboRequest
    {
        public int PedidoId { get; set; }   // ID del pedido al que pertenece
        public int ComboId { get; set; }    // ID del combo a actualizar
        public int Cantidad { get; set; }
        public string Comentario { get; set; }
    }
}
