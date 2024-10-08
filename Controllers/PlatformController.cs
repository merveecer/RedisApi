using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using RedisApi.Data;
using RedisApi.Models;

namespace RedisApi.Controllers
{

    [Route("api/v1")]
    [ApiController]
    public class PlatformController : ControllerBase
    {

        private readonly IPlatformRepo _platformRepo;
        public PlatformController(IPlatformRepo platformRepo)
        {
            _platformRepo = platformRepo;
        }

        [HttpGet("Id",Name ="GetPlatformById")]
        public IActionResult GetPlatformById(string Id)
        {
            var platform = _platformRepo.GetPlatformById(Id);
            if (platform == null)
            {
                return NotFound();
            }
            return Ok(platform);
        }
        [HttpGet]
        public IActionResult GetAllPlatforms()
        {
            IEnumerable<Platform?>? allPlat = _platformRepo.GetAllPlatforms();
        
            return Ok(allPlat);
        }
        [HttpPost]
        public IActionResult CreatePlatform(Platform plat)
        {
            _platformRepo.CreatePlatform(plat);
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = plat.Id }, plat);
        }

    }

}