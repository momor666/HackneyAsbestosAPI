﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LBHAsbestosAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using LBHAsbestosAPI.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Text;
using UnitTests.Helpers;

namespace UnitTests.Integration
{
    public class InspectionIntegrationTests
    {
        readonly TestServer server;
        readonly HttpClient client;
        string baseUri;
        static string triggerExceptionId = "999999";
        static string triggerNotFoundId = "888888";

        public InspectionIntegrationTests()
        {
            server = new TestServer(new WebHostBuilder()
                                     .UseStartup<TestStartup>());
            client = server.CreateClient();
            baseUri = "api/v1/inspection/";
        }

        [Fact]
        public async Task return_200_for_valid_request()
        {
            var randomId = Fake.GenerateRandomId(6);
            var result = await client.GetAsync(baseUri + randomId);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Theory]
        [InlineData("12345678910")]
        [InlineData("abc")]
        [InlineData("A1234567")]
        [InlineData("1!234567")]
        [InlineData("12 456")]
        public async Task return_400_for_invalid_request(string propertyId)
        {
            var result = await client.GetAsync(baseUri + propertyId);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task return_404_if_request_is_successful_but_no_results()
        {
            var result = await client.GetAsync(baseUri + triggerNotFoundId);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task return_500_when_internal_server_error()
        {
            var result = await client.GetAsync(baseUri + triggerExceptionId);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task return_valid_json_for_valid_requests()
        {
            var expectedResultContent = new Inspection()
            {
                Id = 655,
                LocationDescription = "A house"
            };

            var expectedResult = new Dictionary<string, IEnumerable<Inspection>>()
            {
                { "results", new List<Inspection>()
                    {
                        {expectedResultContent}
                    }}
            };

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var expectedStringResult = JsonConvert.SerializeObject(expectedResult, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });

            var randomId = Fake.GenerateRandomId(6);
            var result = await client.GetStringAsync(baseUri + randomId);

            Assert.Equal(expectedStringResult, result);
        }

        [Theory]
        [InlineData("12345678910")]
        [InlineData("abc")]
        [InlineData("A1234567")]
        [InlineData("1!234567")]
        [InlineData("12 456")]
        public async Task return_valid_json_for_invalid_requests(string propertyId)
        {
            var json = new StringBuilder();
            json.Append("{");
            json.Append("\"errors\":");
            json.Append("[");
            json.Append("{");
            json.Append("\"userMessage\":\"Please provide a valid property id\",");
            json.Append("\"developerMessage\":\"Invalid parameter - propertyId\"");
            json.Append("}");
            json.Append("]");
            json.Append("}");

            var result = await client.GetAsync(baseUri + propertyId);
            var resultString = await result.Content.ReadAsStringAsync();
            Assert.Equal(json.ToString(), resultString);
        }
    }
}
