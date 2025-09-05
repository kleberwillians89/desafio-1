using Hypesoft.Application.Categories;
using Hypesoft.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hypesoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator) => _mediator = mediator;

    // GET /api/categories?page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PagedResult<CategoryResponse>>> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? name = null)
        => await _mediator.Send(new ListCategoriesQuery(page, pageSize, name));

    // GET /api/categories/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetById(string id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateCategoryRequest body)
    {
        var id = await _mediator.Send(new CreateCategoryCommand(body));
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] UpdateCategoryRequest body)
    {
        var ok = await _mediator.Send(new UpdateCategoryCommand(id, body));
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var ok = await _mediator.Send(new DeleteCategoryCommand(id));
        return ok ? NoContent() : NotFound();
    }
}
