﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHAsbestosAPI.Entities;
using LBHAsbestosAPI.Interfaces;

namespace LBHAsbestosAPI.Repositories
{
    public class FakePsi2000Api : IPsi2000Api
    {
        static int triggerExceptionIdLength = 4;
        static int nullResponseIdLength = 5;

        public Task<InspectionResponse> GetInspections(string propertyId)
        {
            if (propertyId.Length == triggerExceptionIdLength)
            {
                throw new TestExceptionInFakePSI();
            }

            var fakeInspectionresponse = new InspectionResponse()
            {
                Success = true
            };

            if (propertyId.Length == nullResponseIdLength)
            {
                fakeInspectionresponse.Data = new List<Inspection>();
            }
            else
            {
                fakeInspectionresponse.Data = new List<Inspection>()
                {
                    new Inspection()
                    {
                        Id = 655,
                        LocationDescription = "A house"
                    }
                };
            }

            return Task.FromResult(fakeInspectionresponse);
        }

        public Task<RoomResponse> GetRoom(string roomId)
        {
            if (roomId.Length == triggerExceptionIdLength)
            {
                throw new TestExceptionInFakePSI();
            }

            var fakeRoomResponse = new RoomResponse()
            {
                Success = true
            };

            if (roomId.Length == nullResponseIdLength)
            {
                fakeRoomResponse.Data = null;
            }
            else
            {
                fakeRoomResponse.Data = new Room()
                {
                    Id = 5003,
                    Description = "Ground Floor"
                };
            }

            return Task.FromResult(fakeRoomResponse);
        }

        public Task<FloorResponse> GetFloor(string floorId)
        {
            if (floorId.Length == triggerExceptionIdLength)
            {
                throw new TestExceptionInFakePSI();
            }

            var fakeFloorResponse = new FloorResponse()
            {
                Success = true
            };

            if (floorId.Length == nullResponseIdLength)
            {
                fakeFloorResponse.Data = null;
            }
            else
            {
                fakeFloorResponse.Data = new Floor()
                {
                    Id = 3434,
                    Description = "First Floor"
                };
            }

            return Task.FromResult(fakeFloorResponse);
        }

        public Task<ElementResponse> GetElement(string elementId)
        {
            if (elementId.Length == triggerExceptionIdLength)
            {
                throw new TestExceptionInFakePSI();
            }

            var fakeElementResponse = new ElementResponse()
            {
                Success = true
            };

            if (elementId.Length == nullResponseIdLength)
            {
                fakeElementResponse.Data = null;
            }
            else
            {
                fakeElementResponse.Data = new Element()
                {
                    Id = 3434,
                    Description = "First Floor"
                };
            }

            return Task.FromResult(fakeElementResponse);
        }

        public Task<bool> Login()
        {
            throw new NotImplementedException();
        }
    }

    public class TestExceptionInFakePSI : Exception { }
}
