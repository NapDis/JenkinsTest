using UnityEngine;
using System.IO;
using System;
using BuildTools;

public class BuilderCommandline
{
    public static bool Build_SwitchEnv()
    {
        var config = LoadBuildConfigFromJson(ParseCommandLine());
        Builder.Config = config;
        return Builder.SwitchBuildEnvironment();
    }

    public static bool Build_MakePackage()
    {
        var config = LoadBuildConfigFromJson(ParseCommandLine());
        Builder.Config = config;
        return Builder.MakePackage();
    }

    static string ParseCommandLine()
    {
        string configFilePath = null;

        string[] strs = System.Environment.GetCommandLineArgs();
        foreach (var s in strs)
        {
            if (s.Contains("-configFilePath:"))
            {
                int sidx = s.IndexOf(":");
                if (sidx >= 0)
                {
                    string arg = s.Substring(sidx + 1);
                    configFilePath = arg;
                }
            }
        }

        return configFilePath;
    }

    public static BuildConfig LoadBuildConfigFromJson(string jsonPath)
    {
        BuildConfig config = default;

        if (File.Exists(jsonPath))
        {
            try
            {
                string dataRead = "";
                using (FileStream stream = new FileStream(jsonPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataRead = reader.ReadToEnd();
                    }
                }

                config = JsonUtility.FromJson<BuildConfig>(dataRead);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load build config from file {jsonPath}. Exception:{e}");
            }
        }

        return config;
    }
}
