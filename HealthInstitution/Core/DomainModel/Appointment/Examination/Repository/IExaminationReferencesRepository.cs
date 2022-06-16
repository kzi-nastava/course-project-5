﻿namespace HealthInstitution.Core
{
    public interface IExaminationReferencesRepository
    {
        public ExaminationReference FindByExaminationID(int id);

        public void Add(Examination examination);

        public void Add(ExaminationReference examinationReference);

        public void Remove(Examination examination);
    }
}