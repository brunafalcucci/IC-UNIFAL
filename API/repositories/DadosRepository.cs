using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class DadosRepository : IDadosRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public DadosRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public bool InsertDados(Dados dados)
        {
            try
            {
                _applicationDb.Dados.Add(dados);
                _applicationDb.SaveChanges();

                return true;
            } catch (Exception)
            {
                throw;
            }
        }
        
        public bool UpdateDados(Dados dados)
        {
            try
            {
                Dados? dados1 = _applicationDb.Dados.Where(el => el.Id == dados.Id).FirstOrDefault();
                if (dados1 != null)
                {
                    dados1.City = dados.City;
                    dados1.Dado1 = dados.Dado1;
                    dados1.Dado2 = dados.Dado2;
                    dados1.Dado3 = dados.Dado3;
                    dados1.Dado4 = dados.Dado4;
                    dados1.Dado5 = dados.Dado5;
                    dados1.Ultima_atualizacao = DateTime.Now;
                }
                _applicationDb.SaveChanges();

                return true;
            } catch (Exception)
            {
                throw;
            }
        }
        
        public bool DeleteDados(int id)
        {
            try
            {
                _applicationDb.Dados.Remove(GetDadosById(id));
                _applicationDb.SaveChanges();

                return true;
            } catch (Exception)
            {
                throw;
            }
        }

        public Dados GetDadosById(int id)
        {
            try
            {
                Dados dados = _applicationDb.Dados.Where(el => el.Id == id).FirstOrDefault()!;

                return dados;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<Dados> GetDadosByCity(string city)
        {
            try
            {
                List<Dados> dados = _applicationDb.Dados.Where(el => el.City == city).Take(5).ToList()!;

                return dados;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IDadosRepository
    {
        bool InsertDados(Dados dados);
        bool UpdateDados(Dados dados);
        bool DeleteDados(int id);
        Dados GetDadosById(int id);
        List<Dados> GetDadosByCity(string city);
    }
}