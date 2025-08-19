namespace BibliotecaApi.Dtos
{
    public class LibroCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
        public bool Disponible { get; set; }
        public int CategoriaId { get; set; }
    }
}