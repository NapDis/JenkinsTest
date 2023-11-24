using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BuildTools
{
    public enum BuildOS
    {
        PC,
        Android
    }
    public enum BuildStore
    {
        Default,
        Google,
    }
    public enum VersionType
    {
        Debug,
        Beta,
        Release
    }
    public class BuildConfig
    {
        public string AppName = "Domino";
        public BuildOS OS = BuildOS.Android;
        public BuildStore Store = BuildStore.Default;
        public string Version = "0.0.1";
        public VersionType VersionType = VersionType.Debug;

        public string outputFolder;
        public string buildTime;
        
    }
}

