using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class MotorRepository : IMotorRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public MotorRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public Motor InsertMotor(Motor motor)
        {
            try
            {
                Motor motorDatabase = _applicationDb.Motor.Add(motor).Entity;
                _applicationDb.SaveChanges();

                return motorDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public Motor GetMotorById(int id)
        {
            try
            {
                Motor motor = _applicationDb.Motor.Where(el => el.Id == id).FirstOrDefault()!;

                return motor;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<Motor> GetMotorByIndustry(string industryName)
        {
            try
            {
                List<Motor> motor = _applicationDb.Motor.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return motor;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IMotorRepository
    {
        Motor InsertMotor(Motor motor);
        Motor GetMotorById(int id);
        List<Motor> GetMotorByIndustry(string industryName);
    }
}