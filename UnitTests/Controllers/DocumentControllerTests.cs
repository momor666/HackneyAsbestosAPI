﻿using System;
using System.Threading.Tasks;
using LBHAsbestosAPI.Actions;
using LBHAsbestosAPI.Controllers;
using LBHAsbestosAPI.Entities;
using LBHAsbestosAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.Controllers
{
    public class DocumentControllerTests
    {
        Mock<ILoggerAdapter<AsbestosActions>> fakeActionsLogger;
        Mock<ILoggerAdapter<DocumentController>> fakeControllerLogger;
        Mock<IAsbestosService> fakeAsbestosService;
        DocumentController controller;
        int fakeId;
        Random random;
        int randomPick;

        public DocumentControllerTests()
        {
            fakeActionsLogger = new Mock<ILoggerAdapter<AsbestosActions>>();
            fakeControllerLogger = new Mock<ILoggerAdapter<DocumentController>>();
            fakeAsbestosService = new Mock<IAsbestosService>();

            fakeId = Fake.GenerateRandomId(5);
            random = new Random();
            randomPick = random.Next(3);
        }

        [Fact]
        public async Task return_file_for_valid_request()
        {
            var fakeFile = Fake.GenerateFakeFile("image/jpeg");
            controller = SetupControllerWithServiceReturningFileResponse(fakeFile);
            var response = (FileContentResult)await PickDocumentControllerEndpoint(randomPick, fakeId.ToString());

            Assert.Equal(fakeFile.Size, response.FileContents.Length);

        }

        [Theory]
        [InlineData("abc")]
        [InlineData("1234 56")]
        [InlineData("A123456")]
        [InlineData("1!23456")]
        public async Task return_400_for_invalid_request_on_document_functions(string fileId)
        {
            controller = SetupControllerWithFakeSimpleService();
            var response = (JsonResult)await PickDocumentControllerEndpoint(randomPick, fileId);

            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async Task return_404_for_valid_request_but_no_results()
        {
            var emptyFileResponse = new FileResponse();
            controller = SetupControllerWithServiceReturningFileResponse(emptyFileResponse);
            var response = (JsonResult)await PickDocumentControllerEndpoint(randomPick, fakeId.ToString());

            Assert.Equal(404, response.StatusCode);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("1234 56")]
        [InlineData("123A56")]
        [InlineData("1!23456")]
        public async Task return_error_message_if_floorid_is_not_valid(string fileId)
        {
            controller = SetupControllerWithFakeSimpleService();
            var response = JObject.FromObject((((JsonResult)await PickDocumentControllerEndpoint(
                                                    randomPick, fileId))).Value);

            var userMessage = response["errors"].First["userMessage"].ToString();
            var developerMessage = response["errors"].First["developerMessage"].ToString();

            var expectedUserMessage = "Please provide a valid file id";
            var expectedDeveloperMessage = "Invalid parameter - fileId";

            Assert.Equal(expectedUserMessage, userMessage);
            Assert.Equal(expectedDeveloperMessage, developerMessage);
        }

        private DocumentController SetupControllerWithFakeSimpleService()
        {
            return new DocumentController(fakeAsbestosService.Object,
                                            fakeControllerLogger.Object,
                                            fakeActionsLogger.Object);
        }

        private DocumentController SetupControllerWithServiceReturningFileResponse(FileResponse file)
        {
            fakeAsbestosService = new Mock<IAsbestosService>();
            fakeAsbestosService.Setup(m => m.GetFile(It.IsAny<string>(), It.IsAny<string>()))
                               .Returns(Task.FromResult(file));
            return new DocumentController(fakeAsbestosService.Object,
                                                fakeControllerLogger.Object,
                                                fakeActionsLogger.Object);
        }

        private async Task<IActionResult> PickDocumentControllerEndpoint(int pick, string documentId)
        {
            IActionResult response = null;

            switch (pick)
            {
                case 0:
                    response = await controller.getPhoto(documentId);
                    break;
                case 1:
                    response = await controller.getReport(documentId);
                    break;
                case 2:
                    response = await controller.getDrawing(documentId);
                    break;
                case 3:
                    response = await controller.getMainPhoto(documentId);
                    break;
            }

            return response;
        }
    }
}
