using Microsoft.EntityFrameworkCore;
using MovieAPI_dotnet.Data;
using MovieAPI_dotnet.Dtos;
using MovieAPI_dotnet.Dtos.Responses.Movies;
using MovieAPI_dotnet.Helpers;
using MovieAPI_dotnet.Models;
using static MovieAPI_dotnet.Dtos.Responses.Movies.MovieDetailResponseDto;

namespace MovieAPI_dotnet.Repositories.Movies
{
    public interface IMovieRepository
    {
        Task<PaginatedResponse<Movie>> GetMoviesAsync(IndexDto index);
        Task<Movie> GetMovieById(int id);
        Task AddMovie(Movie movie);
        Task UpdateMovie(Movie movie);
        Task DeleteMovie(int id);
        Task DeleteMovies(List<int> ids);
    }
    public class MovieRepository: IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<Movie>> GetMoviesAsync(IndexDto index)
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

            return response;
        }

        public async Task<Movie> GetMovieById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.UserMovies)
                .ThenInclude(um => um.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(Movie => Movie.Id == id);

            return movie;
        }

        public async Task AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovie(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMovies(List<int> ids)
        {
            var moviesToDelete = await _context.Movies
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();

            if (moviesToDelete != null && moviesToDelete.Count() != 0)
            {
                _context.Movies.RemoveRange(moviesToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
