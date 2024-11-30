using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSpyGame
{
    public static class SystemEvents
    {
        public static Action MoveToNextLevelStart = delegate { };
        public static Action MoveToTitleScreenStart = delegate { };
        public static Action ScreenFadeOutDone = delegate { };
        public static Action LevelLoaded = delegate { };
        public static Action ExitGameSeqStart = delegate { };
        public static Action FadeOutForGameExitDone = delegate { };
    }
}