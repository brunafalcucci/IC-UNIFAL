using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class CriticalityIndexRepository : ICriticalityIndexRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public CriticalityIndexRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public CriticalityIndex InsertCriticalityIndex(CriticalityIndex criticalityIndex)
        {
            try
            {
                CriticalityIndex criticalityIndexDatabase = _applicationDb.CriticalityIndex.Add(criticalityIndex).Entity;
                _applicationDb.SaveChanges();

                return criticalityIndexDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public CriticalityIndex GetCriticalityIndexById(int id)
        {
            try
            {
                CriticalityIndex criticalityIndex = _applicationDb.CriticalityIndex.Where(el => el.Id == id).FirstOrDefault()!;

                return criticalityIndex;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<CriticalityIndex> GetCriticalityIndexByIndustry(string industryName)
        {
            try
            {
                List<CriticalityIndex> criticalityIndex = _applicationDb.CriticalityIndex.Where(el => el.IndustryName == industryName).OrderByAsc(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return criticalityIndex;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface ICriticalityIndexRepository
    {
        bool InsertCriticalityIndex(CriticalityIndex criticalityIndex);
        CriticalityIndex GetCriticalityIndexById(int id);
        List<CriticalityIndex> GetCriticalityIndexByIndustry(string industryName);
    }
}