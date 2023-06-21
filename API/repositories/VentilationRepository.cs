using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class VentilationRepository : IVentilationRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public VentilationRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public Ventilation InsertVentilation(Ventilation ventilation)
        {
            try
            {
                Ventilation ventilationDatabase = _applicationDb.Ventilation.Add(ventilation).Entity;
                _applicationDb.SaveChanges();

                return ventilationDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public Ventilation GetVentilationById(int id)
        {
            try
            {
                Ventilation ventilation = _applicationDb.Ventilation.Where(el => el.Id == id).FirstOrDefault()!;

                return ventilation;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<Ventilation> GetVentilationByIndustry(string industryName)
        {
            try
            {
                List<Ventilation> ventilation = _applicationDb.Ventilation.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return ventilation;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IVentilationRepository
    {
        Ventilation InsertVentilation(Ventilation ventilation);
        Ventilation GetVentilationById(int id);
        List<Ventilation> GetVentilationByIndustry(string industryName);
    }
}