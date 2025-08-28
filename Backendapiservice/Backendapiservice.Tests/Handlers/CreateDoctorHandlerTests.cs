using Microsoft.EntityFrameworkCore;
using Moq;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Application.Handlers;
using Backendapiservice.Domain.Entities;
using Backendapiservice.Domain.Interfaces;
using Backendapiservice.Infrastructure.Data;
using FluentAssertions;
using Xunit;
using Backendapiservice.Domain.Events;

namespace Backendapiservice.Tests.Handlers
{
    public class CreateDoctorHandlerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Mock<IDomainEventPublisher> _mockEventPublisher;
        private readonly CreateDoctorHandler _handler;

        public CreateDoctorHandlerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _mockEventPublisher = new Mock<IDomainEventPublisher>();
            _handler = new CreateDoctorHandler(_context, _mockEventPublisher.Object);

            // Seed test office
            _context.Offices.Add(new Office
            {
                Id = 1,
                Name = "Test Office",
                Address = "Test Address",
                Phone = "Test Phone",
                CreatedAt = DateTime.UtcNow
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task Handle_ShouldCreateDoctor_WhenValidRequest()
        {
            // Arrange
            var createDoctorDto = new CreateDoctorDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                Password = "Password123!",
                Specialization = "Cardiology",
                OfficeId = 1
            };
            var command = new CreateDoctorCommand(createDoctorDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");
            result.Email.Should().Be("john.doe@test.com");
            result.Specialization.Should().Be("Cardiology");
            result.OfficeId.Should().Be(1);

            _mockEventPublisher.Verify(x => x.PublishAsync(It.IsAny<IDomainEvent>()), Times.Once);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}