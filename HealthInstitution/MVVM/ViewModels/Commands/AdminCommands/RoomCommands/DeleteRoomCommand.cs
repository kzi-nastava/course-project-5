﻿using HealthInstitution.Commands;
using HealthInstitution.Exceptions.AdminExceptions;
using HealthInstitution.MVVM.Models;
using HealthInstitution.MVVM.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.MVVM.ViewModels.Commands.AdminCommands
{
    class DeleteRoomCommand : BaseCommand
    {
        private AdminRoomViewModel _model;

        public DeleteRoomCommand(AdminRoomViewModel model)
        {
            _model = model;
        }

        public override void Execute(object parameter)
        {
            try
            {
                _model.DialogOpen = false;

                Institution.Instance().RoomRepository.DeleteRoom(_model.SelectedRoom.Room);

                _model.FillRoomList();
            } catch (RoomCannotBeChangedException e)
            {
                _model.ShowMessage(e.Message);
            } catch (Exception e)
            {
                _model.ShowMessage(e.Message);
            }
        }
    }
}
