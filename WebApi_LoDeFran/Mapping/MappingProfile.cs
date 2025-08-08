using AutoMapper;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.Utlis.Dto;
using WebApi_LoDeFran.ViewModels;
namespace WebApi_LoDeFran.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Rols.Select(r => r.Nombre)))
                .ForMember(dest => dest.Permisos, opt => opt.MapFrom(src => src.Permisos.Select(p => p.Nombre)));
            CreateMap<UsuarioViewModel, Usuario>();

            CreateMap<Producto, ProductoViewModel>()
                .ForMember(dest => dest.CategoriaProductoNombre, opt => opt.MapFrom(src => src.CategoriaProducto != null ? src.CategoriaProducto.Nombre : null))
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.Estado != null ? src.Estado.Nombre : null));
            CreateMap<ProductoViewModel, Producto>();

            CreateMap<PedidoViewModel, Pedido>()
    .ForMember(dest => dest.Mesa, opt => opt.Ignore())
    .ForMember(dest => dest.Promocion, opt => opt.Ignore())
    .ForMember(dest => dest.Cliente, opt => opt.Ignore())
    .ForMember(dest => dest.TipoPedido, opt => opt.Ignore())
    .ForMember(dest => dest.PedidoCombos, opt => opt.Ignore())
    .ForMember(dest => dest.Estado, opt => opt.Ignore())
    .ForMember(dest => dest.PromocionId, opt =>
        opt.MapFrom(src => src.PromocionId == 0 ? null : src.PromocionId));

            CreateMap<Pedido, PedidoViewModel>()
    .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
    .ForMember(dest => dest.DetallePedido, opt => opt.MapFrom(src => src.DetallesPedidos))
    .ForMember(dest => dest.Mesa, opt => opt.MapFrom(src => src.Mesa))
    .ForMember(dest => dest.TipoPedidoNombre, opt => opt.MapFrom(src => src.TipoPedido.Nombre))
    .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre + " " + src.Cliente.Apellido : null))
    .ForMember(dest => dest.NombrePromocion, opt => opt.MapFrom(src => src.Promocion != null ? src.Promocion.Nombre : null));



            CreateMap<DetallesPedido, DetallePedidoViewModel>()
                .ForMember(dest => dest.Producto, opt => opt.MapFrom(src => src.Producto))
                .ForMember(dest => dest.EstadoCocinaNombre, opt => opt.MapFrom(src => src.EstadoCocina.Nombre)); 
            CreateMap<DetallePedidoViewModel, DetallesPedido>();

            CreateMap<Bitacora, BitacoraViewModel>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));
            CreateMap<BitacoraViewModel, Bitacora>();

            CreateMap<CategoriasProducto, CategoriaProductoViewModel>().ReverseMap();

            CreateMap<EstadosPedido, EstadoPedidoViewModel>().ReverseMap();

            CreateMap<EstadosProducto, EstadoProductoViewModel>().ReverseMap();

            CreateMap<Caja, CajaViewModel>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.Movimientos, opt => opt.MapFrom(src => src.MovimientoCajas));
            CreateMap<CajaViewModel, Caja>()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.MovimientoCajas, opt => opt.Ignore());

            CreateMap<MetodoPago, MetodoPagoViewModel>().ReverseMap();

            // MotivoMovimiento
            CreateMap<MotivosMovimiento, MotivoMovimientoViewModel>()
                .ForMember(dest => dest.TipoMovimientoNombre, opt => opt.MapFrom(src => src.TipoMovimiento.Nombre));
            CreateMap<MotivoMovimientoViewModel, MotivosMovimiento>()
                .ForMember(dest => dest.TipoMovimiento, opt => opt.Ignore());

            // MovimientoCaja
            CreateMap<MotivosMovimiento, MotivoMovimientoViewModel>()
                .ForMember(dest => dest.TipoMovimientoNombre, opt => opt.MapFrom(src => src.TipoMovimiento.Nombre))
                .ReverseMap()
                .ForMember(dest => dest.TipoMovimiento, opt => opt.Ignore())
                .ForMember(dest => dest.MovimientoCajas, opt => opt.Ignore());

            CreateMap<MovimientoCaja, MovimientoCajaViewModel>()
            .ForMember(dest => dest.MotivoNombre, opt => opt.MapFrom(src => src.MotivoMovimiento != null ? src.MotivoMovimiento.Nombre : null))
            .ForMember(dest => dest.TipoMovimientoNombre, opt => opt.MapFrom(src => src.MotivoMovimiento != null ? src.MotivoMovimiento.TipoMovimiento.Nombre : null))
            .ForMember(dest => dest.MetodoPagoNombre, opt => opt.MapFrom(src => src.MetodoPago != null ? src.MetodoPago.Nombre : null));

            // ViewModel -> Modelo
            CreateMap<MovimientoCajaViewModel, MovimientoCaja>()
                .ForMember(dest => dest.MetodoPago, opt => opt.Ignore())
                .ForMember(dest => dest.MotivoMovimiento, opt => opt.Ignore())
                .ForMember(dest => dest.Caja, opt => opt.Ignore())
                .ForMember(dest => dest.MetodoPagoId, opt => opt.MapFrom(src => src.MetodoPagoId))
                .ForMember(dest => dest.MotivoMovimientoId, opt => opt.MapFrom(src => src.MotivoMovimientoId));


            CreateMap<Factura, FacturaViewModel>()
     .ForMember(dest => dest.ClienteNombre,
         opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre + " " + src.Cliente.Apellido : null))
     .ForMember(dest => dest.MetodoPagoNombre,
         opt => opt.MapFrom(src => src.MetodoPago != null ? src.MetodoPago.Nombre : null))
     .ForMember(dest => dest.UsuarioNombre,
         opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nombre : null))
     .ForMember(dest => dest.CajaDescripcion,
                 opt => opt.MapFrom(src => src.Caja != null ? $"Caja abierta: {src.Caja.FechaApertura.ToString("dd/MM/yyyy HH:mm")} - Estado: {src.Caja.Estado}" : null))
     .ForMember(dest => dest.EstadoNombre,
         opt => opt.MapFrom(src => src.Estado.Nombre));

            CreateMap<FacturaDto, Factura>();


            CreateMap<FacturaViewModel, Factura>()
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.MetodoPago, opt => opt.Ignore())
                .ForMember(dest => dest.Pedido, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Caja, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore());



            CreateMap<Mesa, MesaViewModel>()
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.IdEstadoNavigation.Nombre))
                .ForMember(dest => dest.EstadoId, opt => opt.MapFrom(src => src.IdEstado))
                .ForMember(dest => dest.IdPiso, opt => opt.MapFrom(src => src.IdPiso))
                .ForMember(dest => dest.PisoNombre, opt => opt.MapFrom(src => src.IdPisoNavigation.Nombre))
                .ReverseMap()
                .ForMember(dest => dest.IdEstadoNavigation, opt => opt.Ignore()) // Ignorar navegación inversa al mapear de VM a entidad
                .ForMember(dest => dest.IdPisoNavigation, opt => opt.Ignore())   // Idem
                .ForMember(dest => dest.Reservas, opt => opt.Ignore());          // También ignorás colecciones complejas


            CreateMap<Permiso, PermisoViewModel>().ReverseMap();


            CreateMap<Reserva, ReservaViewModel>()
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente.Nombre))
                .ForMember(dest => dest.MesaNumero, opt => opt.MapFrom(src => src.Mesa.Numero.ToString()))
                .ReverseMap();

            CreateMap<Role, RolViewModel>()
                .ForMember(dest => dest.Permisos, opt => opt.MapFrom(src => src.Permisos.Select(p => p.Nombre).ToList()))
                .ReverseMap();

            CreateMap<Stock, StockViewModel>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre))  // Mapear nombre del producto
                .ReverseMap();
            CreateMap<Insumo, InsumoViewModel>()
                .ForMember(dest => dest.ProveedorNombre, opt => opt.MapFrom(src => src.Proveedor != null ? src.Proveedor.Nombre : null))
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.Estado != null ? src.Estado.Nombre : null))
                .ForMember(dest => dest.Abreviatura, opt => opt.MapFrom(src => src.UnidadMedida != null ? src.UnidadMedida.Abreviatura : null))
                .ReverseMap()
                .ForMember(dest => dest.Proveedor, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ForMember(dest => dest.UnidadMedida, opt => opt.Ignore());

            CreateMap<Proveedore, ProveedorViewModel>().ReverseMap();

            CreateMap<EstadosInsumo, EstadoInsumoViewModel>().ReverseMap();

            CreateMap<Producto, InsumoProductoViewModel>()
                .ForMember(dest => dest.InsumoId, opt => opt.MapFrom(src => src.InsumosProductos));

            CreateMap<InsumosProducto, InsumoProductoViewModel>()
                .ForMember(dest => dest.NombreInsumo, opt => opt.MapFrom(src => src.Insumo.Nombre));

            CreateMap<UnidadMedidum, UnidadMedidaViewModel>().ReverseMap();

            CreateMap<EstadosMesa, EstadoMesaViewModel>().ReverseMap();

            CreateMap<EstadosCocina , EstadoCocinaViewModel>().ReverseMap();

            CreateMap<Piso, PisoViewModel>().ReverseMap();

            CreateMap<Cliente, ClienteViewModel>()
            .ForMember(dest => dest.CalleNomMapa, opt => opt.MapFrom(src => src.CalleNavigation != null ? src.CalleNavigation.NomMapa : null));

            CreateMap<ClienteViewModel, Cliente>()
                .ForMember(dest => dest.CalleNavigation, opt => opt.Ignore()); // evitamos mapear navegación inversa automáticamente

            CreateMap<TiposPedido, TipoPedidoViewModel>().ReverseMap();

            CreateMap<Calle, CalleViewModel>().ReverseMap();

            CreateMap<Combo, ComboViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CombosItems));

            CreateMap<CombosItem, ComboItemViewModel>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre));

            CreateMap<ComboViewModel, Combo>()
                .ForMember(dest => dest.CombosItems, opt => opt.MapFrom(src => src.Items));

            CreateMap<ComboItemViewModel, CombosItem>();

            CreateMap<PedidoCombo, PedidoComboViewModel>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Combo.Nombre))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Combo.Precio)) // si el precio está en Combo
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.PedidoComboItems));

            CreateMap<PedidoComboItem, PedidoComboItemViewModel>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre));

            CreateMap<PedidoComboViewModel, PedidoCombo>()
                .ForMember(dest => dest.Combo, opt => opt.Ignore())
                .ForMember(dest => dest.PedidoComboItems, opt => opt.MapFrom(src => src.Items));

            CreateMap<PedidoComboItemViewModel, PedidoComboItem>()
                .ForMember(dest => dest.Producto, opt => opt.Ignore());

            CreateMap<Promocione, PromocionViewModel>()
                .ForMember(dest => dest.NombreEstado,
                           opt => opt.MapFrom(src => src.Estado != null ? src.Estado.Nombre : null))
                .ForMember(dest => dest.NombreAplicacion,
                           opt => opt.MapFrom(src => src.Aplicacion != null ? src.Aplicacion.Nombre : null))
                .ForMember(dest => dest.NombreTipoDescuento,
                           opt => opt.MapFrom(src => src.TipoDescuento != null ? src.TipoDescuento.Nombre : null))
                .ForMember(dest => dest.NombreTipoPromocion,
                           opt => opt.MapFrom(src => src.TipoPromocion != null ? src.TipoPromocion.Nombre : null))
                .ForMember(dest => dest.PromocionDia, opt => opt.MapFrom(src => src.PromocionDia));




            CreateMap<PromocionViewModel, Promocione>();
            CreateMap<TipoDescuento, TipoDescuentoViewModel>();
            CreateMap<TipoPromocion, TipoPromocionViewModel>();
            CreateMap<Dia, DiaViewModel>();
            CreateMap<PromocionesAplicacione, PromocionAplicacionViewModel>();
            CreateMap<PromocionDia, PromocionDiaViewModel>();

        }
    }
}
