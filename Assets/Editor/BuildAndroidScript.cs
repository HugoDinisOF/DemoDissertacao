using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.IO;

// Output the build size or a failure depending on BuildPlayer.

public class BuildAndroidScript : MonoBehaviour
{
    [MenuItem("Build/Build Android")]
    public static void MyBuild()
    {
        Directory.CreateDirectory("TempBuildFiles");

        FileUtil.MoveFileOrDirectory("./Packages/com.unity.xr.core-utils~", "./TempBuildFiles/com.unity.xr.core-utils~");
        FileUtil.MoveFileOrDirectory("./Packages/com.unity.xr.interaction.toolkit~", "./TempBuildFiles/com.unity.xr.interaction.toolkit~");
        FileUtil.MoveFileOrDirectory("./Packages/com.unity.xr.legacyinputhelpers~", "./TempBuildFiles/com.unity.xr.legacyinputhelpers~");
        FileUtil.MoveFileOrDirectory("./Packages/com.unity.xr.management~", "./TempBuildFiles/com.unity.xr.management~");
        FileUtil.MoveFileOrDirectory("./Packages/com.unity.xr.oculus~", "./TempBuildFiles/com.unity.xr.oculus~");
        FileUtil.MoveFileOrDirectory("./Packages/com.unity.xr.openxr~", "./TempBuildFiles/com.unity.xr.openxr~");

        FileUtil.MoveFileOrDirectory("./Assets/Samples/XR Interaction Toolkit", "./TempBuildFiles/XR Interaction Toolkit");


        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/MainMenu.unity" };
        buildPlayerOptions.locationPathName = "Temp";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }

        FileUtil.MoveFileOrDirectory("./TempBuildFiles/com.unity.xr.core-utils~", "./Packages/com.unity.xr.core-utils~");
        FileUtil.MoveFileOrDirectory("./TempBuildFiles/com.unity.xr.interaction.toolkit~", "./Packages/com.unity.xr.interaction.toolkit~");
        FileUtil.MoveFileOrDirectory("./TempBuildFiles/com.unity.xr.legacyinputhelpers~", "./Packages/com.unity.xr.legacyinputhelpers~");
        FileUtil.MoveFileOrDirectory("./TempBuildFiles/com.unity.xr.management~", "./Packages/com.unity.xr.management~");
        FileUtil.MoveFileOrDirectory("./TempBuildFiles/com.unity.xr.oculus~", "./Packages/com.unity.xr.oculus~");
        FileUtil.MoveFileOrDirectory("./TempBuildFiles/com.unity.xr.openxr~", "./Packages/com.unity.xr.openxr~");

        FileUtil.MoveFileOrDirectory("./TempBuildFiles/XR Interaction Toolkit", "./Assets/Samples/XR Interaction Toolkit");
    }
}