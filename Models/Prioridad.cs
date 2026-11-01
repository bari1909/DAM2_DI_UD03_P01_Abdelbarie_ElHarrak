namespace DAM2_DI_UD03_P01.Models
{
    /// <summary>
    /// Constantes y catálogo de prioridades admisibles en TaskFlow DAM.
    /// </summary>
    public static class Prioridad
    {
        public const string Baja = "Baja";
        public const string Media = "Media";
        public const string Alta = "Alta";

        public static readonly string[] Todas = { Baja, Media, Alta };
    }
}
