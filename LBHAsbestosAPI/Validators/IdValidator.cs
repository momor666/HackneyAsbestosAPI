﻿using System;
using System.Linq;

namespace LBHAsbestosAPI.Validators
{
    public static class IdValidator
    {
        public static bool ValidatePropertyId(string propertyId)
        {
            // TODO current validation constrains are temporal and have yet to be reviewed 
            var validIdMaxLength = 11;

            if (propertyId.Length >= validIdMaxLength)
            {
                return false;
            }
            if (propertyId.Any(c => !char.IsDigit(c)))
            {
                return false;
            }
            return true;
        }

        public static bool ValidateId(string id)
        {
            var validIdMaxLength = 7;

            if (id.Length >= validIdMaxLength)
            {
                return false;
            }
            if (id.Any(c => !char.IsDigit(c)))
            {
                return false;
            }
            return true;
        }
    }
}
