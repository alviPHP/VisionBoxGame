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
        private readonly IDeckService _deckService;
        private readonly IGameService _gameService;

        public CardController(IDeckService deckService
            , IGameService gameService)
        {
            _deckService = deckService;
            _gameService = gameService;
        }
        
        /// <summary>Get deck of available cards.</summary>
        /// <response code="200">
        /// Deck of cards.
        /// </response>
        [HttpGet("GetCardsDeck")]
        public IActionResult GetCardsDeck(string gameId)
        {
            try
            {
                #region Validate

                //Check if Game Id is in wrong format
                if (!Guid.TryParse(gameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                #endregion

                #region Function

                Guid _gameId = Guid.Parse(gameId);

                //Get Deck of cards
                var result = _deckService.GetDeck(_gameId);
                if (result == null || result.Count == 0)
                    return NotFound("Recored not found.");
                return Ok(result);

                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
