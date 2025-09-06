using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using ConferenceManager.WPF.Repositories;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Data;

namespace ConferenceManager.WPF.Tests
{
    public class ConferenceServiceTests
    {
        private readonly Mock<IConferenceRepository> _mockConferenceRepository;
        private readonly Mock<ISpeakerRepository> _mockSpeakerRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IDbContextFactory<ApplicationDbContext>> _mockContextFactory;
        private readonly ConferenceService _conferenceService;

        public ConferenceServiceTests()
        {
            _mockConferenceRepository = new Mock<IConferenceRepository>();
            _mockSpeakerRepository = new Mock<ISpeakerRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockContextFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();

            _conferenceService = new ConferenceService(
                _mockConferenceRepository.Object,
                _mockSpeakerRepository.Object,
                _mockDocumentRepository.Object,
                _mockContextFactory.Object
            );
        }

        [Fact]
        public async Task AddConferenceAsync_ValidConference_ReturnsAddedConference()
        {
            // Arrange
            var conference = new Conference
            {
                Title = "Test Conference",
                Description = "Test Description",
                Date = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Location = "Test Location",
                Status = "Planned"
            };

            _mockConferenceRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Conference>()))
                .ReturnsAsync(conference);

            // Act
            var result = await _conferenceService.AddConferenceAsync(conference);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(conference.Title, result.Title);
            Assert.Equal(conference.Description, result.Description);
            Assert.Equal(conference.Location, result.Location);
            Assert.Equal(conference.Status, result.Status);
            _mockConferenceRepository.Verify(repo => repo.AddAsync(It.IsAny<Conference>()), Times.Once);
        }

        [Fact]
        public async Task AddConferenceAsync_NullConference_ThrowsArgumentNullException()
        {
            // Arrange
            Conference conference = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _conferenceService.AddConferenceAsync(conference));
        }

        [Fact]
        public async Task AddConferenceAsync_EmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var conference = new Conference
            {
                Title = "",
                Description = "Test Description",
                Date = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Location = "Test Location",
                Status = "Planned"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _conferenceService.AddConferenceAsync(conference));
        }
    }
} 