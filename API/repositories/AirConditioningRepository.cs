using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class AirConditioningRepository : IAirConditioningRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public AirConditioningRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public AirConditioning InsertAirConditioning(AirConditioning airConditioning)
        {
            try
            {
                AirConditioning airConditioningDatabase = _applicationDb.AirConditioning.Add(airConditioning).Entity;
                _applicationDb.SaveChanges();

                return airConditioningDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public AirConditioning GetAirConditioningById(int id)
        {
            try
            {
                AirConditioning airConditioning = _applicationDb.AirConditioning.Where(el => el.Id == id).FirstOrDefault()!;

                return airConditioning;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<AirConditioning> GetAirConditioningByIndustry(string industryName)
        {
            try
            {
                List<AirConditioning> airConditioning = _applicationDb.AirConditioning.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return airConditioning;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IAirConditioningRepository
    {
        AirConditioning InsertAirConditioning(AirConditioning airConditioning);
        AirConditioning GetAirConditioningById(int id);
        List<AirConditioning> GetAirConditioningByIndustry(string industryName);
    }
}