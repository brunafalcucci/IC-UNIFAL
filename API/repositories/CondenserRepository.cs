using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class CondenserRepository : ICondenserRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public CondenserRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public Condenser InsertCondenser(Condenser condenser)
        {
            try
            {
                Condenser condenserDatabase = _applicationDb.Condenser.Add(condenser).Entity;
                _applicationDb.SaveChanges();

                return condenserDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public Condenser GetCondenserById(int id)
        {
            try
            {
                Condenser condenser = _applicationDb.Condenser.Where(el => el.Id == id).FirstOrDefault()!;

                return condenser;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<Condenser> GetCondenserByIndustry(string industryName)
        {
            try
            {
                List<Condenser> condenser = _applicationDb.Condenser.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return condenser;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface ICondenserRepository
    {
        Condenser InsertCondenser(Condenser condenser);
        Condenser GetCondenserById(int id);
        List<Condenser> GetCondenserByIndustry(string industryName);
    }
}