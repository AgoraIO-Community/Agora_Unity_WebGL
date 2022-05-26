using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDebug : MonoBehaviour
{
    public static GlobalDebug Instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        // keep this alive across scenes
        DontDestroyOnLoad(this.gameObject);
        //initEventOnEngineCallback
    }

    public static void Log(string msg )
    {
        Instance.longString += "\n" + msg;
    }

    // The position on of the scrolling viewport
    public Vector2 scrollPosition = Vector2.zero;
    bool show = false;
    // The string to display inside the scrollview. 2 buttons below add & clear this string.
    string longString = "";
    void OnGUI()
    {

        if (GUILayout.Button("?"))
            show = !show;

        if (show)
        {
            // Begin a scroll view. All rects are calculated automatically -
            // it will use up any available screen space and make sure contents flow correctly.
            // This is kept small with the last two parameters to force scrollbars to appear.
            scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, GUILayout.Width(400), GUILayout.Height(400));

            // We just add a single label to go inside the scroll view. Note how the
            // scrollbars will work correctly with wordwrap.
            GUILayout.Label(longString);

            // Add a button to clear the string. This is inside the scroll area, so it
            // will be scrolled as well. Note how the button becomes narrower to make room
            // for the vertical scrollbar
            if (GUILayout.Button("Clear"))
                longString = "";

            // End the scrollview we began above.
            GUILayout.EndScrollView();

        }
    }

}
