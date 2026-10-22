
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users
            .AsNoTracking()
            .Select(u => new
            {
                u.Id,
                u.Fullname,
                u.Email,
                u.CreatedAt
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/users/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new
            {
                u.Id,
                u.Fullname,
                u.Email,
                u.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request)
    {
        bool emailExists = await _context.Users
            .AnyAsync(u => u.Email == request.Email);

        if (emailExists)
            return Conflict("Email đã tồn tại.");

        var user = new User
        {
            Fullname = request.Fullname,
            Email = request.Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            new
            {
                user.Id,
                user.Fullname,
                user.Email,
                user.CreatedAt
            }
        );
    }

    //PUT: api/users/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateUserRequest request)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound("User không tồn tại.");

        bool emailExists = await _context.Users
            .AnyAsync(u => u.Email == request.Email && u.Id != id);

        if (emailExists)
            return Conflict("Email đã được sử dụng.");

        user.Fullname = request.Fullname;
        user.Email = request.Email;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/users/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound("User không tồn tại.");

        bool hasTasks = await _context.Todotasks
            .AnyAsync(t => t.UserId == id);

        if (hasTasks)
            return Conflict("Không thể xóa User đang có TodoTask.");

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return NoContent();
    }


}
