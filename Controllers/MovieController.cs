using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI_dotnet.Dtos;
using MovieAPI_dotnet.Dtos.Requests.Movies;
using MovieAPI_dotnet.Dtos.Responses.Movies;
using MovieAPI_dotnet.Models;
using MovieAPI_dotnet.Repositories.Movies;

namespace MovieAPI_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _repository;

        public MovieController(IMovieRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetAll([FromQuery] IndexDto index) 
        {
            var response = await _repository.GetPaginatedMoviesAsync(index);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetDetail(int id) 
        {
            var movie = await _repository.GetMovieById(id);

            if (movie == null)
            {   
                return NotFound();
            }

            var response = new MovieDetailResponseDto.MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                AiringDate = movie.AiringDate,
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
                AiringDate = createDto.airingDate,
            };

            await _repository.AddMovie(movie);

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieCreateRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var movie = await _repository.GetMovieById(id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = updateDto.Title;
            movie.Description = updateDto.Description;
            movie.AiringDate = updateDto.airingDate;

            await _repository.UpdateMovie(movie);

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteMovie(id);

            return NoContent();
        }

        [HttpDelete("bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromBody] MoviesBulkDeleteRequestDto bulkDeleteDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _repository.DeleteMovies(bulkDeleteDto.Ids);

            return NoContent();
        }
    }
}
