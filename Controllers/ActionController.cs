using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BuildingWebsite.Server;
using BuildingWebsite.Server.ClientObjects;
using BuildingWebsite.Server.ServerObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ActionController : ControllerBase
    {
        private static CookieOptions StandardCookieOptions = new CookieOptions() { SameSite = SameSiteMode.Strict, HttpOnly = true };

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> Authenticate(string user, string password)
        {
            //user = Uri.UnescapeDataString(user);
            //password = Uri.UnescapeDataString(password);
            User player;
            if (!Server.User.Users.ContainsKey(user))
            {
                //for now, this is the user creation workflow
                Server.User.CreatePlayer(user);
                // return base.NotFound();
            }
            if (password != "please") //todo: make actual password system
            {
                return Unauthorized();
            }
            player = Server.User.Users[user];
            string session = Server.User.GetSession();
            player.SessionId = session;
            Response.Cookies.Append(Constants.COOKIE_SESSION_ID, session, StandardCookieOptions);
            return true;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<GameInfoContainer> CreateGame()
        {
            if (!Request.Cookies.ContainsKey(Constants.COOKIE_SESSION_ID))
            {
                return Unauthorized();
            }
            string sessionId = Request.Cookies[Constants.COOKIE_SESSION_ID];
            if (!Server.User.Sessions.ContainsKey(sessionId))
            {
                return Unauthorized();
            }
            User user = Server.User.Sessions[sessionId];
            return new GameInfoContainer(Game.CreateGame(user));
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GameJoinStatus> RequestJoinGame(int gameId)
        {
            if (!Request.Cookies.ContainsKey(Constants.COOKIE_SESSION_ID))
            {
                return Unauthorized();
            }
            string sessionId = Request.Cookies[Constants.COOKIE_SESSION_ID];
            if (!Server.User.Sessions.ContainsKey(sessionId))
            {
                return Unauthorized();
            }
            User player = Server.User.Sessions[sessionId];
            if (!Game.Games.ContainsKey(gameId))
            {
                return NotFound();
            }
            Game game = Game.Games[gameId];
            return game.TryJoin(player);
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ClientRevision> LoadGame(int gameId)
        {
            if (!Request.Cookies.ContainsKey(Constants.COOKIE_SESSION_ID))
            {
                return Unauthorized();
            }
            string sessionId = Request.Cookies[Constants.COOKIE_SESSION_ID];
            if (!Server.User.Sessions.ContainsKey(sessionId))
            {
                return Unauthorized();
            }
            User player = Server.User.Sessions[sessionId];
            if (!Game.Games.ContainsKey(gameId))
            {
                return NotFound();
            }
            Game game = Game.Games[gameId];
            GameView view = game.GetGameView(player);
            if (view == null)
            {
                return Unauthorized();
            }
            Response.Cookies.Delete(Constants.COOKIE_LAST_REV);
            Response.Cookies.Append(Constants.COOKIE_LAST_REV, view.LatestRevisionNumber, StandardCookieOptions);
            return view.GetComprehensiveRevision();
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GameSettings> LoadGameSettings(int gameId)
        {
            if (!TryGetUserSession(Request, out User user))
            {
                return Unauthorized();
            }
            if (!TryGetGame(gameId, out Game game))
            {
                return NotFound();
            }
            //Check view to make sure the user's authorized to be in this game
            if (!TryGetGameView(game, user, out GameView view))
            {
                return Unauthorized();
            }
            return game.Settings;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Dictionary<string, object>> GetConstants()
        {
            Type constantsType = typeof(Constants);
            FieldInfo[] consts = constantsType.GetFields(BindingFlags.Public | BindingFlags.Static);
            Dictionary<string, object> ret = new Dictionary<string, object>();
            foreach (FieldInfo c in consts) {
                ret.Add(c.Name, c.GetValue(null));
            }
            return ret;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Dictionary<string, Dictionary<string, int>>> GetEnums()
        {
            Type[] enums = new Type[]
            {
                typeof(PlayerState),
                typeof(GameState),
                typeof(GameJoinStatus),
                typeof(MaterialType),
                typeof(ExecutiveTaskType),
                typeof(ModalType),
                typeof(ExecutiveAbility),
                typeof(OpportunityType),
                typeof(IntrigueType),
                typeof(CardType),
                typeof(ProjectType),
                typeof(ProjectTier)
            };
            Dictionary<string, Dictionary<string, int>> ret = new Dictionary<string, Dictionary<string, int>>();
            foreach (Type enumT in enums)
            {
                Dictionary<string, int> enumD = new Dictionary<string, int>();
                foreach (int value in Enum.GetValues(enumT))
                {
                    enumD.Add(Enum.GetName(enumT, value), value);
                }
                ret.Add(enumT.Name, enumD);
            }
            return ret;
        }

        private bool TryGetUserSession(HttpRequest request, out User user)
        {
            user = null;
            if (!request.Cookies.ContainsKey(Constants.COOKIE_SESSION_ID))
            {
                return false;
            }
            string sessionId = request.Cookies[Constants.COOKIE_SESSION_ID];
            if (!Server.User.Sessions.ContainsKey(sessionId))
            {
                return false;
            }
            user = Server.User.Sessions[sessionId];
            return true;
        }

        private bool TryGetGame(int gameId, out Game game)
        {
            game = null;
            if (!Game.Games.ContainsKey(gameId))
            {
                return false;
            }
            game = Game.Games[gameId];
            return true;
        }

        private bool TryGetGameView(Game game, User user, out GameView view)
        {
            view = game.GetGameView(user);
            if (view == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get the latest revision for a given view
        /// </summary>
        /// <param name="view"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns>Updated revision or BadRequest if the data supplied is insufficient</returns>
        private ActionResult<ClientRevision> TryGetUpdatedRevision(GameView view, HttpRequest request, HttpResponse response)
        {
            int latestRevision;
            if (!request.Cookies.ContainsKey(Constants.COOKIE_LAST_REV))
            {
                latestRevision = 0;
            }
            else
            {
                latestRevision = int.Parse(request.Cookies[Constants.COOKIE_LAST_REV]);
            }
            ClientRevision rev = null;
            try
            {
                rev = view.GetRevision(latestRevision);
            }
            catch
            {
                return BadRequest("BadLatestRevision");
            }
            finally
            {
                response.Cookies.Append(Constants.COOKIE_LAST_REV, view.LatestRevisionNumber, StandardCookieOptions);
            }
            return rev;
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ClientRevision> Update(int gameId)
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
            if (!Game.Games.ContainsKey(gameId))
            {
                return NotFound();
            }
            Game game = Game.Games[gameId];
            GameView view = game.GetGameView(player);
            PlayerState state = view.PlayerState;
            int latestRevision;
            if (!Request.Cookies.ContainsKey(Constants.COOKIE_LAST_REV))
            {
                latestRevision = 0;
            }
            else
            {
                latestRevision = int.Parse(Request.Cookies[Constants.COOKIE_LAST_REV]);
            }
            for (int i = 0; i < 600; i++)
            {
                view = game.GetGameView(player);
                if (view == null)
                {
                    return Unauthorized();
                }
                if (state != view.PlayerState)
                {
                    return ClientRevision.StopUpdatingRevision(view.PlayerState, game);
                }
                if (view.HasNewRevision(latestRevision))
                {
                    try
                    {
                        return view.GetRevision(latestRevision);
                    }
                    catch
                    {
                        return BadRequest("BadLatestRevision");
                    }
                    finally
                    {
                        //note: this gets executed after the return evaluates but before it's actually returned
                        Response.Cookies.Append(Constants.COOKIE_LAST_REV, view.LatestRevisionNumber, StandardCookieOptions);
                    }
                }
                Thread.Sleep(100);
            }
            view = game.GetGameView(player);
            if (view == null)
            {
                return Unauthorized();
            }
            try
            {
                return view.GetRevision(latestRevision);
            }
            catch
            {
                return BadRequest("BadLatestRevision");
            }
            finally
            {
                //note: this gets executed after the return evaluates but before it's actually returned
                Response.Cookies.Append(Constants.COOKIE_LAST_REV, view.LatestRevisionNumber, StandardCookieOptions);
            }
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ClientRevision> TryBecomePlayer(int gameId)
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
            if (!Game.Games.ContainsKey(gameId))
            {
                return NotFound();
            }
            Game game = Game.Games[gameId];
            GameView view = game.TryBecomePlayer(player);
            if (view == null)
            {
                return Unauthorized();
            }
            return view.GetComprehensiveRevision();
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> ReadyAdvance(ReadyAdvanceArgs clientArgs)
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
            game.ReadyAdvance(player, clientArgs.expectedNextState);
            GameView view = game.GetGameView(player);
            if (view == null)
            {
                return Unauthorized();
            }
            return true;
        }

        public class ReadyAdvanceArgs
        {
            public int gameId { get; set; }

            public GameState expectedNextState { get; set; }
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> UnreadyAdvance(int gameId)
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
            game.UnreadyAdvance(user);
            return true;
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> CreateChat(int gameId, string name, string playerIds)
        {
            name = Uri.UnescapeDataString(name);
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
            string[] idStrArr = playerIds.Split(',');
            if (idStrArr.Length == 0)
            {
                return BadRequest("Malformed player IDs.");
            }
            int[] idArr = new int[idStrArr.Length];
            for (int i = 0; i < idStrArr.Length; i++)
            {
                if (int.TryParse(idStrArr[i], out int id))
                {
                    idArr[i] = id;
                }
                else
                {
                    return BadRequest("Malformed player IDs.");
                }
            }
            game.TryCreateChat(user, name, idArr);
            return true;
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> EnterChatLine(int gameId, int chatId, string entry)
        {
            //entry = Uri.UnescapeDataString(entry);
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
            if (!game.EnterChatLine(user, chatId, entry))
            {
                return BadRequest("Chat ID was invalid in general or for this user.");
            }
            return true;
        }

        [HttpGet("{gameId:int}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> ClearModal(int gameId, GameState phase, ModalType type)
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
            game.ClearModal(user, phase, type);
            return true;
        }
    }

    public class GameInfoContainer
    {
        public int Id { get; private set; }
        
        public GameInfoContainer(int id)
        {
            Id = id;
        }
    }
}
