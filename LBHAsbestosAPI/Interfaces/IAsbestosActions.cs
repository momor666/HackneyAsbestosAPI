﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHAsbestosAPI.Controllers;
using LBHAsbestosAPI.Entities;
using Microsoft.Extensions.Logging;

namespace LBHAsbestosAPI.Actions
{
    public interface IAsbestosActions
    {
        Task<Floor> GetFloor(string floorId);
        Task<IEnumerable<Inspection>> GetInspection(string propertyId);
        Task<Room> GetRoom(string roomId);
    }
}
