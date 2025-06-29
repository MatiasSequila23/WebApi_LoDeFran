using AutoMapper;
using WebApi_LoDeFran.Models;
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

            CreateMap<Pedido, PedidoViewModel>()
               .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
               .ForMember(dest => dest.DetallePedido, opt => opt.MapFrom(src => src.DetallesPedidos))
               .ForMember(dest => dest.Mesa, opt => opt.MapFrom(src => src.Mesa))
               .ForMember(dest => dest.TipoPedidoNombre, opt => opt.MapFrom(src => src.TipoPedido.Nombre))
               .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre + " " + src.Cliente.Apellido : null));


            CreateMap<PedidoViewModel, Pedido>();

            CreateMap<DetallesPedido, DetallePedidoViewModel>()
                .ForMember(dest => dest.Producto, opt => opt.MapFrom(src => src.Producto));
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
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre + " " + src.Cliente.Apellido : null))
                .ForMember(dest => dest.MetodoPagoNombre, opt => opt.MapFrom(src => src.MetodoPago != null ? src.MetodoPago.Nombre : null));
            CreateMap<FacturaViewModel, Factura>()
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.MetodoPago, opt => opt.Ignore())
                .ForMember(dest => dest.Pedido, opt => opt.Ignore());


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

            CreateMap<Promocione, PromocionViewModel>().ReverseMap();

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

            CreateMap<EstadosMesa, EstadoMesaViewModel>().ReverseMap();

            CreateMap<Piso, PisoViewModel>().ReverseMap();

            CreateMap<Cliente, ClienteViewModel>().ReverseMap();

            CreateMap<TiposPedido, TipoPedidoViewModel>().ReverseMap();

        }
    }
}
