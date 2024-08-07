﻿using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.GameManagement;
using Catan.Application.Responses;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using MediatR;

namespace Catan.Application.Features.Trade.Commands.InitiateTrade
{
	public class InitiateTradeCommandHandler : IRequestHandler<InitiateTradeCommand, GameSessionResponse>
    {
        private GameSessionManager _gameSessionManager;
        private IMapper _mapper;

        public InitiateTradeCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
        {
            _gameSessionManager = gameSessionManager;
            _mapper = mapper;
        }

        public async Task<GameSessionResponse> Handle(InitiateTradeCommand request, CancellationToken cancellationToken)
        {
            var validator = new InitiateTradeCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
                return new GameSessionResponse()
				{
                    Success = false,
                    ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };

            var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

            if (!gameSessionResponse.IsSuccess)
            {
                return new GameSessionResponse()
                {
                    Success = false,
                    ValidationErrors = new List<string>() { gameSessionResponse.Error }
                };
            }

            var gameSession = gameSessionResponse.Value;
            bool playerToGiveExists = false;
            bool playerToReceiveExists = false;
            Player playerToGive = null;
            Player playerToReceive = null;

            foreach (var p in gameSession.Players)
            {
                if (p.Id == request.PlayerToGiveId)
                {
                    playerToGiveExists = true;
                    playerToGive = p;
                }
                if (p.Id == request.PlayerToReceiveId)
                {
                    playerToReceiveExists = true;
                    playerToReceive = p;
                }
            }

            if (!playerToGiveExists || !playerToReceiveExists)
            {
                return new GameSessionResponse()
                {
                    Success = false,
                    ValidationErrors = new List<string>() { "Player does not exist." }
                };
            }

            if (!playerToGive.IsActive || !playerToReceive.IsActive)
            {
                return new GameSessionResponse()
                {
                    Success = false,
                    ValidationErrors = new List<string>() { "Player has been disconnected." }
                };
            }

            if (playerToGive == playerToReceive)
            {
                return new GameSessionResponse()
                {
                    Success = false,
                    ValidationErrors = new List<string>() { "You cannot trade with yourself." }
                };
            }

            var resourceToGive = (Resource)Enum.Parse(typeof(Resource), request.ResourceToGive, true);
            var resourceToReceive = (Resource)Enum.Parse(typeof(Resource), request.ResourceToReceive, true);


            var result = await _gameSessionManager.AddNewPendingTrade(gameSession, playerToGive, resourceToGive, request.CountToGive,
                playerToReceive, resourceToReceive, request.CountToReceive);

            if (!result.IsSuccess)
            {
                return new GameSessionResponse()
                {
                    Success = false,
                    ValidationErrors = new List<string>() { result.Error }
                };
            }

            return new GameSessionResponse()
            {
                Success = true,
                GameSession = _mapper.Map<GameSessionDto>(gameSession)
            };
        }
    }
}
