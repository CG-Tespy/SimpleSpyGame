using System;

namespace SimpleSpyGame
{
    public static class StageEvents
    {
        public static Action PlayerCaught = delegate { };
        public static Action DocRetrieved = delegate { };

        public static Action PlayerWon = delegate { };
        public static Action PlayerLost = delegate { };
    }
}