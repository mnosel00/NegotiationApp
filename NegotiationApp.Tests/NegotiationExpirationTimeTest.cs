using FluentAssertions;
using Moq;
using NegotiationApp.Application.Interfaces;
using NegotiationApp.Application.Services;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Enums;
using NegotiationApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            // Arrange
            _negotiationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Negotiation)null);

            // Act
            Func<Task> act = async () => await _negotiationService.CheckExpirationAsync(1);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Negotiation not found.");
        }

        [Fact]
        public async Task CheckExpirationAsync_ShouldReturnTimeRemaining_WhenNegotiationIsRejectedAndNotExpired()
        {
            // Arrange
            var negotiation = new Negotiation(1, 100);

            SetPrivateProperty(negotiation, nameof(Negotiation.Status), NegotiationStatus.Rejected);
            SetPrivateProperty(negotiation, nameof(Negotiation.ProposedAt), DateTime.UtcNow.AddDays(-6).AddMilliseconds(-1));

            _negotiationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(negotiation);

            // Act
            var timeRemaining = await _negotiationService.CheckExpirationAsync(1);

            // Assert
            timeRemaining.Should().BeCloseTo(TimeSpan.FromDays(1), TimeSpan.FromMilliseconds(20));
            negotiation.Status.Should().Be(NegotiationStatus.Rejected);
            _negotiationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Negotiation>()), Times.Never);
        }

        [Fact]
        public async Task CheckExpirationAsync_ShouldExpireNegotiation_WhenNegotiationIsRejectedAndExpired()
        {
            // Arrange
            var negotiation = new Negotiation(1, 100);

            SetPrivateProperty(negotiation, nameof(Negotiation.Status), NegotiationStatus.Rejected);
            SetPrivateProperty(negotiation, nameof(Negotiation.ProposedAt), DateTime.UtcNow.AddDays(-8));

            Debug.WriteLine($"[Test] ProposedAt: {negotiation.ProposedAt}, Status: {negotiation.Status}");

            _negotiationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(negotiation);

            // Act
            var timeRemaining = await _negotiationService.CheckExpirationAsync(1);

            // Assert
            timeRemaining.Should().Be(TimeSpan.Zero);
            negotiation.Status.Should().Be(NegotiationStatus.Expired);
            _negotiationRepositoryMock.Verify(repo => repo.UpdateAsync(negotiation), Times.Once);
        }


        private void SetPrivateProperty<T>(T obj, string propertyName, object value)
        {
            var type = typeof(T);

            var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null)
            {
                var setter = property.GetSetMethod(true);
                if (setter != null)
                {
                    setter.Invoke(obj, new[] { value });
                    Debug.WriteLine($"Property {propertyName} set to {value}");
                    return;
                }
            }

            var field = type.GetField(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(obj, value);
                Debug.WriteLine($"Field {propertyName} set to {value}");
                return;
            }

            Debug.WriteLine($"Property or field {propertyName} not found on {typeof(T)}");
        }

    }
}