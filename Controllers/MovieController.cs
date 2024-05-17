using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI_dotnet.Data;
using MovieAPI_dotnet.Dtos;
using MovieAPI_dotnet.Dtos.Requests.Movies;
using MovieAPI_dotnet.Dtos.Responses.Movies;
using MovieAPI_dotnet.Helpers;
using MovieAPI_dotnet.Models;
using System.Text.Json;

namespace MovieAPI_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly AppDbContext _context;

        public MovieController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetAll([FromQuery] IndexDto index) 
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(index.search))
            {
                query = query.Where(movie => movie.Title.Contains(index.search));
            }

            var paginatedList = await PaginatedList<Movie>.CreateAsync(query.AsNoTracking(), index.pageNumber, index.pageSize);

            var response = new PaginatedResponse<Movie>
            {
                Meta = new PaginationMeta 
                {
                    PageIndex = paginatedList.CurrentPage,
                    TotalPages = paginatedList.TotalPages,
                    PageSize = paginatedList.PageSize,
                    TotalCount = paginatedList.TotalCount,
                    HasPreviousPage = paginatedList.HasPrevious,
                    HasNextPage = paginatedList.HasNext,
                },
                Data = paginatedList.ToList()
            };


            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetDetail(int id) 
        {
            var movie = await _context.Movies
                .Include(m => m.UserMovies)
                .ThenInclude(um => um.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(Movie => Movie.Id == id);

            if (movie == null)
            {   
                return NotFound();
            }

            var response = new MovieDetailResponseDto.MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Users = movie.UserMovies.Select(um => new MovieDetailResponseDto.UserDto
                {
                    Id = um.User?.Id,
                    Name = um.User?.Name,
                }).ToList() ?? new List<MovieDetailResponseDto.UserDto>(),
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieCreateRequestDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var movie = new Movie
            {
                Title = createDto.Title,
                Description = createDto.Description,
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieCreateRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = updateDto.Title;
            movie.Description = updateDto.Description;

            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromBody] MoviesBulkDeleteRequestDto bulkDeleteDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var moviesToDelete = await _context.Movies
                .Where(m => bulkDeleteDto.Ids.Contains(m.Id))
                .ToListAsync();

            if (moviesToDelete == null || moviesToDelete.Count() == 0)
            {
                return NotFound();
            }

            _context.Movies.RemoveRange(moviesToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
