using BuildingWebsite.Server;
using BuildingWebsite.Server.ClientObjects;
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
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> BuyMaterials(BuyMaterialsArgs args)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(args.gameId, out Game game))
            {
                return NotFound();
            }
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            game.BuyMaterials(user, args.resType, args.quantity);
            return true;
        }

        public class BuyMaterialsArgs
        {
            public int gameId { get; set; }

            public MaterialType resType { get; set; }

            public int quantity { get; set; }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> BuyMaterialDeal(BuyMaterialDealArgs args)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(args.gameId, out Game game))
            {
                return NotFound();
            }
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            game.BuyMaterialDeal(user, args.quantity);
            return true;
        }

        public class BuyMaterialDealArgs
        {
            public int gameId { get; set; }

            public int quantity { get; set; }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> HireWorkers(HireWorkersArgs args)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(args.gameId, out Game game))
            {
                return NotFound();
            }
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            game.TryHireWorkers(user, args.quantity);
            return true;
        }

        public class HireWorkersArgs
        {
            public int gameId { get; set; }

            public int quantity { get; set; }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> HireExecutive(HireExecutiveArgs args)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(args.gameId, out Game game))
            {
                return NotFound();
            }
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            game.HireExecutive(user, args.executiveIndex);
            return true;
        }

        public class HireExecutiveArgs
        {
            public int gameId { get; set; }

            public int executiveIndex { get; set; }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> FindOpportunity(FindOpportunityArgs args)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(args.gameId, out Game game))
            {
                return NotFound();
            }
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            game.FindOpportunity(user, args.opportunityIndex);
            return true;
        }

        public class FindOpportunityArgs
        {
            public int gameId { get; set; }

            public int opportunityIndex { get; set; }
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> ConstructProject(int gameId, int projIndex, int concrete, int steel, int glass)
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
            game.AddMaterialsToProject(user, projIndex, concrete, steel, glass);
            return true;
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> ViewCostReport(int gameId)
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
            game.ViewCostReport(user);
            return true;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> AssignExecutive(AssignExecutiveArgs clientArgs)
        {
            if (!Request.Cookies.ContainsKey(Constants.COOKIE_SESSION_ID))
            {
                return Unauthorized();
            }
            string sessionId = Request.Cookies[Constants.COOKIE_SESSION_ID];
            if (!Server.User.Sessions.ContainsKey(sessionId))
            {
                return base.Unauthorized();
            }
            User player = Server.User.Sessions[sessionId];
            if (!Game.Games.ContainsKey(clientArgs.gameId))
            {
                return NotFound();
            }
            Game game = Game.Games[clientArgs.gameId];
            GameView view = game.GetGameView(player);
            if (view == null)
            {
                return Unauthorized();
            }
            game.AssignExecutive(player, clientArgs.executiveIndex, clientArgs.task);
            return true;
        }

        public class AssignExecutiveArgs
        {
            public int gameId { get; set; }

            public int executiveIndex { get; set; }

            public ExecutiveTaskType task { get; set; }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> PlayCard(PlayCardArgs args)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(args.gameId, out Game game))
            {
                return NotFound();
            }
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            game.PlayCard(user, args.cardIndex);
            return true;
        }

        public class PlayCardArgs
        {
            public int gameId { get; set; }

            public int cardIndex { get; set; }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> FireExecutive(FireExecutiveArgs args)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(args.gameId, out Game game))
            {
                return NotFound();
            }
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            game.FireExecutive(user, args.executiveIndex);
            return true;
        }

        public class FireExecutiveArgs
        {
            public int gameId { get; set; }

            public int executiveIndex { get; set; }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> FireWorkers(FireWorkersArgs args)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(args.gameId, out Game game))
            {
                return NotFound();
            }
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            game.FireWorkers(user, args.workersToFire);
            return true;
        }

        public class FireWorkersArgs
        {
            public int gameId { get; set; }

            public int workersToFire { get; set; }
        }
    }
}
