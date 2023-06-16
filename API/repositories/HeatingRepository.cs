using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class HeatingRepository : IHeatingRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public HeatingRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public Heating InsertHeating(Heating heating)
        {
            try
            {
                Heating heatingDatabase = _applicationDb.Heating.Add(heating).Entity;
                _applicationDb.SaveChanges();

                return heatingDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public Heating GetHeatingById(int id)
        {
            try
            {
                Heating heating = _applicationDb.Heating.Where(el => el.Id == id).FirstOrDefault()!;

                return heating;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<Heating> GetHeatingByIndustry(string industryName)
        {
            try
            {
                List<Heating> heating = _applicationDb.Heating.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return heating;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IHeatingRepository
    {
        Heating InsertHeating(Heating heating);
        Heating GetHeatingById(int id);
        List<Heating> GetHeatingByIndustry(string industryName);
    }
}