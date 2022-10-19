using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _unitOfWork.BookRepository.GetAll().ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(Guid BookId)
        {
            return await _unitOfWork.BookRepository.GetByIdAsync(BookId);
        }

        public async Task CreateBookAsync(Book book)
        {
            try
            {
                _unitOfWork.BookRepository.Add(book);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
        
        public async Task UpdateBookAsync(Book book)
        {
            try
            {
                _unitOfWork.BookRepository.Edit(book);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
        
        public async Task DeleteBookAsync(Book book)
        {
            try
            {
                _unitOfWork.BookRepository.Delete(book);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
