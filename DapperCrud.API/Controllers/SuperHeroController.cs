using Microsoft.AspNetCore.Mvc;

namespace DapperCrud.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SuperHeroController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public SuperHeroController(IConfiguration configuration) => _configuration = configuration;
}