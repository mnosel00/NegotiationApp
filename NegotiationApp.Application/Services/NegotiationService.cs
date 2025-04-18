﻿using NegotiationApp.Application.DTOs;
using NegotiationApp.Application.Interfaces;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Enums;
using NegotiationApp.Domain.Interfaces;

namespace NegotiationApp.Application.Services
{
    public class NegotiationService : INegotiationService
    {
        private readonly INegotiationRepository _negotiationRepository;

        public NegotiationService(INegotiationRepository negotiationRepository)
        {
            _negotiationRepository = negotiationRepository;
        }

        public async Task<(bool isSuccess, string errorMessage)> AcceptNegotiationAsync(int id)
        {
            try
            {
                var negotiation = await _negotiationRepository.GetByIdAsync(id);

                if (negotiation == null)
                    return (false, "Negotiation not found.");
                

                await _negotiationRepository.UpdateAsync(negotiation);
                
                return (true, string.Empty);
            }
            catch (InvalidOperationException ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool isSuccess, string errorMessage)> AddNegotiationAsync(CreateNegotiationDto negotiationDto)
        {
            try
            {
                var negotiation = new Negotiation(negotiationDto.ProductId, negotiationDto.ProposedPrice);
                
                await _negotiationRepository.AddAsync(negotiation);
                
                return (true, string.Empty);
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<TimeSpan> CheckExpirationAsync(int id)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(id);

            if (negotiation == null)
                throw new InvalidOperationException("Negotiation not found.");

            var timeRemaining = negotiation.CheckExpiration();

            if (negotiation.Status == NegotiationStatus.Expired)
                await _negotiationRepository.UpdateAsync(negotiation);

            return timeRemaining;
        }

        public async Task<IEnumerable<NegotiationDto>> GetAllNegotiationsAsync()
        {
            var negotiations = await _negotiationRepository.GetAllAsync();
            
            return negotiations.Select(n => new NegotiationDto(n.ProductId, n.ProposedPrice, n.ProposedAt, n.Attempts, n.Status.ToString()));
        }

        public async Task<NegotiationDto?> GetNegotiationByIdAsync(int id)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(id);

            if (negotiation == null)
                return null;

            return new NegotiationDto(negotiation.ProductId, negotiation.ProposedPrice, negotiation.ProposedAt, negotiation.Attempts, negotiation.Status.ToString());
        }

        public async Task<(bool isSuccess, string errorMessage)> ProposeNewPriceAsync(int id, decimal newPrice)
        {
            try
            {
                var negotiation = await _negotiationRepository.GetByIdAsync(id);

                if (negotiation == null)
                    return (false, "Negotiation not found.");
                

                negotiation.ProposeNewPrice(newPrice);
                
                await _negotiationRepository.UpdateAsync(negotiation);
                
                return (true, string.Empty);
            }
            catch (InvalidOperationException ex)
            {
                return (false, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool isSuccess, string errorMessage)> RejectNegotiationAsync(int id)
        {
            try
            {
                var negotiation = await _negotiationRepository.GetByIdAsync(id);

                if (negotiation == null)
                    return (false, "Negotiation not found.");
                
                negotiation.Reject();
                
                await _negotiationRepository.UpdateAsync(negotiation);
                
                return (true, string.Empty);
            }
            catch (InvalidOperationException ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
