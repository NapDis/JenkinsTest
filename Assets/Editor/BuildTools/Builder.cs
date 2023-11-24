using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace BuildTools
{
    public class Builder
    {
        public static BuildConfig Config;
        public static string _outputPathExe;
        public static bool SwitchBuildEnvironment()
        {
            var targetGroup = getBuildTargetGroup();
            SwitchScriptDefines(targetGroup);

            string version = $"{Config.Version}";
            PlayerSettings.bundleVersion = version ;
            PlayerSettings.productName = Config.AppName;
            return BuildPlayer();
        }
        public static bool MakePackage()
        {
            string version = $"{Config.Version}";
            PlayerSettings.bundleVersion = version;
            PlayerSettings.productName = Config.AppName;
            return BuildPlayer();

        }
        /// <summary>
        /// ����Config���÷���ƽ̨
        /// </summary>
        /// <returns></returns>
        public static BuildTargetGroup getBuildTargetGroup()
        {
            BuildTargetGroup buildTargetGroup = BuildTargetGroup.Standalone;
            switch (Config.OS)
            {
                case BuildOS.PC:
                    buildTargetGroup = BuildTargetGroup.Standalone;
                    break;
                case BuildOS.Android:
                    buildTargetGroup = BuildTargetGroup.Android;
                    break;
            }
            return buildTargetGroup;
        }
        public static BuildTarget getBuildTarget()
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
            switch (Config.OS)
            {
                case BuildOS.PC:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    break;
                case BuildOS.Android:
                    buildTarget = BuildTarget.Android;
                    break;
            }
            return buildTarget;
        }
        /// <summary>
        /// ����Ŀ��ƽ̨ ����Ԥ������
        /// </summary>
        /// <param name="targetGroup">Ŀ��ƽ̨</param>
        public static void SwitchScriptDefines(BuildTargetGroup targetGroup)
        {
            //����targetGroup��ȡ��ǰƽ̨���е�symbols����Ԥ������
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            //��ȡ����symbols�Ƿֺŷָ���ַ���,��˽��ַ���split������
            var defines = symbols.Split(new char[] { ';' });

            HashSet<string> hashset = new HashSet<string>(defines);

            //����Config��OS�����������ã���������ƶ�ƽ̨�����MOBILE_UI�ꡣ
            switch (Config.OS)
            {
                case BuildOS.Android:
                    hashset.Add("MOBILE_UI");
                    break;
                default:
                    break;
            }

            //����Config��Store��������Բ�ͬ���̵궨�岻ͬ�ĺ꣬��ȡȥ��һЩ�޹صĺ�
            var store = Config.Store;
             if (store == BuildStore.Google)
            {

                if (!hashset.Contains("PLATFORM_GOOGLE"))
                {
                    hashset.Add("PLATFORM_GOOGLE");
                }
            }
            else if (store == BuildStore.Default)
            {
                if (hashset.Contains("PLATFORM_GOOGLE"))
                {
                    hashset.Remove("PLATFORM_GOOGLE");
                }
            }

            //��hashset������string����
            defines = new string[hashset.Count];
            hashset.CopyTo(defines);

            //���targetGroup�������޸ĺ�� defines
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
        }
        static void PrepareBuildPath()
        {
            var _outputFolder = Config.outputFolder;

            if (!System.IO.Directory.Exists(_outputFolder))
            {
                System.IO.Directory.CreateDirectory(_outputFolder);
            }

            string folderName = $"{Config.AppName}_{Config.Version}_{Config.VersionType}";
            if (Config.Store != BuildStore.Default)
            {
                folderName = $"{folderName}_{Config.Store}";
            }

            var _outputPath = System.IO.Path.Combine(_outputFolder, folderName);
            Debug.LogError("Error!!!:"+_outputPath);
            //var _outputPath = "D:/Project/builds_JenkinsTest/JenkinsTest_0.1.0";

            switch (Config.OS)
            {
                case BuildOS.PC:
                    if (System.IO.Directory.Exists(_outputPath))
                    {
                        System.IO.Directory.Delete(_outputPath, true);
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(_outputPath);
                    }
                    _outputPathExe = System.IO.Path.Combine(_outputPath, $"{Config.AppName}.exe");
                    break;
                case BuildOS.Android:
                    if (System.IO.Directory.Exists(_outputPath))
                    {
                        System.IO.Directory.Delete(_outputPath, true);
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(_outputPath);
                    }
                    _outputPathExe = System.IO.Path.Combine(_outputPath, $"{Config.AppName}.apk");
                    break;
            }
        }
        static string[] getBuildScene()
        {
            List<string> scenes = new List<string>();
            EditorBuildSettingsScene[] scene = EditorBuildSettings.scenes;
            for (int i = 0; i < scene.Length; i++)
            {
                scenes.Add(scene[i].path);
            }
            return scenes.ToArray();
        }
        static bool BuildPlayer()
        {
            PrepareBuildPath();

            BuildPlayerOptions playerOptions = new BuildPlayerOptions();
            playerOptions.locationPathName = _outputPathExe;
            playerOptions.options = BuildOptions.None;
            playerOptions.target = getBuildTarget();
            playerOptions.targetGroup = getBuildTargetGroup();
            playerOptions.scenes = getBuildScene();

            BuildReport report = BuildPipeline.BuildPlayer(playerOptions);
            BuildResult result = report.summary.result;
            return result == BuildResult.Succeeded;
        }
    }
}

