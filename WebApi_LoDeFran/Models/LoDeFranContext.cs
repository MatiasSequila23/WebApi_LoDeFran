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

    public virtual DbSet<Caja> Cajas { get; set; }

    public virtual DbSet<Calle> Calles { get; set; }

    public virtual DbSet<CategoriasProducto> CategoriasProductos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<CombosItem> CombosItems { get; set; }

    public virtual DbSet<Descuento> Descuentos { get; set; }

    public virtual DbSet<DescuentosProducto> DescuentosProductos { get; set; }

    public virtual DbSet<DetallesPedido> DetallesPedidos { get; set; }

    public virtual DbSet<Dia> Dias { get; set; }

    public virtual DbSet<EstadosCocina> EstadosCocinas { get; set; }

    public virtual DbSet<EstadosFactura> EstadosFacturas { get; set; }

    public virtual DbSet<EstadosInsumo> EstadosInsumos { get; set; }

    public virtual DbSet<EstadosMesa> EstadosMesas { get; set; }

    public virtual DbSet<EstadosPedido> EstadosPedidos { get; set; }

    public virtual DbSet<EstadosProducto> EstadosProductos { get; set; }

    public virtual DbSet<EstadosPromocione> EstadosPromociones { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Insumo> Insumos { get; set; }

    public virtual DbSet<InsumosProducto> InsumosProductos { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<MotivosMovimiento> MotivosMovimientos { get; set; }

    public virtual DbSet<MovimientoCaja> MovimientoCajas { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoCombo> PedidoCombos { get; set; }

    public virtual DbSet<PedidoComboItem> PedidoComboItems { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Piso> Pisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductosVariante> ProductosVariantes { get; set; }

    public virtual DbSet<PromocionDia> PromocionDias { get; set; }

    public virtual DbSet<Promocione> Promociones { get; set; }

    public virtual DbSet<PromocionesAplicacione> PromocionesAplicaciones { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<SubcategoriasProducto> SubcategoriasProductos { get; set; }

    public virtual DbSet<TipoDescuento> TipoDescuentos { get; set; }

    public virtual DbSet<TipoPromocion> TipoPromocions { get; set; }

    public virtual DbSet<TiposMovimiento> TiposMovimientos { get; set; }

    public virtual DbSet<TiposPedido> TiposPedidos { get; set; }

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

        modelBuilder.Entity<Caja>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__caja__3213E83F87FA4BDB");

            entity.ToTable("caja");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Diferencia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("diferencia");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaApertura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_apertura");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("datetime")
                .HasColumnName("fecha_cierre");
            entity.Property(e => e.MontoFinal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto_final");
            entity.Property(e => e.MontoInicial)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto_inicial");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Cajas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_caja_usuario");
        });

        modelBuilder.Entity<Calle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__calles__3213E83FA52F6002");

            entity.ToTable("calles");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AltDerFin).HasColumnName("altDerFin");
            entity.Property(e => e.AltDerIni).HasColumnName("altDerIni");
            entity.Property(e => e.AltIzqFin).HasColumnName("altIzqFin");
            entity.Property(e => e.AltIzqIni).HasColumnName("altIzqIni");
            entity.Property(e => e.Barrio)
                .HasMaxLength(100)
                .HasColumnName("barrio");
            entity.Property(e => e.BarrioImp)
                .HasMaxLength(100)
                .HasColumnName("barrioImp");
            entity.Property(e => e.BarrioPar)
                .HasMaxLength(100)
                .HasColumnName("barrioPar");
            entity.Property(e => e.Bicisenda)
                .HasMaxLength(100)
                .HasColumnName("bicisenda");
            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.ComImpar).HasColumnName("comImpar");
            entity.Property(e => e.ComPar).HasColumnName("comPar");
            entity.Property(e => e.Comuna).HasColumnName("comuna");
            entity.Property(e => e.NomAnter)
                .HasMaxLength(255)
                .HasColumnName("nomAnter");
            entity.Property(e => e.NomMapa)
                .HasMaxLength(255)
                .HasColumnName("nomMapa");
            entity.Property(e => e.NomOficial)
                .HasMaxLength(255)
                .HasColumnName("nomOficial");
            entity.Property(e => e.RedJerarq)
                .HasMaxLength(100)
                .HasColumnName("redJerarq");
            entity.Property(e => e.TipoC)
                .HasMaxLength(100)
                .HasColumnName("tipoC");
            entity.Property(e => e.TipoFfcc)
                .HasMaxLength(500)
                .HasColumnName("tipoFFCC");
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
            entity.Property(e => e.Altura)
                .HasMaxLength(20)
                .HasColumnName("altura");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.Calle)
                .HasMaxLength(150)
                .HasColumnName("calle");
            entity.Property(e => e.CalleId).HasColumnName("calle_id");
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
            entity.Property(e => e.Piso)
                .HasMaxLength(20)
                .HasColumnName("piso");
            entity.Property(e => e.PuntosFidelidad)
                .HasDefaultValue(0)
                .HasColumnName("puntos_fidelidad");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.CalleNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.CalleId)
                .HasConstraintName("FK_Clientes_Calles");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__combos__3213E83FDFA7C235");

            entity.ToTable("combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EstadoId).HasColumnName("estado_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");
        });

        modelBuilder.Entity<CombosItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__combos_i__3213E83F2DE8FE06");

            entity.ToTable("combos_items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(1)
                .HasColumnName("cantidad");
            entity.Property(e => e.CategoriaGrupo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("categoria_grupo");
            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.EsOpcional).HasColumnName("es_opcional");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");

            entity.HasOne(d => d.Combo).WithMany(p => p.CombosItems)
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__combos_it__combo__4EDDB18F");

            entity.HasOne(d => d.Producto).WithMany(p => p.CombosItems)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__combos_it__produ__4FD1D5C8");
        });

        modelBuilder.Entity<Descuento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__descuent__3213E83FF8DBCD3C");

            entity.ToTable("descuentos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EsAutomatico).HasColumnName("es_automatico");
            entity.Property(e => e.EstadoId).HasColumnName("estado_id");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valor");
        });

        modelBuilder.Entity<DescuentosProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__descuent__3213E83FB4E1853A");

            entity.ToTable("descuentosProductos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DescuentoId).HasColumnName("descuento_id");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");

            entity.HasOne(d => d.Descuento).WithMany(p => p.DescuentosProductos)
                .HasForeignKey(d => d.DescuentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__descuento__descu__558AAF1E");

            entity.HasOne(d => d.Producto).WithMany(p => p.DescuentosProductos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__descuento__produ__567ED357");
        });

        modelBuilder.Entity<DetallesPedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Detalles__3213E83F3686E91D");

            entity.ToTable("detalles_pedidos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CantidadConfirmada).HasColumnName("cantidad_confirmada");
            entity.Property(e => e.Comentario)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.EstadoCocinaId)
                .HasDefaultValue(1)
                .HasColumnName("estado_cocina_id");
            entity.Property(e => e.PedidoId).HasColumnName("pedido_id");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.EstadoCocina).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.EstadoCocinaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetallesPedidos_EstadoCocina");

            entity.HasOne(d => d.Pedido).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.PedidoId)
                .HasConstraintName("FK_DetallesPedido_Pedidos");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detalles_Producto");
        });

        modelBuilder.Entity<Dia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__dias__3213E83F00CC5A03");

            entity.ToTable("dias");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadosCocina>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__estados___3213E83FEB4F4733");

            entity.ToTable("estados_cocina");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadosFactura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__estados___3213E83F29A7675E");

            entity.ToTable("estados_facturas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
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

        modelBuilder.Entity<EstadosMesa>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__estados___86989FB2D1C31BE5");

            entity.ToTable("estados_mesa");

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
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

        modelBuilder.Entity<EstadosPromocione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__estados___3213E83F3C2C6CEC");

            entity.ToTable("estados_promociones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Facturas__3213E83FD14D3C68");

            entity.ToTable("facturas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CajaId).HasColumnName("caja_id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.CuitCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("cuit_cliente");
            entity.Property(e => e.DescuentoAplicado)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("descuento_aplicado");
            entity.Property(e => e.EstadoId)
                .HasDefaultValue(1)
                .HasColumnName("estado_id");
            entity.Property(e => e.FechaEmision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_emision");
            entity.Property(e => e.MetodoPagoId).HasColumnName("metodo_pago_id");
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numero_factura");
            entity.Property(e => e.Observaciones)
                .IsUnicode(false)
                .HasColumnName("observaciones");
            entity.Property(e => e.PedidoId).HasColumnName("pedido_id");
            entity.Property(e => e.TipoFactura)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipo_factura");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Caja).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.CajaId)
                .HasConstraintName("FK_factura_caja");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK_Factura_Cliente");

            entity.HasOne(d => d.Estado).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_factura_estado");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.MetodoPagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_factura_metodo_pago");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.PedidoId)
                .HasConstraintName("FK_Facturas_Pedidos");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_factura_usuario");
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
            entity.Property(e => e.IdEstado)
                .HasDefaultValue(1)
                .HasColumnName("id_estado");
            entity.Property(e => e.IdPiso)
                .HasDefaultValue(1)
                .HasColumnName("id_piso");
            entity.Property(e => e.Numero).HasColumnName("numero");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mesas_estados");

            entity.HasOne(d => d.IdPisoNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdPiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mesas_pisos");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__metodo_p__3213E83FAE3A1442");

            entity.ToTable("metodo_pago");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<MotivosMovimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__motivos___3213E83FE6F4B1EE");

            entity.ToTable("motivos_movimiento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoMovimientoId).HasColumnName("tipo_movimiento_id");

            entity.HasOne(d => d.TipoMovimiento).WithMany(p => p.MotivosMovimientos)
                .HasForeignKey(d => d.TipoMovimientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__motivos_m__tipo___0FEC5ADD");
        });

        modelBuilder.Entity<MovimientoCaja>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__movimien__3213E83F5D2C7FB2");

            entity.ToTable("movimiento_caja");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CajaId).HasColumnName("caja_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_movimiento");
            entity.Property(e => e.MetodoPagoId).HasColumnName("metodo_pago_id");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.MotivoMovimientoId).HasColumnName("motivo_movimiento_id");

            entity.HasOne(d => d.Caja).WithMany(p => p.MovimientoCajas)
                .HasForeignKey(d => d.CajaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_movimiento_caja_caja");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.MovimientoCajas)
                .HasForeignKey(d => d.MetodoPagoId)
                .HasConstraintName("fk_movimiento_caja_metodo_pago");

            entity.HasOne(d => d.MotivoMovimiento).WithMany(p => p.MovimientoCajas)
                .HasForeignKey(d => d.MotivoMovimientoId)
                .HasConstraintName("fk_movimiento_motivo");
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
            entity.Property(e => e.MesaId).HasColumnName("mesa_id");
            entity.Property(e => e.MontoDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto_descuento");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.PromocionId).HasColumnName("promocion_id");
            entity.Property(e => e.TipoPedidoId).HasColumnName("tipo_pedido_id");
            entity.Property(e => e.Total)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");
            entity.Property(e => e.TotalSinDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_sin_descuento");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK_Pedidos_Clientes");

            entity.HasOne(d => d.Estado).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Estados");

            entity.HasOne(d => d.Mesa).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.MesaId)
                .HasConstraintName("FK_Pedidos_Mesas");

            entity.HasOne(d => d.Promocion).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.PromocionId)
                .HasConstraintName("FK_Pedidos_Promociones");

            entity.HasOne(d => d.TipoPedido).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.TipoPedidoId)
                .HasConstraintName("FK_pedidos_tipos_pedido");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Pedido_Usuario");
        });

        modelBuilder.Entity<PedidoCombo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pedido_c__3213E83F53AF33E1");

            entity.ToTable("pedido_combo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(1)
                .HasColumnName("cantidad");
            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.Comentario)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.PedidoId).HasColumnName("pedido_id");

            entity.HasOne(d => d.Combo).WithMany(p => p.PedidoCombos)
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pedido_combo_combo");

            entity.HasOne(d => d.Pedido).WithMany(p => p.PedidoCombos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pedido_combo_pedido");
        });

        modelBuilder.Entity<PedidoComboItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pedido_c__3213E83FAFD02521");

            entity.ToTable("pedido_combo_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.PedidoComboId).HasColumnName("pedido_combo_id");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");

            entity.HasOne(d => d.PedidoCombo).WithMany(p => p.PedidoComboItems)
                .HasForeignKey(d => d.PedidoComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pedido_co__pedid__7226EDCC");

            entity.HasOne(d => d.Producto).WithMany(p => p.PedidoComboItems)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pedido_co__produ__731B1205");
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

        modelBuilder.Entity<Piso>(entity =>
        {
            entity.HasKey(e => e.IdPiso).HasName("PK__pisos__37C031BCA5364FF8");

            entity.ToTable("pisos");

            entity.Property(e => e.IdPiso).HasColumnName("id_piso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
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
            entity.Property(e => e.SubcategoriaProductoId).HasColumnName("subcategoria_producto_id");
            entity.Property(e => e.TieneInsumos).HasColumnName("tiene_insumos");
            entity.Property(e => e.TieneVariante).HasColumnName("tiene_variante");

            entity.HasOne(d => d.CategoriaProducto).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaProductoId)
                .HasConstraintName("FK_Productos_CategoriaProducto");

            entity.HasOne(d => d.Estado).WithMany(p => p.Productos)
                .HasForeignKey(d => d.EstadoId)
                .HasConstraintName("FK_Productos_EstadoProducto");

            entity.HasOne(d => d.SubcategoriaProducto).WithMany(p => p.Productos)
                .HasForeignKey(d => d.SubcategoriaProductoId)
                .HasConstraintName("FK_Productos_SubcategoriaProducto");
        });

        modelBuilder.Entity<ProductosVariante>(entity =>
        {
            entity.HasKey(e => e.IdVariante).HasName("PK__producto__EE2623B4A8CDE0D5");

            entity.ToTable("productosVariantes");

            entity.Property(e => e.IdVariante).HasColumnName("idVariante");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductosVariantes)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__productos__idPro__52442E1F");
        });

        modelBuilder.Entity<PromocionDia>(entity =>
        {
            entity.HasKey(e => new { e.PromocionId, e.DiaId }).HasName("PK__promocio__783F4F15A9111B4B");

            entity.ToTable("promocion_dias");

            entity.Property(e => e.PromocionId).HasColumnName("promocion_id");
            entity.Property(e => e.DiaId).HasColumnName("dia_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");

            entity.HasOne(d => d.Dia).WithMany(p => p.PromocionDia)
                .HasForeignKey(d => d.DiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promocion__dia_i__1D114BD1");

            entity.HasOne(d => d.Promocion).WithMany(p => p.PromocionDia)
                .HasForeignKey(d => d.PromocionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promocion__promo__1C1D2798");
        });

        modelBuilder.Entity<Promocione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__promocio__3213E83FC69FFA71");

            entity.ToTable("promociones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AplicacionId).HasColumnName("aplicacion_id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EstadoId).HasColumnName("estado_id");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.MontoMinimo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto_minimo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoDescuentoId)
                .HasDefaultValue(1)
                .HasColumnName("tipo_descuento_id");
            entity.Property(e => e.TipoPromocionId).HasColumnName("tipo_promocion_id");
            entity.Property(e => e.ValorDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valor_descuento");

            entity.HasOne(d => d.Aplicacion).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.AplicacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promocion__aplic__09FE775D");

            entity.HasOne(d => d.Estado).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.EstadoId)
                .HasConstraintName("FK__promocion__estad__090A5324");

            entity.HasOne(d => d.TipoDescuento).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.TipoDescuentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Promociones_tipo_descuento");

            entity.HasOne(d => d.TipoPromocion).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.TipoPromocionId)
                .HasConstraintName("FK_Promociones_TipoPromocion");
        });

        modelBuilder.Entity<PromocionesAplicacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__promocio__3213E83F8895BC2B");

            entity.ToTable("promociones_aplicaciones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
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

        modelBuilder.Entity<SubcategoriasProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__subcateg__3213E83F11E544CB");

            entity.ToTable("subcategorias_productos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");

            entity.HasOne(d => d.Categoria).WithMany(p => p.SubcategoriasProductos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__subcatego__categ__2C1E8537");
        });

        modelBuilder.Entity<TipoDescuento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tipo_des__3214EC073D0B020D");

            entity.ToTable("tipo_descuento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TipoPromocion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tipo_pro__3214EC07130461FF");

            entity.ToTable("tipo_promocion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TiposMovimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tipos_mo__3213E83F57B59AA5");

            entity.ToTable("tipos_movimiento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TiposPedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tipos_pe__3213E83F911A0FFC");

            entity.ToTable("tipos_pedido");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
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
