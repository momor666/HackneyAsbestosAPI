﻿using System;
using Xunit;
using LBHAsbestosAPI.Validators;

namespace UnitTests
{
    public class InspectionIdValidationTests
    {
        [Theory]
        [InlineData("12345678", true)]
        [InlineData("1", false)]
        [InlineData("123456789", false)]
        [InlineData("abc", false)]
        [InlineData("A1234567", false)]
        [InlineData("1!234567", false)]
        public void return_boolean_if_InspectionId_is_valid(string propertyId, bool expected)
        {
            var validationResult = InspectionIdValidator.Validate(propertyId);
            Assert.Equal(expected, validationResult);
        }
    }
}