﻿using HealthInstitution.Commands;
using HealthInstitution.MVVM.Models;
using HealthInstitution.MVVM.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.MVVM.ViewModels.Commands.AdminCommands.EquipmentCommands
{
    class SearchCommand : BaseCommand
    {
        private AdminEquipmentViewModel _model;

        public SearchCommand(AdminEquipmentViewModel model)
        {
            _model = model;
        }

        public override void Execute(object parameter)
        {
            if (_model.SearchPhrase is null)
            {
                MessageBox.Show("You need to enter phrase for search", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _model.AllEquipment = Institution.Instance().EquipmentRepository.Search(_model.SearchPhrase);

                _model.FillEquipmentList();
            }
        }
    }
}
