using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class MechanicalTreatmentRepository : IMechanicalTreatmentRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public MechanicalTreatmentRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public MechanicalTreatment InsertMechanicalTreatment(MechanicalTreatment mechanicalTreatment)
        {
            try
            {
                MechanicalTreatment mechanicalTreatmentDatabase = _applicationDb.MechanicalTreatment.Add(mechanicalTreatment).Entity;
                _applicationDb.SaveChanges();

                return mechanicalTreatmentDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public MechanicalTreatment GetMechanicalTreatmentById(int id)
        {
            try
            {
                MechanicalTreatment mechanicalTreatment = _applicationDb.MechanicalTreatment.Where(el => el.Id == id).FirstOrDefault()!;

                return mechanicalTreatment;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<MechanicalTreatment> GetMechanicalTreatmentByIndustry(string industryName)
        {
            try
            {
                List<MechanicalTreatment> mechanicalTreatment = _applicationDb.MechanicalTreatment.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return mechanicalTreatment;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IMechanicalTreatmentRepository
    {
        MechanicalTreatment InsertMechanicalTreatment(MechanicalTreatment mechanicalTreatment);
        MechanicalTreatment GetMechanicalTreatmentById(int id);
        List<MechanicalTreatment> GetMechanicalTreatmentByIndustry(string industryName);
    }
}