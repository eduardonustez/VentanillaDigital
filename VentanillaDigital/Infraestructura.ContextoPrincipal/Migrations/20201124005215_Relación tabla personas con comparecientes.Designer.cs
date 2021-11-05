﻿// <auto-generated />
using System;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    [DbContext(typeof(UnidadTrabajo))]
    [Migration("20201124005215_Relación tabla personas con comparecientes")]
    partial class Relacióntablapersonasconcomparecientes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Categoria", b =>
                {
                    b.Property<long>("CategoriaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoriaId");

                    b.ToTable("Categorias","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Colaborador", b =>
                {
                    b.Property<long>("ColaboradorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Area")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Cargo")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("NotariaId")
                        .HasColumnType("bigint");

                    b.Property<string>("NumeroDocumento")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("TipoDocumentoId")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ColaboradorId");

                    b.HasIndex("NotariaId");

                    b.ToTable("Colaboradores","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.EstadoTramite", b =>
                {
                    b.Property<long>("EstadoTramiteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EstadoTramiteId");

                    b.ToTable("EstadosTramites","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Notaria", b =>
                {
                    b.Property<long>("NotariaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<long>("UbicacionId")
                        .HasColumnType("bigint");

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotariaId");

                    b.HasIndex("UbicacionId");

                    b.ToTable("Notarias","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.NotariaUsuarios", b =>
                {
                    b.Property<long>("NotariaUsuariosId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("NotariaId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(34)")
                        .HasMaxLength(34);

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuariosId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotariaUsuariosId");

                    b.HasIndex("NotariaId");

                    b.ToTable("NotariasUsuario","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Notario", b =>
                {
                    b.Property<long>("NotarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nit")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<long>("NotariaId")
                        .HasColumnType("bigint");

                    b.Property<string>("NumeroDocumento")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("TipoDocumentoId")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotarioId");

                    b.HasIndex("NotariaId")
                        .IsUnique();

                    b.ToTable("Notarios","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Persona", b =>
                {
                    b.Property<long>("PersonaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("nvarchar(120)")
                        .HasMaxLength(120);

                    b.Property<string>("AspNetUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(900)")
                        .HasMaxLength(900);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("NumeroCelular")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("NumeroDocumento")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("TipoDocumentoId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonaId");

                    b.HasIndex("TipoDocumentoId", "NumeroDocumento")
                        .IsUnique();

                    b.ToTable("Personas","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.TipoIdentificacion", b =>
                {
                    b.Property<int>("TipoIdentificacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abreviatura")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TipoIdentificacionId");

                    b.ToTable("TiposIdentificacion","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.TipoTramite", b =>
                {
                    b.Property<long>("TipoTramiteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoriaId")
                        .HasColumnType("bigint");

                    b.Property<long>("CodigoTramite")
                        .HasColumnType("bigint");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TipoTramiteId");

                    b.HasIndex("CategoriaId");

                    b.ToTable("TiposTramites","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Ubicacion", b =>
                {
                    b.Property<long>("UbicacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UbicacionPadreId")
                        .HasColumnType("bigint");

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UbicacionId");

                    b.HasIndex("UbicacionPadreId");

                    b.ToTable("Ubicaciones","Parametricas");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Transaccional.Archivo", b =>
                {
                    b.Property<long>("ArchivoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Contenido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("MetadataArchivoId")
                        .HasColumnType("bigint");

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArchivoId");

                    b.HasIndex("MetadataArchivoId")
                        .IsUnique();

                    b.ToTable("Archivos","Transaccional");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Transaccional.Compareciente", b =>
                {
                    b.Property<long>("ComparecienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<long>("FirmaId")
                        .HasColumnType("bigint");

                    b.Property<long>("FotoId")
                        .HasColumnType("bigint");

                    b.Property<long>("ImagenDocumentoId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("NumeroDocumento")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<long>("PersonaId")
                        .HasColumnType("bigint");

                    b.Property<int>("TipoDocumentoId")
                        .HasColumnType("int");

                    b.Property<long>("TramiteId")
                        .HasColumnType("bigint");

                    b.Property<long>("TransaccionRNECId")
                        .HasColumnType("bigint");

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ComparecienteId");

                    b.HasIndex("FirmaId")
                        .IsUnique();

                    b.HasIndex("FotoId")
                        .IsUnique();

                    b.HasIndex("ImagenDocumentoId")
                        .IsUnique();

                    b.HasIndex("PersonaId");

                    b.HasIndex("TramiteId");

                    b.ToTable("Comparecientes","Transaccional");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Transaccional.MetadataArchivo", b =>
                {
                    b.Property<long>("MetadataArchivoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Ruta")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<long>("Tamanio")
                        .HasColumnType("bigint");

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MetadataArchivoId");

                    b.ToTable("MetadataArchivos","Transaccional");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Transaccional.Tramite", b =>
                {
                    b.Property<long>("TramiteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CantidadComparecientes")
                        .HasColumnType("int");

                    b.Property<long>("EstadoTramiteId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Fecha")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2020, 11, 23, 19, 52, 15, 524, DateTimeKind.Local).AddTicks(5978));

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("NotariaId")
                        .HasColumnType("bigint");

                    b.Property<long>("TipoTramiteId")
                        .HasColumnType("bigint");

                    b.Property<string>("UsuarioCreacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioModificacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TramiteId");

                    b.HasIndex("EstadoTramiteId");

                    b.HasIndex("NotariaId");

                    b.HasIndex("TipoTramiteId");

                    b.ToTable("Tramites","Transaccional");
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Colaborador", b =>
                {
                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.Notaria", "Notaria")
                        .WithMany("Colaboradores")
                        .HasForeignKey("NotariaId")
                        .HasConstraintName("FK_Colaboradores_Notarias")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Notaria", b =>
                {
                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.Ubicacion", "Ubicacion")
                        .WithMany()
                        .HasForeignKey("UbicacionId")
                        .HasConstraintName("FK_Notarias_Ubicaciones")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.NotariaUsuarios", b =>
                {
                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.Notaria", "Notaria")
                        .WithMany("NotariasUsuarios")
                        .HasForeignKey("NotariaId")
                        .HasConstraintName("FK_Notarias_Usuarios")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Notario", b =>
                {
                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.Notaria", "Notaria")
                        .WithOne("Notario")
                        .HasForeignKey("Dominio.ContextoPrincipal.Entidad.Parametricas.Notario", "NotariaId")
                        .HasConstraintName("FK_Notarios_Notarias")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.TipoTramite", b =>
                {
                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.Categoria", "Categoria")
                        .WithMany("TiposTramites")
                        .HasForeignKey("CategoriaId")
                        .HasConstraintName("FK_TiposTramites_Categorias")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Parametricas.Ubicacion", b =>
                {
                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.Ubicacion", "UbicacionPadre")
                        .WithMany("UbicacionesHijo")
                        .HasForeignKey("UbicacionPadreId")
                        .HasConstraintName("FK_Ubicacion_UbicacionPadre")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Transaccional.Archivo", b =>
                {
                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Transaccional.MetadataArchivo", "MetadataArchivo")
                        .WithOne("Archivo")
                        .HasForeignKey("Dominio.ContextoPrincipal.Entidad.Transaccional.Archivo", "MetadataArchivoId")
                        .HasConstraintName("FK_Archivos_MetadataArchivos")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Transaccional.Compareciente", b =>
                {
                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Transaccional.MetadataArchivo", "Firma")
                        .WithOne()
                        .HasForeignKey("Dominio.ContextoPrincipal.Entidad.Transaccional.Compareciente", "FirmaId")
                        .HasConstraintName("FK_Firmas_Comparecientes")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Transaccional.MetadataArchivo", "Foto")
                        .WithOne()
                        .HasForeignKey("Dominio.ContextoPrincipal.Entidad.Transaccional.Compareciente", "FotoId")
                        .HasConstraintName("FK_Fotos_Comparecientes")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Transaccional.MetadataArchivo", "ImagenDocumento")
                        .WithOne()
                        .HasForeignKey("Dominio.ContextoPrincipal.Entidad.Transaccional.Compareciente", "ImagenDocumentoId")
                        .HasConstraintName("FK_ImagenesDocumento_Comparecientes")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.Persona", "Persona")
                        .WithMany()
                        .HasForeignKey("PersonaId")
                        .HasConstraintName("FK_Personas_Comparecientes")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Transaccional.Tramite", "Tramite")
                        .WithMany("Comparecientes")
                        .HasForeignKey("TramiteId")
                        .HasConstraintName("FK_Comparecientes_Tramites")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.ContextoPrincipal.Entidad.Transaccional.Tramite", b =>
                {
                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.EstadoTramite", "EstadoTramite")
                        .WithMany()
                        .HasForeignKey("EstadoTramiteId")
                        .HasConstraintName("FK_Tramites_EstadosTramites")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.Notaria", "Notaria")
                        .WithMany()
                        .HasForeignKey("NotariaId")
                        .HasConstraintName("FK_Tramites_Notarias")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Dominio.ContextoPrincipal.Entidad.Parametricas.TipoTramite", "TipoTramite")
                        .WithMany()
                        .HasForeignKey("TipoTramiteId")
                        .HasConstraintName("FK_Tramites_TiposTramites")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
