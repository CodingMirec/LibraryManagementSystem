using AutoMapper;
using LibraryManagementSystem.Core.DTOs;
using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>()
                .ReverseMap();

            CreateMap<Book, BookDTO>()
                .ReverseMap();

            CreateMap<Loan, LoanDTO>()
                .ReverseMap();
        }
    }
}
