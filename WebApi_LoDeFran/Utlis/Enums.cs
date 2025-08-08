namespace WebApi_LoDeFran.Utlis
{
    public class Enums
    {
        public enum EstadoMesa
        {
            Disponible = 1,
            Ocupada = 2,
            Reservada = 3,
            Fuera_de_servicio = 4
        }
        public enum EstadoPedido
        {
            Abierto = 1,             // Pedido creado y se están cargando productos.
            EnPreparacion = 2,       // Cocina está preparando.
            ListoParaEntregar = 3,   // Cocina termina, listo para el mozo.
            Entregado = 4,           // El mozo entrega a la mesa.
            A_Cobrar = 5,            // El cliente pide la cuenta.
            Cobrado = 6,             // Cajero cobra el pedido.
            Cerrado = 7,             // Se liberó la mesa.
            Cancelado = 8
        }
        public enum EstadoDetallePedidoCocina
        {
            Pendiente = 1,             
            EnPreparacion = 2,       
            Preparado = 3,   
            Entregado = 4
        }
    }
}
