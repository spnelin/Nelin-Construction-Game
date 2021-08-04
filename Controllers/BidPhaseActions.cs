using BuildingWebsite.Server;
using BuildingWebsite.Server.ServerObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Controllers
{
    public partial class ActionController
    {

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> SubmitBid(int gameId, int bidQuantity, int expectedBidIndex)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(gameId, out Game game))
            {
                return NotFound();
            }
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            game.SubmitBidForProject(user, bidQuantity, expectedBidIndex);
            return true;
        }
    }
}
