using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildTool
{

    [MenuItem("Build/Build APK")]
    public static void BuildAPK()
    {

        // 解析命令行参数
        string[] args = System.Environment.GetCommandLineArgs();
        foreach (var s in args)
        {
            if (s.Contains("--productName:"))
            {
                string productName = s.Split(':')[1];
                // 设置app名字
                PlayerSettings.productName = productName;
            }

            if (s.Contains("--version:"))
            {
                string version = s.Split(':')[1];
                // 设置版本号
                PlayerSettings.bundleVersion = version;
            }
        }

        BuildPlayerOptions bpo = new BuildPlayerOptions();
        bpo.scenes = new string[] { "Assets/Scenes/SampleScene.unity" };
        bpo.locationPathName = Application.dataPath + "/../BuildedApk/test.apk";
        bpo.target = BuildTarget.Android;
        bpo.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(bpo);

        Debug.Log("BuildAPK is Done");
    }
}
