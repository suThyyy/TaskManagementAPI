
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description
            })
            .ToListAsync();

        return Ok(categories);
    }

    // GET: api/categories/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description
            })
            .FirstOrDefaultAsync();

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    // POST: api/categories
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequest request)
    {
        bool nameExists = await _context.Categories
            .AnyAsync(c => c.Name == request.Name);

        if (nameExists)
            return Conflict("Tên danh mục đã tồn tại.");

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            new
            {
                category.Id,
                category.Name,
                category.Description
            }
        );
    }

    // PUT: api/categories/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCategoryRequest request)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return NotFound("Category không tồn tại.");

        bool nameExists = await _context.Categories
            .AnyAsync(c => c.Name == request.Name && c.Id != id);

        if (nameExists)
            return Conflict("Tên danh mục đã tồn tại.");

        category.Name = request.Name;
        category.Description = request.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/categories/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return NotFound("Category không tồn tại.");

        bool hasTasks = await _context.Todotasks
            .AnyAsync(t => t.CategoryId == id);

        if (hasTasks)
            return Conflict("Không thể xóa Category đang có TodoTask.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
