namespace BibliotecaApi.Dtos
{
    public class LibroDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
        public bool Disponible { get; set; }

        // Solo el nombre de la categoría, no el objeto completo
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}