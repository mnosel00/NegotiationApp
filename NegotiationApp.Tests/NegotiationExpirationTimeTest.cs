using FluentAssertions;
using Moq;
using NegotiationApp.Application.Interfaces;
using NegotiationApp.Application.Services;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Enums;
using NegotiationApp.Domain.Interfaces;
using System.Diagnostics;
using System.Reflection;


namespace NegotiationApp.Tests
{
    public class NegotiationExpirationTimeTest
    {
        private readonly Mock<INegotiationRepository> _negotiationRepositoryMock;
        private readonly INegotiationService _negotiationService;

        public NegotiationExpirationTimeTest()
        {
            _negotiationRepositoryMock = new Mock<INegotiationRepository>();
            _negotiationService = new NegotiationService(_negotiationRepositoryMock.Object);
        }
        [Fact]
        public async Task CheckExpirationAsync_ShouldThrowException_WhenNegotiationNotFound()
        {
          
            _negotiationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Negotiation)null);
            
            Func<Task> act = async () => await _negotiationService.CheckExpirationAsync(1);
            
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Negotiation not found.");
        }

        [Fact]
        public async Task CheckExpirationAsync_ShouldReturnTimeRemaining_WhenNegotiationIsRejectedAndNotExpired()
        {
            
            var negotiation = new Negotiation(1, 100);

            SetPrivateProperty(negotiation, nameof(Negotiation.Status), NegotiationStatus.Rejected);
            SetPrivateProperty(negotiation, nameof(Negotiation.ProposedAt), DateTime.UtcNow.AddDays(-6).AddMilliseconds(-1));

            _negotiationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(negotiation);

            
            var timeRemaining = await _negotiationService.CheckExpirationAsync(1);

            
            timeRemaining.Should().BeCloseTo(TimeSpan.FromDays(1), TimeSpan.FromMilliseconds(20));
            negotiation.Status.Should().Be(NegotiationStatus.Rejected);
            _negotiationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Negotiation>()), Times.Never);
        }

        [Fact]
        public async Task CheckExpirationAsync_ShouldExpireNegotiation_WhenNegotiationIsRejectedAndExpired()
        {
            
            var negotiation = new Negotiation(1, 100);

            SetPrivateProperty(negotiation, nameof(Negotiation.Status), NegotiationStatus.Rejected);
            SetPrivateProperty(negotiation, nameof(Negotiation.ProposedAt), DateTime.UtcNow.AddDays(-8));

            Debug.WriteLine($"[Test] ProposedAt: {negotiation.ProposedAt}, Status: {negotiation.Status}");

            _negotiationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(negotiation);

            
            var timeRemaining = await _negotiationService.CheckExpirationAsync(1);

            
            timeRemaining.Should().Be(TimeSpan.Zero);
            negotiation.Status.Should().Be(NegotiationStatus.Expired);
            _negotiationRepositoryMock.Verify(repo => repo.UpdateAsync(negotiation), Times.Once);
        }


        private static void SetPrivateProperty<T>(T obj, string propertyName, object value)
        {
            var type = typeof(T);

            var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null)
            {
                var setter = property.GetSetMethod(true);
                if (setter != null)
                {
                    setter.Invoke(obj, new[] { value });
                }
            }
        }

    }
}