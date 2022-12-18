using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using visonBoxGame.Models;
using visonBoxGame.Services;

namespace visonBoxGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        /// <summary>Get deck of available cards.</summary>
        /// <response code="200">
        /// Deck of cards.
        /// </response>
        [HttpGet("GetCardsDeck")]
        public async Task<IActionResult> GetCardsDeck(string gameId)
        {
            try
            {
                if (!Guid.TryParse(gameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                Guid _gameId = Guid.Parse(gameId);

                var result = await _cardService.GetDeck(_gameId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Guess next card add 'H' for High or 'L' for Low.</summary>
        /// <response code="200">
        /// Confirms the card is guess. see the result in score 'GetResult' method.
        /// </response>
        [HttpGet("GuessNextCard")]
        public async Task<IActionResult> GuessNextCard([FromQuery] GuessCardModel model)
        {
            try
            {
                if (!Guid.TryParse(model.PlayerId, out var _))
                    return BadRequest("Player Id is not given in correct format.");

                if (!Guid.TryParse(model.GameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                var result = await _cardService.GuessNextCard(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
