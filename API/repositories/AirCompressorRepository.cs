using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class AirCompressorRepository : IAirCompressorRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public AirCompressorRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public AirCompressor InsertAirCompressor(AirCompressor airCompressor)
        {
            try
            {
                AirCompressor airCompressorDatabase = _applicationDb.AirCompressor.Add(airCompressor).Entity;
                _applicationDb.SaveChanges();

                return airCompressorDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public AirCompressor GetAirCompressorById(int id)
        {
            try
            {
                AirCompressor airCompressor = _applicationDb.AirCompressor.Where(el => el.Id == id).FirstOrDefault()!;

                return airCompressor;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<AirCompressor> GetAirCompressorByIndustry(string industryName)
        {
            try
            {
                List<AirCompressor> airCompressor = _applicationDb.AirCompressor.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return airCompressor;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IAirCompressorRepository
    {
        AirCompressor InsertAirCompressor(AirCompressor airCompressor);
        AirCompressor GetAirCompressorById(int id);
        List<AirCompressor> GetAirCompressorByIndustry(string industryName);
    }
}