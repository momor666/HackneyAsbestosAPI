using System;
using System.Collections.Generic;

namespace LBHAsbestosAPI.Entities
{
    public class InspectionResponse
    {
	public bool Success { get; set; }
        public string ErrorMessage { get; set; }
	public List<Inspection> Data { get; set; }
    }
}
