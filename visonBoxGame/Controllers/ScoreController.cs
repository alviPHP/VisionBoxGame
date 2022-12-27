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
        private readonly IGameService _gameService;

        public ScoreController(IScoreService scoreService
            , IGameService gameService)
        {
            _scoreService = scoreService;
            _gameService = gameService;
        }

        /// <summary>Get score table when a round completed or the game ended.</summary>
        /// <response code="200">
        /// Score table. 
        /// Player Id.
        /// Score of Player.
        /// </response>
        [HttpGet("GetScoreBoard")]
        public IActionResult GetScoreBoard(string gameId)
        {
            try
            {
                #region Validate

                //Check if Game Id is in wrong format
                if (!Guid.TryParse(gameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                #endregion

                #region Fucntion

                //Check if game Id exixts.
                Guid _gameId = Guid.Parse(gameId);

                //Get score table.
                var result = _scoreService.GetScoreTable(_gameId);
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
