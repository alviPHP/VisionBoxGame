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
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        /// <summary>Add a player.</summary>
        /// <response code="200">
        /// Player Id.
        /// </response>
        [HttpPost("AddPlayer")]
        public async Task<IActionResult> AddPlayer(string playerName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playerName))
                    return BadRequest("Player name required.");

                var result = await _playerService.AddPlayer(playerName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
