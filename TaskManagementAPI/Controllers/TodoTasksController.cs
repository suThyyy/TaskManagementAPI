
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoTasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodoTasksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/todotasks
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _context.Todotasks
            .AsNoTracking()
            .Select(t => new
            {
                t.Id,
                t.UserId,
                t.CategoryId,
                t.Title,
                t.Description,
                t.Completed,
                t.DueDate,
                t.CreatedAt
            })
            .ToListAsync();

        return Ok(tasks);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var todo = await _context.Todotasks
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new
            {
                t.Id,
                t.UserId,
                t.CategoryId,
                t.Title,
                t.Description,
                t.Completed,
                t.DueDate,
                t.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (todo == null)
            return NotFound();

        return Ok(todo);
    }



    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTodoTaskRequest request)
    {
        // Kiểm tra User có tồn tại không
        bool userExists = await _context.Users
            .AnyAsync(u => u.Id == request.UserId);

        if (!userExists)
            return BadRequest("User không tồn tại.");

        // Kiểm tra Category có tồn tại không
        bool categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId);

        if (!categoryExists)
            return BadRequest("Category không tồn tại.");

        // Tạo Entity
        var todo = new Todotask
        {
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            Title = request.Title,
            Description = request.Description,
            Completed = false,
            DueDate = request.DueDate
        };

        // Thêm vào DbContext
        _context.Todotasks.Add(todo);

        // Lưu xuống SQL Server
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = todo.Id },
            new
            {
                todo.Id,
                todo.Title,
                todo.Description,
                todo.Completed,
                todo.UserId,
                todo.CategoryId,
                todo.DueDate,
                todo.CreatedAt
            }
        );
    }


    // PUT: api/todotasks/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateTodoTaskRequest request)
    {
        // 1. Tìm TodoTask
        var todo = await _context.Todotasks.FindAsync(id);

        if (todo == null)
            return NotFound("TodoTask không tồn tại.");

        // 2. Kiểm tra User
        bool userExists = await _context.Users
            .AnyAsync(u => u.Id == request.UserId);

        if (!userExists)
            return BadRequest("User không tồn tại.");

        // 3. Kiểm tra Category
        bool categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId);

        if (!categoryExists)
            return BadRequest("Category không tồn tại.");

        // 4. Cập nhật dữ liệu
        todo.UserId = request.UserId;
        todo.CategoryId = request.CategoryId;
        todo.Title = request.Title;
        todo.Description = request.Description;
        todo.DueDate = request.DueDate;

        // 5. Lưu xuống SQL Server
        await _context.SaveChangesAsync();

        return NoContent();
    }


    // PATCH: api/todotasks/1/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateTodoTaskStatusRequest request)
    {
        // 1. Tìm TodoTask
        var todo = await _context.Todotasks.FindAsync(id);

        if (todo == null)
            return NotFound("TodoTask không tồn tại.");

        // 2. Chỉ cập nhật Completed
        todo.Completed = request.Completed!.Value;

        // 3. Lưu thay đổi
        await _context.SaveChangesAsync();

        return NoContent();
    }



    // DELETE: api/todotasks/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        // 1. Tìm TodoTask
        var todo = await _context.Todotasks.FindAsync(id);

        if (todo == null)
            return NotFound("TodoTask không tồn tại.");

        // 2. Đánh dấu Entity cần xóa
        _context.Todotasks.Remove(todo);

        // 3. Xóa trong SQL Server
        await _context.SaveChangesAsync();

        return NoContent();
    }



}
