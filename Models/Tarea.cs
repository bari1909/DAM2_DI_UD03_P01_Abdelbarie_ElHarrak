using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DAM2_DI_UD03_P01.Models
{
    /// <summary>
    /// Entidad de datos Tarea que implementa INotifyPropertyChanged
    /// para permitir enlaces reactivos bidireccionales con la interfaz de usuario en WPF.
    /// Mapeada automáticamente por Entity Framework Core a la tabla 'Tareas' en SQLite.
    /// </summary>
    public class Tarea : INotifyPropertyChanged
    {
        private int _id;
        private string _titulo = string.Empty;
        private string _modulo = "DI";
        private DateTime? _fechaLimite;
        private string _prioridad = "Media";
        private bool _esUrgente;
        private bool _completada;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Titulo
        {
            get => _titulo;
            set
            {
                if (_titulo != value)
                {
                    _titulo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Modulo
        {
            get => _modulo;
            set
            {
                if (_modulo != value)
                {
                    _modulo = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? FechaLimite
        {
            get => _fechaLimite;
            set
            {
                if (_fechaLimite != value)
                {
                    _fechaLimite = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FechaLimiteFormateada));
                }
            }
        }

        public string Prioridad
        {
            get => _prioridad;
            set
            {
                if (_prioridad != value)
                {
                    _prioridad = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool EsUrgente
        {
            get => _esUrgente;
            set
            {
                if (_esUrgente != value)
                {
                    _esUrgente = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Completada
        {
            get => _completada;
            set
            {
                if (_completada != value)
                {
                    _completada = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Propiedad calculada auxiliar para mostrar la fecha de vencimiento en la lista.
        /// </summary>
        public string FechaLimiteFormateada =>
            FechaLimite.HasValue ? FechaLimite.Value.ToString("dd/MM/yyyy") : "Sin fecha";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
