using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class BoilerRepository : IBoilerRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public BoilerRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public Boiler InsertBoiler(Boiler boiler)
        {
            try
            {
                Boiler boilerDatabase = _applicationDb.Boiler.Add(boiler).Entity;
                _applicationDb.SaveChanges();

                return boilerDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public Boiler GetBoilerById(int id)
        {
            try
            {
                Boiler boiler = _applicationDb.Boiler.Where(el => el.Id == id).FirstOrDefault()!;

                return boiler;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<Boiler> GetBoilerByIndustry(string industryName)
        {
            try
            {
                List<Boiler> boiler = _applicationDb.Boiler.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return boiler;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IBoilerRepository
    {
        Boiler InsertBoiler(Boiler boiler);
        Boiler GetBoilerById(int id);
        List<Boiler> GetBoilerByIndustry(string industryName);
    }
}