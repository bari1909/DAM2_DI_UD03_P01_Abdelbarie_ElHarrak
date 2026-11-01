using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using DAM2_DI_UD03_P01.Models;

namespace DAM2_DI_UD03_P01.Data
{
    /// <summary>
    /// Contexto de Entity Framework Core para la base de datos local SQLite de TaskFlow DAM.
    /// Administra la persistencia de las tareas en un archivo local 'taskflow.db'.
    /// </summary>
    public class TaskFlowDbContext : DbContext
    {
        public DbSet<Tarea> Tareas { get; set; } = null!;

        public string DbPath { get; }

        public TaskFlowDbContext()
        {
            // Ubica la base de datos SQLite en la carpeta del ejecutable
            string appFolder = AppDomain.CurrentDomain.BaseDirectory;
            DbPath = Path.Combine(appFolder, "taskflow.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={DbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Tarea en SQLite
            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Modulo).IsRequired().HasMaxLength(20);
                entity.Property(t => t.Prioridad).IsRequired().HasMaxLength(15);
                entity.Property(t => t.EsUrgente).IsRequired();
                entity.Property(t => t.Completada).IsRequired();
            });
        }

        /// <summary>
        /// Asegura la creación del archivo SQLite e inserta tareas iniciales de prueba
        /// si la tabla se encuentra completamente vacía.
        /// </summary>
        public void InicializarBaseDatos()
        {
            Database.EnsureCreated();

            if (!Tareas.Any())
            {
                Tareas.AddRange(
                    new Tarea
                    {
                        Titulo = "Repasar árboles lógico y visual en XAML",
                        Modulo = "DI",
                        FechaLimite = DateTime.Today.AddDays(2),
                        Prioridad = Prioridad.Alta,
                        EsUrgente = true,
                        Completada = true
                    },
                    new Tarea
                    {
                        Titulo = "Instalar extensiones de Figma para VS Code",
                        Modulo = "DI",
                        FechaLimite = DateTime.Today.AddDays(4),
                        Prioridad = Prioridad.Media,
                        EsUrgente = false,
                        Completada = true
                    },
                    new Tarea
                    {
                        Titulo = "Diseñar wireframe de la biblioteca escolar",
                        Modulo = "DI",
                        FechaLimite = DateTime.Today.AddDays(6),
                        Prioridad = Prioridad.Alta,
                        EsUrgente = false,
                        Completada = false
                    },
                    new Tarea
                    {
                        Titulo = "Configurar contenedor Docker para Odoo 18",
                        Modulo = "SGE",
                        FechaLimite = DateTime.Today.AddDays(8),
                        Prioridad = Prioridad.Media,
                        EsUrgente = false,
                        Completada = false
                    },
                    new Tarea
                    {
                        Titulo = "Crear repositorio GitHub para la práctica",
                        Modulo = "DI",
                        FechaLimite = DateTime.Today.AddDays(1),
                        Prioridad = Prioridad.Baja,
                        EsUrgente = false,
                        Completada = true
                    }
                );

                SaveChanges();
            }
        }
    }
}
