using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public class ClientRevision
    {
        public List<ChangedObject> ChangedObjects = new List<ChangedObject>();

        public PlayerState PlayerState;

        public GameState GameState;

        public GameState ExpectedNextState;

        public bool StopUpdating = false;

        public GameSettings Settings;

        public int TurnNumber;

        private static DateTime UnixStartDate = new DateTime(1970, 1, 1);

        public double UTCTime => new TimeSpan(DateTime.UtcNow.Ticks - UnixStartDate.Ticks).TotalMilliseconds;

        public static ClientRevision EmptyRevision(PlayerState playerState, Game game)
        {
            return new ClientRevision(playerState, game, new ChangedObject() { Identifier = "NO_CHANGES", Object = "" });
        }

        public static ClientRevision StopUpdatingRevision(PlayerState playerState, Game game)
        {
            ClientRevision rev = EmptyRevision(playerState, game);
            rev.StopUpdating = true;
            return rev;
        }

        public ClientRevision(PlayerState playerState, Game game, params ChangedObject[] changedObjects)
        {
            PlayerState = playerState;
            GameState = game.CurrentState;
            ExpectedNextState = Game.ExpectedNextState(GameState);
            ChangedObjects.AddRange(changedObjects);
            Settings = game.Settings;
            TurnNumber = game.TurnNumber;
        }
    }

    public class ChangedObject
    {
        public string Identifier;
        public object Object;
    }

    public enum PlayerState
    {
        Observer = 1,
        Player = 2
    }
}
