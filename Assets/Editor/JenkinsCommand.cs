using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Linq;


public static class Builder
{
    public static void BuildGame ()
    {
        string platform = CommandLineTool.GetEnvironmentVariable(CommandArgsName.PLATFORM);
        string gameName = CommandLineTool.GetEnvironmentVariable(CommandArgsName.GAME_NAME);
        
        string path = Application.dataPath.Replace("/Assets", $"/Build{platform}");
        switch(platform)
        {
            case "Windows":
                BuildGameWindows(path, gameName);
                break;
            case "Android":
                BuildGameAndroid(path, gameName);
                break;
            case "Html":
                BuildGameHtml(path, gameName);
                break;
        }
    }

    [MenuItem("MyTools/Windows Build With Postprocess")]
    public static void BuildGameWindows(string path, string gameName)
    {
        string[] levels = new string[] {"Assets/Scenes/GameCore.unity"};
        BuildPipeline.BuildPlayer(levels, path + $"/{gameName}.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    [MenuItem("MyTools/Android Build With Postprocess")]
    public static void BuildGameAndroid(string path, string gameName)
    {
        string[] levels = new string[] {"Assets/Scenes/GameCore.unity"};
        BuildPipeline.BuildPlayer(levels, path + $"/{gameName}.apk", BuildTarget.Android, BuildOptions.None);
    }
    
    [MenuItem("MyTools/Html Build With Postprocess")]
    public static void BuildGameHtml(string path, string gameName)
    {
        string[] levels = new string[] {"Assets/Scenes/GameCore.unity"};
        BuildPipeline.BuildPlayer(levels, path + $"/{gameName}.html", BuildTarget.WebGL, BuildOptions.None);
    }
    
    [MenuItem("MyTools/Mac Build With Postprocess")]
    public static void BuildGameMac(string path, string gameName)
    {
        string[] levels = new string[] {"Assets/Scenes/GameCore.unity"};
        BuildPipeline.BuildPlayer(levels, path + $"/{gameName}.apk", BuildTarget.iOS, BuildOptions.None);
        //触发
    }

    //SVN获取版本号
    [MenuItem("MyTools/GetSVNVersion")]
    public static string GetSVNVersion()
    {
        Process procUpdate = new Process();
        procUpdate.StartInfo.FileName = "svn";
        procUpdate.StartInfo.Arguments = "update";
        procUpdate.StartInfo.UseShellExecute = false;
        procUpdate.StartInfo.RedirectStandardOutput = true;
        procUpdate.Start();
        procUpdate.WaitForExit();
        
        Process proc = new Process();
        proc.StartInfo.FileName = "svn";
        proc.StartInfo.Arguments = "info";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.Start();
        string output = proc.StandardOutput.ReadToEnd();
        
        UnityEngine.Debug.Log(output);
        //从output中取出版本号
        // 格式如下
        //     Working Copy Root Path: D:\Project_Game_Dev\2022-GridGuiderPure
        // URL: svn://gingerbreadstudio.cc/GridGuider/trunk
        // Relative URL: ^/trunk
        //     Repository Root: svn://gingerbreadstudio.cc/GridGuider
        // Repository UUID: 414fbde1-8cfe-42c7-8eb2-43e1e2b8bc4d
        // Revision: 2
        // Node Kind: directory
        // Schedule: normal
        //     Last Changed Author: c14sama
        //     Last Changed Rev: 2
        // Last Changed Date: 2024-12-01 06:27:00 +0800 (??, 01 12? 2024)

        List<string> info = output.Split('\n').ToList();
        string finalRevision = "0";
        info.ForEach((string line) => {
            if (line.Contains("Revision:")) {
                string[] revision = line.Split(':');
                finalRevision = revision[1].Trim();
            }
        });

        proc.WaitForExit();
        UnityEngine.Debug.Log(finalRevision);
        return finalRevision;
    }
}



public enum CommandArgsName
{
    PLATFORM,
    GAME_NAME
}
public static class CommandLineTool
{
    private static Dictionary<CommandArgsName, string> dicCommandArgsName = new Dictionary<CommandArgsName, string>();
    public static string GetEnvironmentVariable(CommandArgsName commandArgsName)
    {
        return dicCommandArgsName.ContainsKey(commandArgsName) ? dicCommandArgsName[commandArgsName] :
            System.Environment.GetEnvironmentVariable(commandArgsName.ToString()) ?? string.Empty;
    }
 
    public static bool HasCommandArgs(CommandArgsName commandArgsName)
    {
        var value = GetEnvironmentVariable(commandArgsName);
        return !(string.IsNullOrEmpty(value) || string.Compare(value, "false", true) == 0);
    }
}