using System;
using System.IO;
using System.Threading.Tasks;
using ContactsAPI.Controllers;
using ContactsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace ContactsAPI.Tests
{
    // NOTE: For time constraints limits, ther is only unit test coverage for
    // AddUpdateContactRecordProfileImageById, which is arguably the most complex function of the controller.
    // Unit test should cover all scenarios for all controller methods in production environment.

    public class ContactRecordsControllerFixture
    {
        private IContactRecordsService _contactRecordsService;
        private ILogger<ContactRecordsController> _logger;
        private ContactRecordsController _contactRecordsController;

        [SetUp]
        public void Setup()
        {
            _contactRecordsService = Substitute.For<IContactRecordsService>();
            _logger = Substitute.For<ILogger<ContactRecordsController>>();
            _contactRecordsController = new ContactRecordsController(_contactRecordsService, _logger);
        }

        [Test]
        public async Task AddUpdateContactRecordProfileImageById_ValidPicture_ShouldUpload()
        {
            // Arrange
            var id = 1;
            var fileName = "phillip-jeffries.jpg";
            var bytes = GetTestFileBytes(fileName);
            var stream = new MemoryStream(bytes);
            IFormFile file = new FormFile(stream, 0, bytes.Length, "file", fileName);

            // Act
            var actionResult = await _contactRecordsController.AddUpdateContactRecordProfileImageById(id, file);
            
            // Assert
            var objectResponse = actionResult as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
            Assert.IsTrue((string)objectResponse.Value == "File uploaded successfully.");
        }

        [Test]
        public async Task AddUpdateContactRecordProfileImageById_ExceededSizePicture_ShouldNotUpload()
        {
            // Arrange
            var id = 1;
            var fileName = "dougie-jones.jpg";
            var bytes = GetTestFileBytes(fileName);
            var stream = new MemoryStream(bytes);
            IFormFile file = new FormFile(stream, 0, bytes.Length, "file", fileName);

            // Act
            var actionResult = await _contactRecordsController.AddUpdateContactRecordProfileImageById(id, file);

            // Assert
            var objectResponse = actionResult as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 400);
            Assert.IsTrue((string)objectResponse.Value == "The file must not be larger than 256 KB.");
        }

        [Test]
        public async Task AddUpdateContactRecordProfileImageById_InvalidFormatPicture_ShouldNotUpload()
        {
            // Arrange
            var id = 1;
            var fileName = "laura-palmer.bmp";
            var bytes = GetTestFileBytes(fileName);
            var stream = new MemoryStream(bytes);
            IFormFile file = new FormFile(stream, 0, bytes.Length, "file", fileName);

            // Act
            var actionResult = await _contactRecordsController.AddUpdateContactRecordProfileImageById(id, file);

            // Assert
            var objectResponse = actionResult as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 400);
            Assert.IsTrue((string)objectResponse.Value == "The file type must be .jpeg or .png.");
        }

        private byte[] GetTestFileBytes(string fileName)
        {
            var workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            var imageBytes = File.ReadAllBytes(projectDirectory + "\\TestFiles\\" + fileName);
            
            return imageBytes;
        }
    }
}