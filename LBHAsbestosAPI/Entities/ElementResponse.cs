﻿using System;
using System.Collections.Generic;

namespace LBHAsbestosAPI.Entities
{
    public class ElementResponse
    {
		public bool Success { get; set; }
        public string ErrorMesage { get; set; }
        public Element Data { get; set; }
    }
}
