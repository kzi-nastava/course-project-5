﻿using HealthInstitution.MVVM.ViewModels.Commands;
using HealthInstitution.MVVM.ViewModels.Commands.PatientCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.MVVM.Views.PatientViews
{
    public class PatientNavigationViewModel
    {

        public ICommand Record { get; }
        public ICommand Appointments { get; }
        public ICommand AdvancedScheduling { get; }
        public ICommand Search { get; }
        public ICommand Survey { get; }
        public ICommand LogOut { get; }

        public PatientNavigationViewModel() {
            LogOut = new LogOutCommand();
            Record = new RecordContentCommand();
            Appointments = new AppointmentsContentCommand();
            Search = new SearchContentCommand();
            Survey = new SurveyContentCommand();
        }
    }
}
