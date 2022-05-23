﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using HealthInstitution.MVVM.Models.Entities;
using HealthInstitution.MVVM.ViewModels.Commands.DoctorCommands;
using HealthInstitution.MVVM.Models;
using HealthInstitution.MVVM.Models.Enumerations; 


namespace HealthInstitution.MVVM.ViewModels.DoctorViewModels
{
    class DoctorPendingMedicineViewModel : BaseViewModel
    {
        public DoctorNavigationViewModel Navigation { get; }

        private readonly ObservableCollection<PendingMedicineItemViewModel> _pendingMedicines;
        public IEnumerable<PendingMedicineItemViewModel> PendingMedicines => _pendingMedicines;

        public ICommand DeletePendingMedicine { get; }
        public ICommand SendToRevision { get; }

        private PendingMedicineItemViewModel _selectedMedicine;
        public PendingMedicineItemViewModel SelectedMedicine => _selectedMedicine;

        private bool _dialogOpen;
        public bool DialogOpen
        {
            get => _dialogOpen;
            set
            {
                _dialogOpen = value;
                OnPropertyChanged(nameof(DialogOpen));
            }
        }

        private bool _enableChanges;
        public bool EnableChanges
        {
            get => _enableChanges;
            set
            {
                _enableChanges = value;
                OnPropertyChanged(nameof(EnableChanges));
            }
        }

        private int _selection;
        public int Selection
        {
            get => _selection;
            set
            {
                if (value < 0) { return; };
                _selection = value;
                EnableChanges = true;
                OnPropertyChanged(nameof(Selection));
                _selectedMedicine = _pendingMedicines.ElementAt(_selection);
            }
        }

        private string _revisionReason;
        public string RevisionReason
        {
            get
            {
                return _revisionReason;
            }
            set
            {
                _revisionReason = value;
                OnPropertyChanged(nameof(RevisionReason));
            }
        }

        public DoctorPendingMedicineViewModel()
        {
            bool isSpecialist = true;
            Doctor doctor = (Doctor)Institution.Instance().CurrentUser;
            if (doctor.Specialization == Specialization.NONE) isSpecialist = false;
            Navigation = new DoctorNavigationViewModel(isSpecialist);

            // initializing
            _pendingMedicines = new ObservableCollection<PendingMedicineItemViewModel>();

            // commands
            DeletePendingMedicine = new DeletePendingMedicineCommand(this);
            SendToRevision = new RevisionPendingMedicineCommand(this);

            // filling with data
            SetProperties();
            FindPendingMedicines();
        }

        public void SetProperties()
        {
            if (_selectedMedicine != null) RevisionReason = _selectedMedicine.RevisionReason;
        }


        public void FindPendingMedicines()
        {
            _pendingMedicines.Clear();
            List<PendingMedicine> pendingMedicines = Institution.Instance().PendingMedicineRepository.PendingMedicines;
            foreach(PendingMedicine pendingMedicine in pendingMedicines)
            {
                if (pendingMedicine.State == State.ON_HOLD)
                _pendingMedicines.Add(new PendingMedicineItemViewModel(pendingMedicine));
            }
        }

    }
}