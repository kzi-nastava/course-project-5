﻿using HealthInstitution.MVVM.Models.Entities;
using HealthInstitution.MVVM.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.MVVM.Models.Repositories.Room
{
    public class EquipmentOrderRepository
    {
        private readonly string _fileName;
        private List<EquipmentOrder> _orders;

        public List<EquipmentOrder> Rooms { get => _orders; }

        public EquipmentOrderRepository(string roomsFileName)
        {
            _fileName = roomsFileName;
            _orders = new List<EquipmentOrder>();
        }

        public void LoadFromFile()
        {
            _orders = FileService.Deserialize<EquipmentOrder>(_fileName);
        }

        public void SaveToFile()
        {

            FileService.Serialize<EquipmentOrder>(_fileName, _orders);
        }

        public EquipmentOrder FindById(int id)
        {
            foreach (EquipmentOrder o in _orders)
            {
                if (o.ID == id) return o;
            }
            return null;
        }

        public void Deliver(EquipmentRepository equipments)
        {
            List<EquipmentOrder> futureOrders = new List<EquipmentOrder>();

            foreach (EquipmentOrder o in _orders)
            {
                if (o.isDelivered())
                {
                    Equipment e = equipments.FindById(o.EquipmentID);
                    e.Quantity += o.Quantity;
                }
                else futureOrders.Add(o);
            }
            _orders = futureOrders;
        }
    }
}