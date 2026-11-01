using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAM2_DI_UD03_P01.Data;
using DAM2_DI_UD03_P01.Models;

namespace DAM2_DI_UD03_P01
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml.
    /// Implementa Data Binding directo mediante DataContext y gestiona la persistencia
    /// automática en SQLite utilizando Entity Framework Core (TaskFlowDbContext).
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly TaskFlowDbContext _context;

        // Propiedades enlazadas del formulario de inserción
        private string _nuevoTitulo = string.Empty;
        private string _nuevoModulo = "DI";
        private DateTime? _nuevaFechaLimite = DateTime.Today.AddDays(1);
        private string _nuevaPrioridad = Prioridad.Media;
        private bool _nuevoEsUrgente = false;

        public string NuevoTitulo
        {
            get { return _nuevoTitulo; }
            set
            {
                _nuevoTitulo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PuedeAnadir));
            }
        }

        public string NuevoModulo
        {
            get { return _nuevoModulo; }
            set
            {
                _nuevoModulo = value;
                OnPropertyChanged();
            }
        }

        public DateTime? NuevaFechaLimite
        {
            get { return _nuevaFechaLimite; }
            set
            {
                _nuevaFechaLimite = value;
                OnPropertyChanged();
            }
        }

        public string NuevaPrioridad
        {
            get { return _nuevaPrioridad; }
            set
            {
                _nuevaPrioridad = value;
                OnPropertyChanged();
            }
        }

        public bool NuevoEsUrgente
        {
            get { return _nuevoEsUrgente; }
            set
            {
                _nuevoEsUrgente = value;
                OnPropertyChanged();
            }
        }

        // 4.4.5 E. Prevención Activa de Errores: deshabilitado si el título está vacío o solo tiene espacios
        public bool PuedeAnadir
        {
            get { return !string.IsNullOrWhiteSpace(NuevoTitulo); }
        }

        // Propiedades reactivas para métricas y estado
        private double _progresoPorcentaje;
        private string _textoProgreso = string.Empty;
        private int _tareasPendientesCount;
        private string _estadoApp = "Listo para operar";
        private bool _noHayTareas;

        public double ProgresoPorcentaje
        {
            get { return _progresoPorcentaje; }
            set
            {
                _progresoPorcentaje = value;
                OnPropertyChanged();
            }
        }

        public string TextoProgreso
        {
            get { return _textoProgreso; }
            set
            {
                _textoProgreso = value;
                OnPropertyChanged();
            }
        }

        public int TareasPendientesCount
        {
            get { return _tareasPendientesCount; }
            set
            {
                _tareasPendientesCount = value;
                OnPropertyChanged();
            }
        }

        public string EstadoApp
        {
            get { return _estadoApp; }
            set
            {
                _estadoApp = value;
                OnPropertyChanged();
            }
        }

        public bool NoHayTareas
        {
            get { return _noHayTareas; }
            set
            {
                _noHayTareas = value;
                OnPropertyChanged();
            }
        }

        public string FechaHoy { get; }

        public ObservableCollection<Tarea> Tareas { get; } = new();


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            CultureInfo cultura = new CultureInfo("es-ES");
            string fecha = DateTime.Today.ToString("dddd, d 'de' MMMM 'de' yyyy", cultura);
            FechaHoy = char.ToUpper(fecha[0]) + fecha.Substring(1);

            _context = new TaskFlowDbContext();
            _context.InicializarBaseDatos();

            CargarTareasDesdeSqlite();
            Tareas.CollectionChanged += Tareas_CollectionChanged;

            ActualizarMetricas();

            DpFechaLimite.Loaded += DpFechaLimite_Loaded;
        }

        // AutomationProperties.Name y HelpText también en el botón del calendario del DatePicker
        private void DpFechaLimite_Loaded(object sender, RoutedEventArgs e)
        {
            if (DpFechaLimite.Template.FindName("PART_Button", DpFechaLimite) is Button botonCalendario)
            {
                System.Windows.Automation.AutomationProperties.SetName(botonCalendario, "Abrir calendario");
                System.Windows.Automation.AutomationProperties.SetHelpText(botonCalendario, "Muestra un calendario para elegir la fecha límite");
            }
        }

        /// <summary>
        /// Recupera las tareas desde SQLite a través de EF Core y las suscribe a la colección observable.
        /// </summary>
        private void CargarTareasDesdeSqlite()
        {
            var listaTareas = _context.Tareas.OrderBy(t => t.Completada).ThenByDescending(t => t.Id).ToList();
            Tareas.Clear();

            foreach (var tarea in listaTareas)
            {
                tarea.PropertyChanged += Tarea_PropertyChanged;
                Tareas.Add(tarea);
            }
        }

        // 4.2 Sincronización en tiempo real: evento CollectionChanged de la colección Tareas
        private void Tareas_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Tarea tarea in e.NewItems)
                {
                    tarea.PropertyChanged += Tarea_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Tarea tarea in e.OldItems)
                {
                    tarea.PropertyChanged -= Tarea_PropertyChanged;
                }
            }

            ActualizarMetricas();
        }

        // 4.2 Sincronización en tiempo real: evento PropertyChanged de cada elemento Tarea
        private void Tarea_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Tarea.Completada) && sender is Tarea tarea)
            {
                _context.SaveChanges();

                ActualizarMetricas();

                if (tarea.Completada)
                {
                    EstadoApp = $"Tarea «{tarea.Titulo}» completada";
                }
                else
                {
                    EstadoApp = $"Tarea «{tarea.Titulo}» marcada como pendiente";
                }
            }
        }

        // 4.2 Propiedades reactivas calculadas: ProgresoPorcentaje, TextoProgreso y TareasPendientesCount
        private void ActualizarMetricas()
        {
            int total = Tareas.Count;
            int completadas = 0;

            foreach (Tarea tarea in Tareas)
            {
                if (tarea.Completada)
                {
                    completadas++;
                }
            }

            // Si no hay tareas, debe valer 0
            if (total > 0)
            {
                ProgresoPorcentaje = (double)completadas * 100 / total;
            }
            else
            {
                ProgresoPorcentaje = 0;
            }

            TareasPendientesCount = total - completadas;
            TextoProgreso = $"{completadas} de {total} completadas ({ProgresoPorcentaje:0}%)";
            NoHayTareas = total == 0;
        }

        /// <summary>
        /// Añade una nueva tarea a la base de datos SQLite y a la colección reactiva.
        /// Activado por clic o pulsando la tecla Enter gracias a IsDefault="True".
        /// </summary>
        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NuevoTitulo))
            {
                MessageBox.Show("Por favor, introduzca un título para la tarea.", "Campo Obligatorio",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NuevaFechaLimite != null && NuevaFechaLimite.Value.Date < DateTime.Today)
            {
                MessageBox.Show("La fecha límite no puede ser anterior a hoy. Elige otra fecha en el calendario.",
                    "Fecha no válida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var nuevaTarea = new Tarea
            {
                Titulo = NuevoTitulo.Trim(),
                Modulo = NuevoModulo,
                FechaLimite = NuevaFechaLimite,
                Prioridad = NuevaPrioridad,
                EsUrgente = NuevoEsUrgente,
                Completada = false
            };

            // 1. Guardar en SQLite mediante Entity Framework Core
            try
            {
                _context.Tareas.Add(nuevaTarea);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                _context.Tareas.Remove(nuevaTarea);
                MessageBox.Show("No se ha podido guardar la tarea. Inténtalo de nuevo y, si el problema continúa, reinicia la aplicación.",
                    "Error al guardar", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 2. Suscribir y añadir a la ObservableCollection para reflejar en XAML
            Tareas.Insert(0, nuevaTarea);

            // 3. Limpiar formulario y notificar
            BtnLimpiar_Click(sender, e);
            EstadoApp = "Tarea agregada con éxito";
        }

        /// <summary>
        /// Restablece los campos del formulario de entrada.
        /// Activado por clic o pulsando la tecla Escape gracias a IsCancel="True".
        /// </summary>
        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            NuevoTitulo = string.Empty;
            NuevoModulo = "DI";
            NuevaFechaLimite = DateTime.Today.AddDays(1);
            NuevaPrioridad = Prioridad.Media;
            NuevoEsUrgente = false;

            EstadoApp = "Formulario limpiado";

            TxtTitulo.Focus();
        }

        /// <summary>
        /// Elimina una tarea individual de SQLite y del listado enlazado.
        /// </summary>
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Tarea tarea)
            {
                var resultado = MessageBox.Show(
                    $"¿Desea eliminar la tarea '{tarea.Titulo}' de la base de datos?",
                    "Confirmar Eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    _context.Tareas.Remove(tarea);
                    _context.SaveChanges();
                    Tareas.Remove(tarea);

                    EstadoApp = "Tarea eliminada";
                }
            }
        }

        private void BtnNotificaciones_Click(object sender, RoutedEventArgs e)
        {
            int urgentes = 0;
            foreach (Tarea tarea in Tareas)
            {
                if (tarea.EsUrgente && !tarea.Completada)
                {
                    urgentes++;
                }
            }

            MessageBox.Show($"Tienes {TareasPendientesCount} tareas pendientes, de las cuales {urgentes} son urgentes.",
                "Resumen de tareas", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // H10: Atajo F1 para abrir la guía rápida
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                MessageBox.Show(
                    "Atajos de teclado de TaskFlow DAM:\n\n" +
                    "Alt + T  →  Título de la tarea\n" +
                    "Alt + M  →  Módulo formativo\n" +
                    "Alt + F  →  Fecha límite\n" +
                    "Alt + P  →  Prioridad (cambia con las flechas)\n" +
                    "Alt + U  →  Marcar como urgente\n" +
                    "Enter    →  Añadir la tarea\n" +
                    "Esc      →  Limpiar el formulario\n" +
                    "Tab      →  Pasar al siguiente campo",
                    "Ayuda rápida", MessageBoxButton.OK, MessageBoxImage.Information);

                e.Handled = true;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _context.Dispose();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
