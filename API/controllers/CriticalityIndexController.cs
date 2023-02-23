using Application.Models;
using Application.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class CriticalityIndexController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly ICriticalityIndexRepository _CriticalityIndexRepository;
        public CriticalityIndexController(IConfiguration configuration, ICriticalityIndexRepository CriticalityIndexRepository)
        {
            Configuration = configuration;
            _CriticalityIndexRepository = CriticalityIndexRepository;
        }

        [HttpPost]
        public ActionResult<bool> InsertCriticalityIndex([FromBody] CriticalityIndex criticalityIndex)
        {
            try
            {
                return _CriticalityIndexRepository.InsertCriticalityIndex(criticalityIndex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteCriticalityIndex(int id)
        {
            try
            {
                return _CriticalityIndexRepository.DeleteCriticalityIndex(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CriticalityIndex> GetCriticalityIndexById(int id)
        {
            try
            {
                return _CriticalityIndexRepository.GetCriticalityIndexById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{city}")]
        public ActionResult<List<CriticalityIndex>> GetCriticalityIndexByCity(string city)
        {
            try
            {
                return _CriticalityIndexRepository.GetCriticalityIndexByCity(city);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}