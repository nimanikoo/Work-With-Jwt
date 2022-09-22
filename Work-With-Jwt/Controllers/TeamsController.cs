using Authentication_with_Jwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Work_With_Jwt.Data;
using Work_With_Jwt.Services.Interfaces;

namespace Authentication_with_Jwt.Controllers;

[Route("api/[controller]")] // api/teams
[ApiController]
public class TeamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeamsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet, Authorize(Roles = "Manager")]
    public async Task<IActionResult> Get()
    {
        var teams = await _context.Teams.ToListAsync();
        return Ok(teams);
    }

    [HttpGet("{id:int}"), Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

        if (team == null)
            return BadRequest("Invalid Id");

        return Ok(team);
    }

    [HttpPost, Authorize(Roles = "Manager")]
    public async Task<IActionResult> Post(Team team)
    {
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        return CreatedAtAction("Get", team.Id, team);
    }

    [HttpPatch, Authorize(Roles = "Manager")]
    public async Task<IActionResult> Patch(int id, string country)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

        if (team == null)
            return BadRequest("Invalid id");

        team.Country = country;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete, Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

        if (team == null)
            return BadRequest("Invalid id");

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}