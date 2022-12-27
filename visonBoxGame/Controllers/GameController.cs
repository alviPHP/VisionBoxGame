using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using visonBoxGame.Models;
using visonBoxGame.Services;

namespace visonBoxGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private short _minPlayersValue;
        private short _maxPlayersValue;
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;
        private readonly IDeckService _deckService;
        private readonly IStateMachineService _stateMachineService;
        public GameController(IGameService gameService,
                              IPlayerService playerService,
                              IDeckService deckService,
                              IStateMachineService stateMachineService,
                              IConfiguration configuration)
        {
            _gameService = gameService;
            _playerService = playerService;
            _deckService = deckService;
            _stateMachineService = stateMachineService;
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
                var result = _gameService.GetGames();
                if (result.Any())
                    return NotFound("List of games not found.");
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
        public IActionResult CreateGame([FromBody] GameBindingModel model)
        {
            try
            {
                #region Validate

                //Check if title is already exists.
                if (_gameService.TitleExists(model.Title))
                    return BadRequest($"Game title : {model.Title.ToUpper()} already exists.");

                #endregion

                #region Function

                //Add new game
                var game = _gameService.AddGame(model);

                //Add and join new player
                var playerId = _playerService.AddPlayer(model.PlayerName, game.Id);

                //Add new deck of cards
                _deckService.AddNewDeck(game.Id);

                //Create Finite State Machine
                _stateMachineService.Create(game.Id);

                //Return gameId and playerId
                var gameIds = new GameIdModel
                {
                    GameId = game.Id.ToString(),
                    PlayerId = playerId.ToString()
                };

                return Ok(gameIds);

                #endregion
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
        public IActionResult JoinGame([FromBody] GameJoinModel model)
        {
            try
            {
                #region Validate

                //Check if Game Id is in wrong format
                if (!Guid.TryParse(model.GameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                //Check if game Id exixts.
                Guid gameId = Guid.Parse(model.GameId);
                var game = _gameService.GetGameById(gameId);
                if (game == null)
                    return BadRequest("Game not exist");

                //Check no of Players count
                if (_maxPlayersValue == game.PlayersJoined)
                    return BadRequest($"The maximum limit of players is {_maxPlayersValue}.");

                //Check if Player name already exixts
                if (_playerService.NameExists(model.PlayerName,gameId))
                    return BadRequest($"Player name : {model.PlayerName} already exists.");

                #endregion

                #region Function

                //Join Game
                var playerId = _playerService.AddPlayer(model.PlayerName, gameId);
                return Ok($"Player Id : {playerId}");

                #endregion
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
        public IActionResult StartGame([FromBody] GameIdModel model)
        {
            try
            {
                #region Validate

                //Check if game Id exixts.
                if (!Guid.TryParse(model.GameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                //Check if player Id exixts.
                if (!Guid.TryParse(model.PlayerId, out var _))
                    return BadRequest("Player Id is not given in correct format.");

                //Check if game Id exixts.
                Guid gameId = Guid.Parse(model.GameId);
                var game = _gameService.GetGameById(gameId);
                if (game == null)
                    return BadRequest("Game Id not exist");

                //Check if player Id exixts.
                Guid playerId = Guid.Parse(model.PlayerId);
                var player = _playerService.GetPlayer(playerId, gameId);
                if (player == null)
                    return BadRequest("player Id not exist");

                //Check no of Players count.
                if (game.PlayersJoined < _minPlayersValue)
                    return BadRequest($"The minimum players limit is {_minPlayersValue} to start the game.");

                #endregion

                #region Start Finite Statemachine

                _stateMachineService.PlayTurn(gameId, player);
                return Ok("Game started.");

                #endregion
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
        [HttpPost("GuessNextCard")]
        public IActionResult GuessNextCard([FromBody] GuessCardModel model)
        {
            try
            {
                #region Validate

                if (!Guid.TryParse(model.PlayerId, out var _))
                    return BadRequest("Player Id is not given in correct format.");

                if (!Guid.TryParse(model.GameId, out var _))
                    return BadRequest("Game Id is not given in correct format.");

                //Check if game Id exixts.
                Guid gameId = Guid.Parse(model.GameId);
                if (!_gameService.Validate(gameId))
                    return BadRequest("Game Id not exist");

                //Check if player Id exixts.
                Guid playerId = Guid.Parse(model.PlayerId);
                var player = _playerService.GetPlayer(playerId, gameId);
                if (player == null)
                    return BadRequest("player Id not exist");

                #endregion

                #region Play Next Turn

                player.Guess = model.Guess;
                _stateMachineService.PlayTurn(gameId, player);
                return Ok("You guess card.");

                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Notify the outcome after each turn.</summary>
        /// <response code="200">
        ///  Next Player Id.
        ///  Next PlayerName.
        ///  Last Card Played.
        ///  Result.
        /// </response>

        [HttpGet("NotifyResult")]
        public IActionResult NotifyResult(string gameId)
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

                //Get Turn outcome
                var result = _gameService.GetResult(_gameId);
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
