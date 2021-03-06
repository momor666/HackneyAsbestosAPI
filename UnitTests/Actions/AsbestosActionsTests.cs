﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHAsbestosAPI.Actions;
using LBHAsbestosAPI.Entities;
using LBHAsbestosAPI.Interfaces;
using Moq;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.Actions
{
    public class AsbestosActionsTests
    {
        Mock<ILoggerAdapter<AsbestosActions>> fakeLogger;
        Mock<IAsbestosService> fakeAsbestosService;
        string fakeId;

        public AsbestosActionsTests()
        {
            fakeLogger = new Mock<ILoggerAdapter<AsbestosActions>>();
            fakeAsbestosService = new Mock<IAsbestosService>();
            fakeId = Fake.GenerateRandomId(6).ToString();
        }

        [Fact]
        public async Task return_type_should_be_list_of_inspections()
        {
            var fakeResponse = new List<Inspection>()
            {
                new Inspection()
            };

            fakeAsbestosService
                .Setup(m => m.GetInspection(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<Inspection>>(fakeResponse));

            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);
            var response = await asbestosAction.GetInspection(fakeId);

            Assert.True(response is List<Inspection>);
        }

        [Fact]
        public async Task get_inspection_throws_expected_custom_exeption()
        {
            var fakeEmptyResponse = new List<Inspection>();
            fakeAsbestosService
                .Setup(m => m.GetInspection(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<Inspection>>(fakeEmptyResponse));
            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);

            await Assert.ThrowsAsync<MissingInspectionException>(
                async () => await asbestosAction.GetInspection(fakeId)); 
        }

        [Fact]
        public async Task return_type_should_be_room()
        {
            var fakeResponse = new Room();

            fakeAsbestosService
                .Setup(m => m.GetRoom(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResponse));

            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);
            var response = await asbestosAction.GetRoom(fakeId);

            Assert.True(response is Room);
        }

        [Fact]
        public async Task get_room_throws_expected_custom_exeption()
        {
            Room fakeEmptyResponse = null;
            fakeAsbestosService
                .Setup(m => m.GetRoom(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeEmptyResponse));
            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);

            await Assert.ThrowsAsync<MissingRoomException>(
                async () => await asbestosAction.GetRoom(fakeId));
        }

        [Fact]
        public async Task return_type_should_be_floor()
        {
            var fakeResponse = new Floor();

            fakeAsbestosService
                .Setup(m => m.GetFloor(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResponse));

            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);
            var response = await asbestosAction.GetFloor(fakeId);

            Assert.True(response is Floor);
        }

        [Fact]
        public async Task get_floor_throws_expected_custom_exeption()
        {
            Floor fakeEmptyResponse = null;
            fakeAsbestosService
                .Setup(m => m.GetFloor(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeEmptyResponse));
            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);

            await Assert.ThrowsAsync<MissingFloorException>(
                async () => await asbestosAction.GetFloor(fakeId));
        }

        [Fact]
        public async Task return_type_should_be_element()
        {
            var fakeResponse = new Element();

            fakeAsbestosService
                .Setup(m => m.GetElement(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResponse));

            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);
            var response = await asbestosAction.GetElement(fakeId);

            Assert.True(response is Element);
        }

        [Fact]
        public async Task get_element_throws_expected_custom_exeption()
        {
            Element fakeEmptyResponse = null;
            fakeAsbestosService
                .Setup(m => m.GetElement(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeEmptyResponse));
            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);

            await Assert.ThrowsAsync<MissingElementException>(
                async () => await asbestosAction.GetElement(fakeId));
        }

        [Fact]
        public async Task return_type_should_be_list_of_documents()
        {
            var fakeResponse = Fake.GenerateDocument(123, null);

            fakeAsbestosService
                .Setup(m => m.GetDocument(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResponse));

            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);
            var response = await asbestosAction.GetDocument(fakeId, null);

            Assert.True(response is List<Document>);
        }

        [Fact]
        public async Task get_document_throws_expected_custom_exeption()
        {
            var fakeEmptyResponse = new List<Document>();
            fakeAsbestosService
                .Setup(m => m.GetDocument(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<Document>>(fakeEmptyResponse));
            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);

            await Assert.ThrowsAsync<MissingDocumentException>(
                async () => await asbestosAction.GetDocument(fakeId, null));
        }

        [Fact]
        public async Task return_type_should_be_file_container()
        {
            var fakeResponse = Fake.GenerateFakeFile(null);

            fakeAsbestosService
                .Setup(m => m.GetFile(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResponse));

            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);
            var response = await asbestosAction.GetFile(fakeId, null);

            Assert.True(response is FileContainer);
        }

        [Fact]
        public async Task get_file_throws_expected_custom_exeption()
        {
            var fakeEmptyResponse = new FileContainer();
            fakeAsbestosService
                .Setup(m => m.GetFile(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fakeEmptyResponse));
            var asbestosAction = new AsbestosActions(fakeAsbestosService.Object, fakeLogger.Object);

            await Assert.ThrowsAsync<MissingFileException>(
                async () => await asbestosAction.GetFile(fakeId, null));
        }
    }
}
