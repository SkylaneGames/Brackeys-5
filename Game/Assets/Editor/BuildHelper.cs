using UnityEditor;

class BuildHelper
{
    static void PerformBuild()
    {
        string[] scenes = { "Assets/Scenes/Splash.unity","Assets/Scenes/Menu.unity","Assets/Scenes/Game.unity" };
        BuildPipeline.BuildPlayer(scenes, "../build", BuildTarget.WebGL, BuildOptions.None);
    }
}