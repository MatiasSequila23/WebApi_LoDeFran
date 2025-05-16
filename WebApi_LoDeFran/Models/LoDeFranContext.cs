using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApi_LoDeFran.Models;

public partial class LoDeFranContext : DbContext
{
    public LoDeFranContext()
    {
    }

    public LoDeFranContext(DbContextOptions<LoDeFranContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bitacora> Bitacoras { get; set; }

    public virtual DbSet<CategoriasProducto> CategoriasProductos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetallesPedido> DetallesPedidos { get; set; }

    public virtual DbSet<EstadosInsumo> EstadosInsumos { get; set; }

    public virtual DbSet<EstadosPedido> EstadosPedidos { get; set; }

    public virtual DbSet<EstadosProducto> EstadosProductos { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Insumo> Insumos { get; set; }

    public virtual DbSet<InsumosProducto> InsumosProductos { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Promocione> Promociones { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<UnidadMedidum> UnidadMedida { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-O064NS2\\SQLEXPRESS;Database=LoDeFran;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bitacora>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bitacora__3213E83F40EC9F06");

            entity.ToTable("bitacoras");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accion");
            entity.Property(e => e.Detalle)
                .HasColumnType("text")
                .HasColumnName("detalle");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Bitacoras)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Bitacora__usuari__6E01572D");
        });

        modelBuilder.Entity<CategoriasProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3213E83FEFA6EF6D");

            entity.ToTable("categorias_productos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clientes__3213E83FC0507C03");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.Email, "UQ__Clientes__AB6E6164AF96F521").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PuntosFidelidad)
                .HasDefaultValue(0)
                .HasColumnName("puntos_fidelidad");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<DetallesPedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Detalles__3213E83F3686E91D");

            entity.ToTable("detalles_pedidos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.PedidoId).HasColumnName("pedido_id");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.Pedido).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.PedidoId)
                .HasConstraintName("FK_DetallesPedido_Pedidos");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detalles_Producto");
        });

        modelBuilder.Entity<EstadosInsumo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__estados___3213E83F35CD2668");

            entity.ToTable("estados_insumos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadosPedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Estados___3213E83FE27A2447");

            entity.ToTable("estados_pedidos");

            entity.HasIndex(e => e.Nombre, "UQ__Estados___72AFBCC64B3BA07E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadosProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EstadoPr__3213E83F0FE16F27");

            entity.ToTable("estados_productos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Facturas__3213E83FD14D3C68");

            entity.ToTable("facturas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaEmision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_emision");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.PedidoId).HasColumnName("pedido_id");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.PedidoId)
                .HasConstraintName("FK_Facturas_Pedidos");
        });

        modelBuilder.Entity<Insumo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__insumos__3213E83F5F8B6EF6");

            entity.ToTable("insumos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadDisponible)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cantidad_disponible");
            entity.Property(e => e.CantidadMinima)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cantidad_minima");
            entity.Property(e => e.Costo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("costo");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EstadoId).HasColumnName("estado_id");
            entity.Property(e => e.FechaUltimoIngreso)
                .HasColumnType("datetime")
                .HasColumnName("fecha_ultimo_ingreso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");
            entity.Property(e => e.UnidadMedidaId).HasColumnName("unidad_medida_id");

            entity.HasOne(d => d.Estado).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.EstadoId)
                .HasConstraintName("fk_estado_insumo");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("FK__insumos__proveed__6442E2C9");

            entity.HasOne(d => d.UnidadMedida).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.UnidadMedidaId)
                .HasConstraintName("FK_insumos_unidad_medida");
        });

        modelBuilder.Entity<InsumosProducto>(entity =>
        {
            entity.HasKey(e => new { e.ProductoId, e.InsumoId });

            entity.ToTable("insumos_productos");

            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.InsumoId).HasColumnName("insumo_id");
            entity.Property(e => e.Cantidad)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cantidad");

            entity.HasOne(d => d.Insumo).WithMany(p => p.InsumosProductos)
                .HasForeignKey(d => d.InsumoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__insumos_p__insum__690797E6");

            entity.HasOne(d => d.Producto).WithMany(p => p.InsumosProductos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__insumos_p__productos__681373AD");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mesas__3213E83F9C6AB3D9");

            entity.ToTable("mesas");

            entity.HasIndex(e => e.Numero, "UQ__Mesas__FC77F211F7B15DD2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Numero).HasColumnName("numero");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pedidos__3213E83F4D8F25EE");

            entity.ToTable("pedidos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.EstadoId)
                .HasDefaultValue(1)
                .HasColumnName("estado_id");
            entity.Property(e => e.FechaEntrega)
                .HasColumnType("datetime")
                .HasColumnName("fecha_entrega");
            entity.Property(e => e.FechaPedido)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_pedido");
            entity.Property(e => e.Total)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK_Pedidos_Clientes");

            entity.HasOne(d => d.Estado).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Estados");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permisos__3213E83FECB92AB7");

            entity.ToTable("permisos");

            entity.HasIndex(e => e.Nombre, "UQ__Permisos__72AFBCC6CD734B5F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3213E83FD7211198");

            entity.ToTable("productos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.CategoriaProductoId).HasColumnName("categoria_producto_id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EstadoId).HasColumnName("estado_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.InsumoId).HasColumnName("insumo_id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.TieneInsumos).HasColumnName("tiene_insumos");

            entity.HasOne(d => d.CategoriaProducto).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaProductoId)
                .HasConstraintName("FK_Producto_Categoria");

            entity.HasOne(d => d.Estado).WithMany(p => p.Productos)
                .HasForeignKey(d => d.EstadoId)
                .HasConstraintName("FK_Productos_EstadoProducto");

            entity.HasOne(d => d.Insumo).WithMany(p => p.Productos)
                .HasForeignKey(d => d.InsumoId)
                .HasConstraintName("FK_productos_insumo");
        });

        modelBuilder.Entity<Promocione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promocio__3213E83FF99A89A1");

            entity.ToTable("promociones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("descuento");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasMany(d => d.Productos).WithMany(p => p.Promocions)
                .UsingEntity<Dictionary<string, object>>(
                    "PromocionProducto",
                    r => r.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .HasConstraintName("FK__Promocion__produ__75A278F5"),
                    l => l.HasOne<Promocione>().WithMany()
                        .HasForeignKey("PromocionId")
                        .HasConstraintName("FK__Promocion__promo__74AE54BC"),
                    j =>
                    {
                        j.HasKey("PromocionId", "ProductoId").HasName("PK__Promocio__3061FCFC3DC88059");
                        j.ToTable("promocion_producto");
                        j.IndexerProperty<int>("PromocionId").HasColumnName("promocion_id");
                        j.IndexerProperty<int>("ProductoId").HasColumnName("producto_id");
                    });
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__proveedo__3213E83F039ADCD6");

            entity.ToTable("proveedores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Direccion)
                .HasColumnType("text")
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservas__3213E83F4CC54156");

            entity.ToTable("reservas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.MesaId).HasColumnName("mesa_id");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Reservas__client__7E37BEF6");

            entity.HasOne(d => d.Mesa).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MesaId)
                .HasConstraintName("FK__Reservas__mesa_i__7F2BE32F");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3213E83FCCD14D46");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "UQ__Roles__72AFBCC6C852AA26").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasMany(d => d.Permisos).WithMany(p => p.Rols)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermiso",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("PermisoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Rol_Permi__permi__32AB8735"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Rol_Permi__rol_i__31B762FC"),
                    j =>
                    {
                        j.HasKey("RolId", "PermisoId").HasName("PK__Rol_Perm__0939B2DF81EC5DA6");
                        j.ToTable("rol_permiso");
                        j.IndexerProperty<int>("RolId").HasColumnName("rol_id");
                        j.IndexerProperty<int>("PermisoId").HasColumnName("permiso_id");
                    });
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stock__3213E83F44461705");

            entity.ToTable("stocks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadDisponible).HasColumnName("cantidad_disponible");
            entity.Property(e => e.CantidadEntrada).HasColumnName("cantidad_entrada");
            entity.Property(e => e.CantidadSalida).HasColumnName("cantidad_salida");
            entity.Property(e => e.FechaActualizacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_actualizacion");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");

            entity.HasOne(d => d.Producto).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("FK_Stock_Productos");
        });

        modelBuilder.Entity<UnidadMedidum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__unidad_m__3213E83FFBCE2E37");

            entity.ToTable("unidad_medida");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Abreviatura)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("abreviatura");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3213E83F46A57B7A");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__AB6E61647FBE1D3A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasMany(d => d.Permisos).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioPermiso",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("PermisoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Usuario_P__permi__2DE6D218"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Usuario_P__usuar__2CF2ADDF"),
                    j =>
                    {
                        j.HasKey("UsuarioId", "PermisoId").HasName("PK__Usuario___E8DC8433647C3E0A");
                        j.ToTable("usuario_permiso");
                        j.IndexerProperty<int>("UsuarioId").HasColumnName("usuario_id");
                        j.IndexerProperty<int>("PermisoId").HasColumnName("permiso_id");
                    });

            entity.HasMany(d => d.Rols).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioRol",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Usuario_R__rol_i__2A164134"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Usuario_R__usuar__29221CFB"),
                    j =>
                    {
                        j.HasKey("UsuarioId", "RolId").HasName("PK__Usuario___0224FCEB416D44D1");
                        j.ToTable("usuario_rol");
                        j.IndexerProperty<int>("UsuarioId").HasColumnName("usuario_id");
                        j.IndexerProperty<int>("RolId").HasColumnName("rol_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
