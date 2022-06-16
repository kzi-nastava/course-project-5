﻿using HealthInstitution.Core.Exceptions;
using HealthInstitution.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Repository
{
    class AllergenRepositoryService : IAllergenRepositoryService
    {
        private readonly IAllergenRepository _allergenRepository;

        public AllergenRepositoryService()
        {
            _allergenRepository = Institution.Instance().AllergenRepository;
        }

        public Allergen FindByID(int id)
        {
            return _allergenRepository.FindByID(id);
        }

        public List<Allergen> PatientAllergenToAllergen(List<PatientAllergen> patientAllergens)
        {
            return _allergenRepository.PatientAllergenToAllergen(patientAllergens);
        }

        public List<Allergen> MedicineAllergenToAllergen(List<MedicineAllergen> medicineAllergens)
        {
            return _allergenRepository.MedicineAllergenToAllergen(medicineAllergens);
        }

        public void ChangeName(Allergen allergen, string name)
        {
            AllergenService service = new AllergenService(allergen);
            if (!service.IsNameAvailable(name)) throw new NameNotAvailableException("Name already taken");
            _allergenRepository.ChangeName(allergen, name);

        }

        public Allergen AddNewAllergen(Allergen allergen)
        {
            AllergenService service = new AllergenService(allergen);
            if (!service.IsNameAvailable(allergen.Name)) throw new NameNotAvailableException("Name already in use!");

            return _allergenRepository.AddNewAllergen(allergen);
        }

        public void DeleteAllergen(Allergen allergen)
        {
            AllergenService service = new AllergenService(allergen);
            if (!service.IsNameAvailable(allergen.Name)) throw new NameNotAvailableException("Name already in use!");

            _allergenRepository.DeleteAllergen(allergen);
        }

        public List<Allergen> GetAllergens()
        {
            return _allergenRepository.GetAllergens();
        }
    }
}
