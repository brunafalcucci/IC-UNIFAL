using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class CoolingSystemRepository : ICoolingSystemRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public CoolingSystemRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public CoolingSystem InsertCoolingSystem(CoolingSystem coolingSystem)
        {
            try
            {
                CoolingSystem coolingSystemDatabase = _applicationDb.CoolingSystem.Add(coolingSystem).Entity;
                _applicationDb.SaveChanges();

                return coolingSystemDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public CoolingSystem GetCoolingSystemById(int id)
        {
            try
            {
                CoolingSystem coolingSystem = _applicationDb.CoolingSystem.Where(el => el.Id == id).FirstOrDefault()!;

                return coolingSystem;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<CoolingSystem> GetCoolingSystemByIndustry(string industryName)
        {
            try
            {
                List<CoolingSystem> coolingSystem = _applicationDb.CoolingSystem.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return coolingSystem;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface ICoolingSystemRepository
    {
        CoolingSystem InsertCoolingSystem(CoolingSystem coolingSystem);
        CoolingSystem GetCoolingSystemById(int id);
        List<CoolingSystem> GetCoolingSystemByIndustry(string industryName);
    }
}