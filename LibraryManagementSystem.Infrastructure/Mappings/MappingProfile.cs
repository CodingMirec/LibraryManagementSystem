using AutoMapper;
using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;
using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRequestDto>()
                .ReverseMap();

            CreateMap<User, UserResponseDto>()
                .ReverseMap();

            CreateMap<Loan, LoanRequestDto>()
                .ReverseMap();

            CreateMap<Loan, LoanResponseDto>()
                .ReverseMap();

            CreateMap<Book, BookRequestDto>()
                .ReverseMap();

            CreateMap<Book, BookResponseDto>()
                .ReverseMap();

            CreateMap<Loan, Loan>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 

        }
    }
}
