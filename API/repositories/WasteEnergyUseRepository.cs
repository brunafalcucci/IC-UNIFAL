using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class WasteEnergyUseRepository : IWasteEnergyUseRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public WasteEnergyUseRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public WasteEnergyUse InsertWasteEnergyUse(WasteEnergyUse wasteEnergyUse)
        {
            try
            {
                WasteEnergyUse wasteEnergyUseDatabase = _applicationDb.WasteEnergyUse.Add(wasteEnergyUse).Entity;
                _applicationDb.SaveChanges();

                return wasteEnergyUseDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public WasteEnergyUse GetWasteEnergyUseById(int id)
        {
            try
            {
                WasteEnergyUse wasteEnergyUse = _applicationDb.WasteEnergyUse.Where(el => el.Id == id).FirstOrDefault()!;

                return wasteEnergyUse;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<WasteEnergyUse> GetWasteEnergyUseByIndustry(string industryName)
        {
            try
            {
                List<WasteEnergyUse> wasteEnergyUse = _applicationDb.WasteEnergyUse.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return wasteEnergyUse;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IWasteEnergyUseRepository
    {
        WasteEnergyUse InsertWasteEnergyUse(WasteEnergyUse wasteEnergyUse);
        WasteEnergyUse GetWasteEnergyUseById(int id);
        List<WasteEnergyUse> GetWasteEnergyUseByIndustry(string industryName);
    }
}