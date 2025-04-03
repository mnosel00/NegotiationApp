﻿using NegotiationApp.Application.DTOs;
using NegotiationApp.Application.Interfaces;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Application.Services
{
    public class NegotiationService : INegotiationService
    {
        private readonly INegotiationRepository _negotiationRepository;

        public NegotiationService(INegotiationRepository negotiationRepository)
        {
            _negotiationRepository = negotiationRepository;
        }

        public async Task AcceptNegotiationAsync(int id)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(id);

            negotiation.Accept();
            await _negotiationRepository.UpdateAsync(negotiation);
        }

        public async Task AddNegotiationAsync(CreateNegotiationDto negotiationDto)
        {
            var negotiation = new Negotiation(negotiationDto.ProductId, negotiationDto.ProposedPrice);
            await _negotiationRepository.AddAsync(negotiation);
        }

        public async Task<TimeSpan> CheckExpirationAsync(int id)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(id);

            var timeRemaining = negotiation.CheckExpiration();
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

            return new NegotiationDto(negotiation.ProductId, negotiation.ProposedPrice, negotiation.ProposedAt, negotiation.Attempts, negotiation.Status.ToString());
        }

        public async Task ProposeNewPriceAsync(int id, decimal newPrice)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(id);

            negotiation.ProposeNewPrice(newPrice);
            await _negotiationRepository.UpdateAsync(negotiation);
        }

        public async Task RejectNegotiationAsync(int id)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(id);
            
            negotiation.Reject();
            await _negotiationRepository.UpdateAsync(negotiation);
        }
    }
}
