#if UNITY_IPHONE || UNITY_STANDALONE_OSX

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IPHONE
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#endif

public class BL_BuildPostProcess
{

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
#if UNITY_IPHONE
            LinkLibraries(path);
            UpdatePermission(path + "/Info.plist");
#endif
        }
    }
#if UNITY_IPHONE
    public static void DisableBitcode(string projPath)
    {
        PBXProject proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));

        string target = GetTargetGuid(proj);
        proj.SetBuildProperty(target, "ENABLE_BITCODE", "false");
        File.WriteAllText(projPath, proj.WriteToString());
    }

    static string GetTargetGuid(PBXProject proj)
    {
#if UNITY_2019_3_OR_NEWER
        return proj.GetUnityFrameworkTargetGuid();
#else
        return proj.TargetGuidByName("Unity-iPhone");
#endif
    }

    static void LinkLibraries(string path)
    {
        // linked library
        const string defaultLocationInProj = "AgoraEngine/Plugins/iOS";
        string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);
        string target = GetTargetGuid(proj);
        // embedded frameworks
#if UNITY_2019_1_OR_NEWER
        target = proj.GetUnityMainTargetGuid();
#endif
        string pluginDir = path + "/Frameworks/AgoraEngine/Plugins/iOS/";
        var frameworkFilePaths = Directory.GetDirectories(pluginDir, "*.framework", SearchOption.TopDirectoryOnly);
        foreach (string fullpath in frameworkFilePaths)
        {
            string frameworkName = Path.Combine(defaultLocationInProj, Path.GetFileName(fullpath)); 
            string fileGuid = proj.AddFile(frameworkName, "Frameworks/" + frameworkName, PBXSourceTree.Sdk);
            PBXProjectExtensions.AddFileToEmbedFrameworks(proj, target, fileGuid);
        }
        proj.SetBuildProperty(target, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");

        // done, write to the project file
        File.WriteAllText(projPath, proj.WriteToString());
    }

#endif
    /// <summary>
    ///   Update the permission 
    /// </summary>
    /// <param name="pListPath">path to the Info.plist file</param>
    static void UpdatePermission(string pListPath)
    {
#if UNITY_IPHONE
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(pListPath));
        PlistElementDict rootDic = plist.root;
        var cameraPermission = "NSCameraUsageDescription";
        var micPermission = "NSMicrophoneUsageDescription";
        rootDic.SetString(cameraPermission, "Video need to use camera");
        rootDic.SetString(micPermission, "Voice call need to user mic");
        File.WriteAllText(pListPath, plist.WriteToString());
#endif
    }

}

#endif
