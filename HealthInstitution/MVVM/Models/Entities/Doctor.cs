﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.MVVM.Models.Entities
{
    public class Doctor : User
    {
        public Specialization Specialization { get; set; }

        public Doctor(Specialization specialization) => Specialization = specialization;
    }
}
