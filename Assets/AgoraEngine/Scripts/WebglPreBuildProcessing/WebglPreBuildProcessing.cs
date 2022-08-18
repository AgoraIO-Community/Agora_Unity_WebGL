#if UNITY_2019_1_OR_NEWER && UNITY_EDITOR_OSX
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

// This script is used for building WebGL on MacOS 12.3 or newer, where Apple took out /usr/bin/python
public class WebglPreBuildProcessing : IPreprocessBuildWithReport
{
    public readonly string PythonPath = "/usr/local/bin/python3";
    public int callbackOrder { get { return 1; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        System.Environment.SetEnvironmentVariable("EMSDK_PYTHON", PythonPath);
    }
}
#endif