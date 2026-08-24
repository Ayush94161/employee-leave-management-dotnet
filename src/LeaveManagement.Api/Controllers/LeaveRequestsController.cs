using LeaveManagement.Api.Data;
using LeaveManagement.Api.DTOs;
using LeaveManagement.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Api.Controllers;

[ApiController, Route("api/leave-requests")]
public class LeaveRequestsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] LeaveStatus? status) => Ok(await db.LeaveRequests
        .Include(x => x.Employee).Where(x => status == null || x.Status == status).AsNoTracking().ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(LeaveRequestCreate request)
    {
        if (request.EndDate < request.StartDate) return BadRequest(new { message = "End date must be on or after start date" });
        var employee = await db.Employees.FindAsync(request.EmployeeId);
        if (employee is null) return NotFound(new { message = "Employee not found" });
        var days = request.EndDate.DayNumber - request.StartDate.DayNumber + 1;
        if (days > employee.AvailableLeaveDays) return BadRequest(new { message = "Insufficient leave balance" });
        var leave = new LeaveRequest { EmployeeId = request.EmployeeId, StartDate = request.StartDate, EndDate = request.EndDate, Reason = request.Reason };
        db.LeaveRequests.Add(leave);
        await db.SaveChangesAsync();
        return Created($"/api/leave-requests/{leave.Id}", leave);
    }

    [Authorize(Roles = "Admin"), HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, LeaveStatusUpdate request)
    {
        if (request.Status == LeaveStatus.Pending) return BadRequest(new { message = "Select Approved or Rejected" });
        var leave = await db.LeaveRequests.Include(x => x.Employee).FirstOrDefaultAsync(x => x.Id == id);
        if (leave is null) return NotFound();
        if (leave.Status != LeaveStatus.Pending) return Conflict(new { message = "Request is already processed" });
        if (request.Status == LeaveStatus.Approved)
        {
            var days = leave.EndDate.DayNumber - leave.StartDate.DayNumber + 1;
            if (leave.Employee!.AvailableLeaveDays < days) return BadRequest(new { message = "Insufficient leave balance" });
            leave.Employee.AvailableLeaveDays -= days;
        }
        leave.Status = request.Status;
        await db.SaveChangesAsync();
        return Ok(leave);
    }
}
