﻿using System.Collections.Generic;
using HealthInstitution.MVVM.Models.Entities;
using HealthInstitution.MVVM.Models.Services;

namespace HealthInstitution.Repositories
{
    public class DoctorRepository
    {
        private readonly string _fileName;
        private List<Doctor> _doctors;

        public List<Doctor> Doctors { get => _doctors; }
        public DoctorRepository(string fileName)
        {
            _fileName = fileName;
            _doctors = new List<Doctor>();
        }

        public void LoadFromFile()
        {
            _doctors = FileService.Deserialize<Doctor>(_fileName);
        }

        public void SaveToFile()
        {
            FileService.Serialize<Doctor>(_fileName, _doctors);
        }
        public Doctor FindByID(int id)
        {
            foreach(Doctor doctor in _doctors)
            {
                if (doctor.ID == id) return doctor;
            }
            return null;
        }
        public List<Doctor> GetGeneralPractitioners()
        {
            List<Doctor> generalPractitioners = new();

            foreach (Doctor doctor in _doctors)
            {
                if (doctor.Specialization == Specialization.NONE) generalPractitioners.Add(doctor);
            }

            return generalPractitioners;
        }

        public Doctor FindDoctorBySpecialization(Specialization specialization)
        {
            foreach (Doctor doctor in _doctors)
            {
                if (doctor.Specialization == specialization) return doctor;
            }
            return null;
        }
    }
}
