using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ShootBoxes.Constants
{
    public static class Enums
    {
        public enum Side
        {
            Left,
            Right
        }

        public enum GameScene
        {
            Menu,
            InGame,
            Pause
        }
    }

    public static class Strings
    {
        public const string Highscore = "HighScore";
    }

    public static class Tags
    {
        public const string BouncingSurface = "BouncingSurface";
        public const string Hittable = "Hittable";
    }

    public static class Layers
    {
    }

    public static class SceneNames
    {
    }

    public static class AnimatorParameters
    {
    }
}