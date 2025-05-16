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
           .ForMember(dest => dest.DetallePedido, opt => opt.MapFrom(src => src.DetallesPedidos));
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

            CreateMap<Factura, FacturaViewModel>().ReverseMap();

            CreateMap<Mesa, MesaViewModel>().ReverseMap();

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
        }
    }
}
