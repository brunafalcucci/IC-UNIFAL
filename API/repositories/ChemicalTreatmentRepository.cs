using Application.Context;
using Application.Models;

namespace Application.Repositories
{
    public class ChemicalTreatmentRepository : IChemicalTreatmentRepository
    {
        private readonly ApplicationDbContext _applicationDb;

        public ChemicalTreatmentRepository(ApplicationDbContext context)
        {
            _applicationDb = context;
        }

        public ChemicalTreatment InsertChemicalTreatment(ChemicalTreatment chemicalTreatment)
        {
            try
            {
                ChemicalTreatment chemicalTreatmentDatabase = _applicationDb.ChemicalTreatment.Add(chemicalTreatment).Entity;
                _applicationDb.SaveChanges();

                return chemicalTreatmentDatabase;
            } catch (Exception)
            {
                throw;
            }
        }

        public ChemicalTreatment GetChemicalTreatmentById(int id)
        {
            try
            {
                ChemicalTreatment chemicalTreatment = _applicationDb.ChemicalTreatment.Where(el => el.Id == id).FirstOrDefault()!;

                return chemicalTreatment;
            } catch (Exception)
            {
                throw;
            }
        }

        public List<ChemicalTreatment> GetChemicalTreatmentByIndustry(string industryName)
        {
            try
            {
                List<ChemicalTreatment> chemicalTreatment = _applicationDb.ChemicalTreatment.Where(el => el.IndustryName == industryName).OrderByDescending(el => el.Ultima_Atualizacao).Take(5).ToList()!;

                return chemicalTreatment;
            } catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IChemicalTreatmentRepository
    {
        ChemicalTreatment InsertChemicalTreatment(ChemicalTreatment chemicalTreatment);
        ChemicalTreatment GetChemicalTreatmentById(int id);
        List<ChemicalTreatment> GetChemicalTreatmentByIndustry(string industryName);
    }
}