using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Dtos.Books;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IReadOnlyList<BookDto>>>> get()
        {
            var booksDb = await _context.Books
                .Include(b => b.Autor)
                .ToListAsync();

            var booksDto = _mapper.Map<List<BookDto>>(booksDb);

            return new ResponseDto<IReadOnlyList<BookDto>>
            {
                Status = true,
                Data = booksDto
            };
        }


        [HttpGet("{id:Guid}")] //   api/books/ADSA87D6ADC87
        public async Task<ActionResult<ResponseDto<BookDto>>> Get(Guid id)
        {
            var bookDb = await _context.Books
               .Include(b => b.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bookDb == null)
            {
                return NotFound(new ResponseDto<BookDto>
                {
                    Status = false,
                    Message = $"El libro con el Id {id} no fue encontrado"
                });

                var bookDto = _mapper.Map<BookDto>(bookDb);

                return Ok(new ResponseDto<BookDto>
                {
                    Status = true,
                    Data = bookDto
                });
            }

            [HttpPost]
            public async Task<ActionResult> Post(BookCreateDto dto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Error en la peticion");
                }

                var autorExiste = await _context.Autores.AnyAsync(x => x.Id == dto.AutorId);

                if (!autorExiste)
                {
                    return BadRequest($"No Existe el Autor: {dto.AutorId}");
                }

                var book = _mapper.Map<Book>(dto);

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return Ok("Libro creado correctamente");
            }

            [HttpPut("{id:Guid}")] // api/books/DFGFHDJK-KOO-87
            public async Task<ActionResult> Put(BookUpdateDto dto, Guid id)
            {
                var bookDb = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

                if (bookDb is null)
                {
                    return NotFound("No existe el libro: {id}");
                }

                var autorExiste = await _context.Autores.AnyAsync(x => x.Id == dto.AutorId);

                if (!autorExiste)
                {
                    return BadRequest($"No Existe el Autor: {dto.AutorId}");
                }

                _mapper.Map<BookUpdateDto, Book>(dto, bookDb);

                _context.Update(bookDb);

                await _context.SaveChangesAsync();

                return Ok();
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult> Delete(Guid id)
            {
                var bookExist = await _context.Books.AnyAsync(x => x.Id == id);

                if (!bookExist)
                {
                    return NotFound("Libro no encontrado");
                }

                _context.Remove(new Book { Id = id });

                await _context.SaveChangesAsync();

                return Ok(new { msg = $"Libro {id} Borrado" });
            }
        }
    }
}