using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VisionBoxGame.Api.Tests;
using visonBoxGame;
using visonBoxGame.Models;
using Xunit;

namespace VisionBoxGame.Api.Tests
{
    [Collection("Integration")]
    public class StartGameTests
    {
        private readonly HttpClient _client;

        public StartGameTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task CreateGame_AddPlayer_JoinGame_StartGame()
        {
            //Create Game.

            GameBindingModel gameBindingModel = new GameBindingModel
            {
                 PlayerName ="Sohaib Alvi",
                 Title ="Alvi Games"
            };

            GameIdModel gameIdModel;
            using (var postCreateGameResponse = await _client.PostAsJsonAsync($"/api/Game/CreateGame", gameBindingModel))
            {
                Assert.True(postCreateGameResponse.IsSuccessStatusCode);
                gameIdModel = await postCreateGameResponse.Content.ReadAsAsync<GameIdModel>();
            }

            //Join Game

            GameJoinModel gameJoinModel = new GameJoinModel
            {
                GameId = gameIdModel.GameId,
                PlayerName = "Saad Alvi",
            };

            bool flagJoinGame = false;
            using (var postJoinGameResponse = await _client.PostAsJsonAsync($"/api/Game/JoinGame", gameJoinModel))
            {
                Assert.True(postJoinGameResponse.IsSuccessStatusCode);
                flagJoinGame = await postJoinGameResponse.Content.ReadAsAsync<bool>();
            }

            //Start Game

            bool flagStartGame = false;
            using (var postStartGameResponse = await _client.PostAsJsonAsync($"/api/Game/StartGame", gameIdModel))
            {
                Assert.True(postStartGameResponse.IsSuccessStatusCode);
                flagStartGame = await postStartGameResponse.Content.ReadAsAsync<bool>();
            }
        }
    }
}
