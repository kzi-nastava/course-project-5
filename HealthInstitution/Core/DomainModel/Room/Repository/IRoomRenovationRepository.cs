﻿using System.Collections.Generic;

namespace HealthInstitution.Core.Repository
{
    public interface IRoomRenovationRepository
    {
        public List<RoomRenovation> GetRooms();
    }
}