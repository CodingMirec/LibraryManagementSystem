using LibraryManagementSystem.Core.DTOs.RequestDtos;

namespace LibraryManagementSystem.Core.DTOs.ResponseDtos
{
    public class BookResponseDto : BookRequestDto
    {
        public int Id { get; set; }
    }
}
