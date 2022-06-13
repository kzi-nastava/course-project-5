﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core;
using HealthInstitution.Core.Repositories.References;

namespace HealthInstitution.Core.Services
{
    public class ExaminationChangeService
    {
        private readonly ExaminationChangeRepository _examinationChangeRepository;
        private readonly ExaminationRepository _examinationRepository;
        public ExaminationChangeService()
        {
            _examinationChangeRepository = Institution.Instance().ExaminationChangeRepository;
            _examinationRepository = Institution.Instance().ExaminationRepository;
        }

        public string ApproveChange(int requestId)
        {
            ExaminationChange request = _examinationChangeRepository.FindByID(requestId);
            request.Resolved = true;
            Appointment appointment = _examinationRepository.FindByID(request.AppointmentID);

            if (request.ChangeStatus.ToString() == "EDITED")
            {
                bool resolved = new DoctorRescheduleAppointmentService().RescheduleExamination(appointment, request.NewDate);
                if (resolved)
                {
                    return "The request has been successfully accepted.";
                }
                else
                {
                    return "Request cannot be accepted because either Doctor or Room are not available.";
                }
            }
            else if (request.ChangeStatus.ToString() == "DELETED")
            {
                new SecretaryDeleteAppointmentService().DeleteAppointment(appointment);
                return "The appointment has been successfully deleted.";
            }
            return null;
        }

        public void RejectChange(int requestId)
        {
            ExaminationChange request = _examinationChangeRepository.FindByID(requestId);
            request.Resolve();
        }

        public void RemoveOutdatedRequests()
        {
            foreach (ExaminationChange request in _examinationChangeRepository.Changes)
            {
                if (!request.Resolved && request.NewDate <= DateTime.Now)
                {
                    //request.ChangeStatus = Models.Enumerations.AppointmentStatus.DELETED;
                    request.Resolved = true;
                }
            }
        }
    }
}