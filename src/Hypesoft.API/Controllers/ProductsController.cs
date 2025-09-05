using Hypesoft.Application.DTOs;
using Hypesoft.Application.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hypesoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductResponse>>> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? name = null,
        [FromQuery] string? categoryId = null)
        => await _mediator.Send(new ListProductsQuery(page, pageSize, name, categoryId));

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetById(string id)
        => await _mediator.Send(new GetProductByIdQuery(id));

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreateProductRequest body)
    {
        var id = await _mediator.Send(new CreateProductCommand(body));
        return Ok(new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductRequest body)
    {
        // força o id do caminho no DTO
        var fixedBody = body with { Id = id };
        var ok = await _mediator.Send(new UpdateProductCommand(id, fixedBody));
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ok = await _mediator.Send(new DeleteProductCommand(id));
        return ok ? NoContent() : NotFound();
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<ProductsDashboardResult>> Dashboard()
        => await _mediator.Send(new GetProductsDashboardQuery());
}

