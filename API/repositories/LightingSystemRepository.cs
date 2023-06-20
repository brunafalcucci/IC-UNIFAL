using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class LightingSystemRepository : ILightingSystemRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public LightingSystemRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public LightingSystem InsertLightingSystem(LightingSystem lightingSystem)
        {
            try
            {
                LightingSystem lightingSystemDatabase = _applicationDb.LightingSystem.Add(lightingSystem).Entity;
                _applicationDb.SaveChanges();

                return lightingSystemDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public LightingSystem GetLightingSystemById(int id)
        {
            try
            {
                LightingSystem lightingSystem = _applicationDb.LightingSystem.Where(el => el.Id == id).FirstOrDefault()!;

                return lightingSystem;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<LightingSystem> GetLightingSystemByIndustry(string industryName)
        {
            try
            {
                List<LightingSystem> lightingSystem = _applicationDb.LightingSystem.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return lightingSystem;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface ILightingSystemRepository
    {
        LightingSystem InsertLightingSystem(LightingSystem lightingSystem);
        LightingSystem GetLightingSystemById(int id);
        List<LightingSystem> GetLightingSystemByIndustry(string industryName);
    }
}