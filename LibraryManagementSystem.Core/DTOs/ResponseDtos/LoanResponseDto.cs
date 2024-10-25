using LibraryManagementSystem.Core.DTOs.RequestDtos;

namespace LibraryManagementSystem.Core.DTOs.ResponseDtos
{
    public class LoanResponseDto : LoanRequestDto
    {
        public int Id { get; set; }
    }
}
