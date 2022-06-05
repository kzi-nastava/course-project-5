﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.MVVM.Models.Entities;
using HealthInstitution.MVVM.Models.Repositories;
using HealthInstitution.Repositories;
using HealthInstitution.MVVM.Models.Services;
using HealthInstitution.MVVM.Models.Services.DoctorServices;

namespace HealthInstitution.MVVM.Models.Services
{
    public class EmergencyAppointmentService
    {
        private DateTime _newDate;
        private Doctor _doctor;
        private Patient _patient;
        private int _newDuration;

        private readonly DoctorRepository _doctorRepository;
        private readonly ExaminationRepository _examinationRepository;
        private readonly OperationRepository _operationRepository;

        public EmergencyAppointmentService()
        {
            _doctor = null;
            _doctorRepository = Institution.Instance().DoctorRepository;
            _examinationRepository = Institution.Instance().ExaminationRepository;
            _operationRepository = Institution.Instance().OperationRepository;
        }

        public void ChangeDuration(int duration)
        {
            _newDuration = duration;
        }

        public bool CreateEmergencyAppointment(Specialization specialization, Patient patient)
        {
            _patient = patient;
            DateTime startTime = DateTime.Now.AddMinutes(5);
            DateTime currentTime = DateTime.Now.AddMinutes(5);
            _newDate = currentTime;
            string type = _newDuration == 15 ? nameof(Examination) : nameof(Operation);
            bool done = false;

            for (; currentTime < startTime.AddHours(2); currentTime = currentTime.AddMinutes(15))
            {
                bool specialistException = false;
                foreach (Doctor doctor in _doctorRepository.Doctors)
                {
                    if (doctor.Specialization == specialization)
                    {
                        specialistException = true;
                        Examination appointment = new(doctor, patient, currentTime);
                        done = new SecretaryScheduleAppointmentService().ScheduleAppointment(appointment, _newDuration, false);
                        if (done)
                        {
                            _newDate = currentTime;
                            _doctor = doctor;
                            return true;
                        }
                    }
                }
                if (!specialistException)
                    throw new Exception("There are currently no doctors with selected specialization that work in this hospital.");
            }
            return done;
        }

        public Appointment FindAppointment(Patient patient, Doctor doctor, DateTime newDate)
        {
            Appointment appointment = _examinationRepository.FindAppointment(doctor, patient, newDate);
            if (appointment == null)
                appointment = _operationRepository.FindAppointment(doctor, patient, newDate);
            return appointment;
        }

        public void FindNewAppointmentTime(Appointment appointment, int duration, Dictionary<Appointment, DateTime> appointments)
        {
            //if (appointment.Emergency) return;
            DateTime startTime = appointment.Date;

            while (true)
            {
                startTime = startTime.AddMinutes(15);
                try
                {
                    Examination examination = new(appointment.Doctor, appointment.Patient, startTime);
                    bool done = CheckNewAppointmentTime(examination, duration, false);
                    if (done)
                    {
                        appointments.Add(appointment, startTime);
                        break;
                    }
                }
                catch (Exception e) { }
            }
        }

        private bool CheckNewAppointmentTime(Appointment examination, int duration, bool validation = false)
        {
            PatientService patientService = new PatientService(examination.Patient);
            DoctorService doctorService = new DoctorService(examination.Doctor);
            if (!doctorService.IsAvailable(examination.Date, duration))
            {
                return false;
            }
            if (!doctorService.IsAvailable(examination.Date, duration))
            {
                return false;
            }
            new ValidationService().ValidateAppointmentData(examination, examination.Date, validation);
            return true;
        }

        public void NotifyDoctor()
        {
            Appointment newAppointment = FindAppointment(_patient, _doctor, _newDate);
            newAppointment.Emergency = true;
            _doctor.Notifications.Add("An emergency appointment with id=" + newAppointment.ID.ToString() + " has been scheduled!");
        }

        public void SendNotifications(Appointment rescheduledAppointment, Appointment emergencyAppointment)
        {
            string message = "Appointment with id=" + rescheduledAppointment.ID.ToString() + " has been changed." +
                " Changed date from " + emergencyAppointment.Date.ToString() + " to " + rescheduledAppointment.Date.ToString();
            Notification notification = Institution.Instance().NotificationRepository.CreateNotification(emergencyAppointment.Patient.ID, message);
            emergencyAppointment.Patient.Notifications.Add(notification);
            emergencyAppointment.Doctor.Notifications.Add(message);
            emergencyAppointment.Emergency = true;
            emergencyAppointment.Doctor.Notifications.Add("An emergency appointment with id=" + emergencyAppointment.ID.ToString() + " has been scheduled!");
        }

    }
}
