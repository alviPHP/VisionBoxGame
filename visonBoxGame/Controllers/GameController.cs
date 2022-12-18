using visonBoxGame.DeckCards;
using visonBoxGame.Models;
using visonBoxGame.Models.Response;
using visonBoxGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace visonBoxGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {        
        private short _minPlayersValue;
        private short _maxPlayersValue;
        private readonly IGameService _gameService;
        public GameController(IGameService gameService,
                              IConfiguration configuration)
        {
            _gameService = gameService;
            _minPlayersValue = Convert.ToInt16(configuration["MinPlayersValue"]);
            _maxPlayersValue = Convert.ToInt16(configuration["MaxPlayersValue"]);
        }

        /// <summary>Get all available games that needs start.</summary>
        /// <response code="200">
        /// List of available Games.
        /// </response>
        [HttpGet("GetAllGames")]
        public IActionResult Get()
        {
            try
            {
                var result =  _gameService.GetAllGames();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Create and join game.</summary>
        /// <response code="200">
        /// game Id. 
        /// player Id.
        /// </response>
        [HttpPost("CreateGame")]
        public async Task<IActionResult> CreateGame([FromBody] GameBindingModel model)
        {
            try
            {
                var result = await  _gameService.CreateGame(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Join game (a player can participate only one game at a moment).</summary>
        /// <response code="200">
        /// Confirms player joins.
        /// </response>
        [HttpPost("JoinGame")]
        public IActionResult JoinGame([FromBody] GameIdModel model)
        {
            try
            {
                if (!Guid.TryParse(model.PlayerId, out var _))
                    return BadRequest("Player Id is not given in correct format.");

                if (!Guid.TryParse(model.GameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                _gameService.JoinGame(model, _maxPlayersValue);
                return Ok("Game joined.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Start Game.</summary>
        /// <response code="200">
        /// Confirm player started the game.
        /// </response>
        [HttpPost("StartGame")]
        public async Task<IActionResult> StartGame([FromBody] GameIdModel model)
        {
            try
            {
                if (!Guid.TryParse(model.PlayerId, out var _))
                    return BadRequest("Player Id is not given in correct format.");

                if (!Guid.TryParse(model.GameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                var result = await _gameService.StartGame(model, _minPlayersValue);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
