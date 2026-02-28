using Api.Models;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ITenantProvider _tenant;

    public InvoicesController(AppDbContext db, ITenantProvider tenant)
    {
        _db = db;
        _tenant = tenant;
    }

    
    [HttpGet]
    public IActionResult Get()
    {
        var list = _db.Invoices
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return Ok(list);
    }

   
    [HttpPost]
    public IActionResult Create([FromBody] InvoiceCreateDto dto)
    {
        var tenantId = _tenant.GetTenantId();
        if (tenantId == Guid.Empty)
            return Unauthorized(new { message = "Token sin tenantId válido" });

        var inv = new Invoice
        {
            TenantId = tenantId,
            Number = dto.Number,
            Total = dto.Total
        };

        _db.Invoices.Add(inv);
        _db.SaveChanges();

        return Ok(inv);
    }
}