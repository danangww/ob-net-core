using BLL;
using book_api.DTO;
using book_api.Models;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace book_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;

        private readonly BookService _bookService;

        public BookController(ILogger<BookController> logger, IUnitOfWork uow)
        {
            _logger = logger;
            _bookService ??= new BookService(uow);
        }

        [HttpGet]
        [Route("")]
        public async Task<ApiResponse<List<Book>>> GetBooks()
        {
            return new ApiResponse<List<Book>>
            {
                Data = await _bookService.GetBooksAsync()
            };
        }

        [HttpGet]
        [Route("{BookId}")]
        public async Task<ApiResponse<Book>> GetBookById([FromRoute] Guid BookId)
        {
            return new ApiResponse<Book>
            {
                Data = await _bookService.GetBookByIdAsync(BookId)
            };
        }

        [HttpPost]
        [Route("")]
        public async Task<ApiResponse<Book>> CreateBook([FromBody] BookDTO bookDTO)
        {
            ApiResponse<Book> apiResponse = new ApiResponse<Book>();

            try
            {
                Book book = new Book()
                {
                    Id = Guid.NewGuid(),
                    Title = bookDTO.Title,
                    Description = bookDTO.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                await _bookService.CreateBookAsync(book);
                apiResponse.Message = "Book created successfully";
                apiResponse.Data = book;
            }
            catch (Exception ex)
            {
                apiResponse.Code = 500;
                apiResponse.Message = "Error: " + ex.Message;
            }

            return apiResponse;
        }

        [HttpPut]
        [Route("{BookId}")]
        public async Task<ApiResponse<Book>> UpdateBook([FromRoute] Guid BookId, [FromBody] BookDTO bookDTO)
        {
            ApiResponse<Book> apiResponse = new ApiResponse<Book>();
            Book? book = await _bookService.GetBookByIdAsync(BookId);

            if (book == null)
            {
                apiResponse.Message = "Book not found";
            }
            else
            {
                try
                {
                    book.Title = bookDTO.Title;
                    book.Description = bookDTO.Description;
                    book.UpdatedAt = DateTime.Now;

                    await _bookService.UpdateBookAsync(book);

                    apiResponse.Message = "Book updated successfully";
                    apiResponse.Data = book;
                }
                catch (Exception ex)
                {
                    apiResponse.Code = 500;
                    apiResponse.Message = ex.Message;
                }

            }

            return apiResponse;
        }

        [HttpDelete]
        [Route("{BookId}")]
        public async Task<ApiResponse<Book>> DeleteBook([FromRoute] Guid BookId)
        {
            ApiResponse<Book> apiResponse = new ApiResponse<Book>();
            Book? book = await _bookService.GetBookByIdAsync(BookId);

            if (book == null)
            {
                apiResponse.Message = "Book not found";
            }
            else
            {
                await _bookService.DeleteBookAsync(book);

                apiResponse.Message = "Book deleted successfully";
                apiResponse.Data = book;
            }

            return apiResponse;
        }
    }
}
