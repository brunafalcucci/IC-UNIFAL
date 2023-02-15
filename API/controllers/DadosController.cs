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
    public class DadosController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IDadosRepository _DadosRepository;
        public DadosController(IConfiguration configuration, IDadosRepository dadosRepository)
        {
            Configuration = configuration;
            _DadosRepository = dadosRepository;
        }

        [HttpPost]
        public ActionResult<bool> InsertDados([FromBody] Dados dados)
        {
            try
            {
                return _DadosRepository.InsertDados(dados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public ActionResult<bool> UpdateDados([FromBody] Dados dados)
        {
            try
            {                
                return _DadosRepository.UpdateDados(dados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteDados(int id)
        {
            try
            {
                return _DadosRepository.DeleteDados(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Dados> GetDadosById(int id)
        {
            try
            {
                return _DadosRepository.GetDadosById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{city}")]
        public ActionResult<List<Dados>> GetDadosByCity(string city)
        {
            try
            {
                return _DadosRepository.GetDadosByCity(city);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}