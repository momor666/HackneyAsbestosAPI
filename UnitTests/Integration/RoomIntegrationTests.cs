﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LBHAsbestosAPI;
using LBHAsbestosAPI.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.Integration
{
    public class RoomIntegrationTests
    {
        readonly TestServer _server;
        readonly HttpClient _client;
        string _baseUri;

        public RoomIntegrationTests()
        {
            _server = new TestServer(new WebHostBuilder()
                                     .UseStartup<TestStartup>());
            _client = _server.CreateClient();
            _baseUri = "api/v1/room/";
        }

        [Fact]
        public async Task return_200_for_valid_request()
        {
            var randomId = Fake.GenerateRandomId(6);
            var result = await _client.GetAsync(_baseUri + randomId);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("abc")]
        [InlineData("A1234567")]
        [InlineData("1!234567")]
        [InlineData("12 456")]
        public async Task return_400_for_invalid_request(string roomId)
        {
            var result = await _client.GetAsync(_baseUri + roomId);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task return_404_if_request_successful_but_no_results()
        {
            var randomBadId = Fake.GenerateRandomId(5);
            var result = await _client.GetAsync(_baseUri + randomBadId);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task return_500_when_internal_server_error()
        {
            var randomBadId = Fake.GenerateRandomId(4);
            var result = await _client.GetAsync(_baseUri + randomBadId);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task return_valid_json_for_valid_requests()
        {
            var expectedResult = new Dictionary<string, Room>()
            {
                { "results", new Room()
                    {
                        Id = 5003,
                        Description = "Ground Floor"
                    }
                }
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
            var result = await _client.GetStringAsync(_baseUri + randomId);

            Assert.Equal(expectedStringResult, result);
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("abc")]
        [InlineData("A1234567")]
        [InlineData("1!234567")]
        [InlineData("12 456")]
        public async Task return_valid_json_for_invalid_requests(string roomId)
        {
            var json = new StringBuilder();
            json.Append("{");
            json.Append("\"errors\":");
            json.Append("[");
            json.Append("{");
            json.Append("\"userMessage\":\"Please provide a valid room id\",");
            json.Append("\"developerMessage\":\"Invalid parameter - roomId\"");
            json.Append("}");
            json.Append("]");
            json.Append("}");

            var result = await _client.GetAsync(_baseUri + roomId);
            var resultString = await result.Content.ReadAsStringAsync();
            Assert.Equal(json.ToString(), resultString);
        }
    }
}