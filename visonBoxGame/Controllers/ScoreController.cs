using visonBoxGame.Models;
using visonBoxGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace visonBoxGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreService _scoreService;

        public ScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        /// <summary>Get score table when a round completed.</summary>
        /// <response code="200">
        /// Score table. 
        /// Player Id.
        /// Score of Player.
        /// </response>
        [HttpGet("GetScoreBoard")]
        public async Task<IActionResult> GetScoreBoard(string gameId)
        {
            try
            {
                if (!Guid.TryParse(gameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                Guid _playerId = Guid.Parse(gameId);

                var result = await _scoreService.GetScoreTable(_playerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Get the outcome after each turn.</summary>
        /// <response code="200">
        ///  Next Player Id.
        ///  Next PlayerName.
        ///  Last Card Played.
        ///  Result.
        /// </response>
        
        [HttpGet("GetResult")]
        public async Task<IActionResult> GetResult(string gameId)
        {
            try
            {
                if (!Guid.TryParse(gameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                Guid _gameId = Guid.Parse(gameId);

                var result =await _scoreService.GetResult(_gameId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
