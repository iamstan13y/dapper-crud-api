using Dapper;
using DapperCrud.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SuperHeroController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public SuperHeroController(IConfiguration configuration) => _configuration = configuration;

    [HttpGet]
    public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var heroes = await connection.QueryAsync<SuperHero>("SELECT * FROM SuperHeroes");

        return Ok(heroes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SuperHero>> GetSuperHero(int id)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var hero = await connection.QueryFirstAsync<SuperHero>("SELECT * FROM SuperHeroes WHERE Id=@Id", new {Id = id});

        return Ok(hero);
    }

    [HttpPost]
    public async Task<ActionResult<List<SuperHero>>> CreateSuperHero(SuperHero hero)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("INSERT INTO SuperHeroes (Name, FirstName, LastName, Place) Values(@Name, @FirstName, @LastName, @Place)", hero);

        return Ok(await SelectAllHereos(connection));
    }

    [HttpPut]
    public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("UPDATE SuperHeroes SET Name=@Name, FirstName=@FirstName, LastName=@LastName, Place=@Place WHERE Id=@Id", hero);

        return Ok(await SelectAllHereos(connection));
    }

    [HttpDelete]
    public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("DELETE FROM SuperHeroes WHERE Id=@Id", new {Id = id});

        return Ok(await SelectAllHereos(connection));
    }

    private static async Task<IEnumerable<SuperHero>> SelectAllHereos(SqlConnection connection)
    {
        return await connection.QueryAsync<SuperHero>("SELECT * FROM SuperHeroes");
    }
}