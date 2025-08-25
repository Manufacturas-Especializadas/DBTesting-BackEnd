using DBTesting.Dtos;
using DBTesting.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
      
        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees
                                        .Select(e => new
                                        {
                                            e.Id,
                                            e.FullName,
                                            e.IdDepartamentNavigation.Name,
                                            e.PhoneNumber,
                                            e.Address,
                                            e.Email
                                        })
                                        .AsNoTracking()
                                        .ToListAsync();

            return Ok(employees);
        }

        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _context.Employees
                                        .Where(e => e.Id == id)
                                        .Select(e => new
                                        {
                                            e.FullName,
                                            e.IdDepartamentNavigation.Name,
                                            e.PhoneNumber,
                                            e.Address,
                                            e.Email,
                                            e.IdDepartament
                                        })
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            return Ok(employee);
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
        {
            if(employeeDto == null)
            {
                return BadRequest(new { message = "Employee data is null" });
            }

            try
            {
                var employee = new Employees
                {
                    FullName = employeeDto.FullName,
                    Address = employeeDto.Address,
                    Email = employeeDto.Email,
                    PhoneNumber = employeeDto.PhoneNumber!,
                    IdDepartament = employeeDto.IdDepartament
                };

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Employee added successfully", employeeId = employee.Id });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpPatch]
        [Route("UpdateEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest(new { message = "Employee data is null" });
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            employee.FullName = employeeDto.FullName;
            employee.Address = employeeDto.Address;
            employee.Email = employeeDto.Email;
            employee.PhoneNumber = employeeDto.PhoneNumber!;
            employee.IdDepartament = employeeDto.IdDepartament;

            try
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Employee updated successfully", employee });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = new Employees { Id = id };

            _context.Entry(employee).State = EntityState.Deleted;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }

    }
}