using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos.Autores;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AutorDto>>> Get()
        {
            var autoresDb = await _context.Autores.ToListAsync();
            var autoresDto = _mapper.Map<List<AutorDto>>(autoresDb);
            return autoresDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(AutorCreateDto dto)
        {
            var autor = _mapper.Map<Autor>(dto);

            _context.Add(autor);
            await _context.SaveChangesAsync();

            var autorDto = _mapper.Map<AutorDto>(autor);

            return StatusCode(StatusCodes.Status201Created, autorDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorGetByIdDto>> GetOneById(int id)
        {
            //obtener varaiable de la base de datos
            var autorDb =  await _context.Autores
                .Include(a => a.Books)
                .FirstOrDefaultAsync(x => x.Id == id);

            var autorDto = _mapper.Map<AutorGetByIdDto>(autorDb);

            return autorDto;
        }

        [HttpPut("{id:int}")] // api/autores/1
        public async  Task<IActionResult> Put(int id, AutorUpdateDto dto)
        {
            var autorDb = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autorDb is null)
            {
                return NotFound($"Autor {id} no fue encontrado");
            }

            _mapper.Map<AutorUpdateDto, Autor>(dto, autorDb);

            _context.Update(autorDb);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor is null)
            {
                return NotFound("Autor no encontrado");
            }

            _context.Remove(autor);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
