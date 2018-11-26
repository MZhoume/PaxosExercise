using System.Threading.Tasks;
using AutoMapper;
using Challenge1.Core.ActionResults;
using Challenge1.Data.Abstractions;
using Challenge1.Data.Dtos;
using Challenge1.Features;
using Challenge1.Features.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Challenge1.Tests
{
    public class Tests
    {
        [Fact]
        public async Task ValidMessageShouldGenerateHashAndSaveToDatabase()
        {
            var message = "foo";
            var expectedHash = "2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae";
            var saved = false;

            var repo = new Mock<IServiceRepository>();
            repo.Setup(r => r.HashExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            repo.Setup(r => r.SaveChangesAsync()).Callback(() => saved = true).ReturnsAsync(true);
            var mapper = this.GetConfiguredMapper();
            var logger = Mock.Of<ILogger<MessageController>>();

            var request = new MessageRequestModel(message);

            var controller = new MessageController(repo.Object, mapper, logger);
            var response = await controller.CreateMessage(request);

            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var result = Assert.IsType<MessageRequestModel>(objectResult.Value);
            Assert.Equal(expectedHash, result.Digest);
            Assert.True(saved);
        }

        [Fact]
        public async Task SeenMessageShouldGenerateHashButNotSaveToDatabase()
        {
            var message = "foo";
            var expectedHash = "2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae";
            var saved = false;

            var repo = new Mock<IServiceRepository>();
            repo.Setup(r => r.HashExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            repo.Setup(r => r.SaveChangesAsync()).Callback(() => saved = true).ReturnsAsync(true);
            var mapper = this.GetConfiguredMapper();
            var logger = Mock.Of<ILogger<MessageController>>();

            var request = new MessageRequestModel(message);

            var controller = new MessageController(repo.Object, mapper, logger);
            var response = await controller.CreateMessage(request);

            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var result = Assert.IsType<MessageRequestModel>(objectResult.Value);
            Assert.Equal(expectedHash, result.Digest);
            Assert.False(saved);
        }

        [Fact]
        public async Task InvalidHashShouldGenerateBadRequestResult()
        {
            var repo = Mock.Of<IServiceRepository>();
            var mapper = Mock.Of<IMapper>();
            var logger = Mock.Of<ILogger<MessageController>>();

            var controller = new MessageController(repo, mapper, logger);

            var response = await controller.GetMessage(string.Empty);
            var objectResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.IsType<ErrorResult>(objectResult.Value);

            // Hash of length 63 characters.
            response = await controller.GetMessage("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            objectResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.IsType<ErrorResult>(objectResult.Value);

            // Hash of length 65 characters
            response = await controller.GetMessage("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            objectResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.IsType<ErrorResult>(objectResult.Value);
        }

        [Fact]
        public async Task UnseenHashShouldGenerateNotFoundResult()
        {
            var hash = "2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae";

            var repo = new Mock<IServiceRepository>();
            repo.Setup(r => r.HashExistsAsync(hash)).ReturnsAsync(false);
            var mapper = Mock.Of<IMapper>();
            var logger = Mock.Of<ILogger<MessageController>>();

            var controller = new MessageController(repo.Object, mapper, logger);

            var response = await controller.GetMessage(hash);

            var objectResult = Assert.IsType<NotFoundObjectResult>(response.Result);
            Assert.IsType<ErrorResult>(objectResult.Value);
        }

        [Fact]
        public async Task SeenHashShouldReturnStoredMessage()
        {
            var hash = "2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae";
            var dto = new HashDto()
            {
                Hash = hash,
                Value = "foo"
            };

            var repo = new Mock<IServiceRepository>();
            repo.Setup(r => r.HashExistsAsync(hash)).ReturnsAsync(true);
            repo.Setup(r => r.FindHashAsync(hash)).ReturnsAsync(dto);
            var mapper = this.GetConfiguredMapper();
            var logger = Mock.Of<ILogger<MessageController>>();

            var controller = new MessageController(repo.Object, mapper, logger);

            var response = await controller.GetMessage(hash);

            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var result = Assert.IsType<MessageResponseModel>(objectResult.Value);
            Assert.Equal("foo", result.Message);
        }

        private IMapper GetConfiguredMapper()
        {
            return new MapperConfiguration(options => options.AddProfile(new MappingConfig()))
                .CreateMapper();
        }
    }
}
