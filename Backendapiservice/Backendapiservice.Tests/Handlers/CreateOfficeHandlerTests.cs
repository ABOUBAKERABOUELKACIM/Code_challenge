using Microsoft.EntityFrameworkCore;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Application.Handlers;
using Backendapiservice.Infrastructure.Data;
using FluentAssertions;
using Xunit;

namespace Backendapiservice.Tests.Handlers
{
    public class CreateOfficeHandlerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly CreateOfficeHandler _handler;

        public CreateOfficeHandlerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _handler = new CreateOfficeHandler(_context);
        }

        [Fact]
        public async Task Handle_ShouldCreateOffice_WhenValidRequest()
        {
            // Arrange
            var createOfficeDto = new CreateOfficeDto
            {
                Name = "Test Medical Center",
                Address = "123 Test Street",
                Phone = "+1-555-0123"
            };
            var command = new CreateOfficeCommand(createOfficeDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Medical Center");
            result.Address.Should().Be("123 Test Street");
            result.Phone.Should().Be("+1-555-0123");
            result.Id.Should().BeGreaterThan(0);

            var officeInDb = await _context.Offices.FirstOrDefaultAsync();
            officeInDb.Should().NotBeNull();
            officeInDb.Name.Should().Be("Test Medical Center");
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}