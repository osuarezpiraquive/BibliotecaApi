using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Data;
using BibliotecaApi.Models;
using BibliotecaApi.Dtos;

namespace BibliotecaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LibrosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDto>>> GetLibros()
        {
            return await _context.Libros
                .Include(l => l.Categoria)
                .Select(l => new LibroDto
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    Autor = l.Autor,
                    ISBN = l.ISBN,
                    AnioPublicacion = l.AnioPublicacion,
                    Disponible = l.Disponible,
                    CategoriaNombre = l.Categoria.Nombre
                })
                .ToListAsync();
        }

        // GET: api/libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDto>> ObtenerLibro(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Categoria)
                .Where(l => l.Id == id)
                .Select(l => new LibroDto
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    Autor = l.Autor,
                    ISBN = l.ISBN,
                    AnioPublicacion = l.AnioPublicacion,
                    Disponible = l.Disponible,
                    CategoriaNombre = l.Categoria.Nombre
                })
                .FirstOrDefaultAsync();

            if (libro == null)
                return NotFound();

            return libro;
        }

        // POST: api/libros
        [HttpPost]
        public async Task<ActionResult<LibroDto>> CrearLibro(LibroCreateDto dto)
        {
            var libro = new Libro
            {
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                ISBN = dto.ISBN,
                AnioPublicacion = dto.AnioPublicacion,
                Disponible = dto.Disponible,
                CategoriaId = dto.CategoriaId
            };

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerLibro), new { id = libro.Id }, dto);
        }

        // PUT: api/libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarLibro(int id, LibroCreateDto dto)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                return NotFound();

            libro.Titulo = dto.Titulo;
            libro.Autor = dto.Autor;
            libro.ISBN = dto.ISBN;
            libro.AnioPublicacion = dto.AnioPublicacion;
            libro.Disponible = dto.Disponible;
            libro.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                return NotFound();

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}