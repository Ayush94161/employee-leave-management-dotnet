using System.ComponentModel.DataAnnotations;
using LeaveManagement.Api.Models;

namespace LeaveManagement.Api.DTOs;

public record LoginRequest([Required] string Username, [Required] string Password);
public record EmployeeRequest([Required, MaxLength(100)] string Name,
    [Required, EmailAddress] string Email, [Required] string Department, int AvailableLeaveDays = 20);
public record LeaveRequestCreate(int EmployeeId, DateOnly StartDate, DateOnly EndDate,
    [Required, MaxLength(500)] string Reason);
public record LeaveStatusUpdate(LeaveStatus Status);
