using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;
using LibraryManagementSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoansService _loanService;

        public LoansController(ILoansService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanResponseDto>>> GetAllLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            return Ok(loans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanResponseDto>> GetLoanById(int id)
        {
            try
            {
                var loan = await _loanService.GetLoanByIdAsync(id);
                return Ok(loan);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<LoanResponseDto>> CreateLoan([FromBody] LoanRequestDto loanDto)
        {
            if (loanDto == null)
            {
                return BadRequest("Loan data is invalid.");
            }

            try
            {
                var createdLoan = await _loanService.AddLoanAsync(loanDto);
                return CreatedAtAction(nameof(GetLoanById), new { id = createdLoan.Id }, createdLoan);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoanRequestDto loanDto)
        {
            if (loanDto == null)
            {
                return BadRequest("Loan data is invalid.");
            }

            try
            {
                await _loanService.UpdateLoanAsync(id, loanDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                await _loanService.DeleteLoanAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
