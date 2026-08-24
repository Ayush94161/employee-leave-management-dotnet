using LeaveManagement.Api.Data;
using LeaveManagement.Api.DTOs;
using LeaveManagement.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class EmployeesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await db.Employees.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var employee = await db.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return employee is null ? NotFound() : Ok(employee);
    }

    [Authorize(Roles = "Admin"), HttpPost]
    public async Task<IActionResult> Create(EmployeeRequest request)
    {
        if (await db.Employees.AnyAsync(x => x.Email == request.Email)) return Conflict(new { message = "Email already exists" });
        var employee = new Employee { Name = request.Name, Email = request.Email, Department = request.Department, AvailableLeaveDays = request.AvailableLeaveDays };
        db.Employees.Add(employee);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }
}
