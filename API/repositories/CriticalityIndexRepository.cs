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

        public bool InsertCriticalityIndex(CriticalityIndex criticalityIndex)
        {
            try
            {
                _applicationDb.CriticalityIndex.Add(criticalityIndex);
                _applicationDb.SaveChanges();

                return true;
            } catch (Exception)
            {
                throw;
            }
        }
        
        public bool DeleteCriticalityIndex(int id)
        {
            try
            {
                _applicationDb.CriticalityIndex.Remove(GetCriticalityIndexById(id));
                _applicationDb.SaveChanges();

                return true;
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

        public List<CriticalityIndex> GetCriticalityIndexByCity(string city)
        {
            try
            {
                List<CriticalityIndex> criticalityIndex = _applicationDb.CriticalityIndex.Where(el => el.City == city).Take(5).ToList()!;

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
        bool DeleteCriticalityIndex(int id);
        CriticalityIndex GetCriticalityIndexById(int id);
        List<CriticalityIndex> GetCriticalityIndexByCity(string city);
    }
}