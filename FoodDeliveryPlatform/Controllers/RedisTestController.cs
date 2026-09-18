using FDP.Interface;
using FDP.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace FDP.Controller;

[ApiController]
[Route("api/[controller]")]
public class RedisTestController : ControllerBase
{
    private readonly IRedisService _redisService;
    public RedisTestController(IRedisService redisService)
    {
        _redisService=redisService;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        //await _database.StringSetAsync("fdp:test2","Hello again");
        await _redisService.SetAsync("fdp:test","I am using redis",TimeSpan.FromSeconds(20));

        var result=await _redisService.GetAsync<string>("fdp:test");

        return Ok(result?.ToString());
    }
}

