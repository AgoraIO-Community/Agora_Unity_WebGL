using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{

    /// <summary>
    ///   React to a button click event.  Used in the UI Button action definition.
    /// </summary>
    /// <param name="button"></param>
    public void onButtonClicked(Button button)
    {
        // which GameObject?
        GameObject go = GameObject.Find("GameController");
        if (go != null)
        {
            TestHome gameController = go.GetComponent<TestHome>();
            if (gameController == null)
            {
                Debug.LogError("Missing game controller...");
                return;
            }
            if (button.name == "JoinButton")
            {
                gameController.onJoinButtonClicked(true);
            }
            else if (button.name == "JoinButton2")
            {
                gameController.onJoinButtonClicked(false);
            }
            else if (button.name == "JoinButton3")
            {
                gameController.onJoinButtonClicked(true, true);
            }
            else if (button.name == "LeaveButton")
            {
                gameController.onLeaveButtonClicked();
            }
            else if (button.name == "JoinButton4")
            {
                gameController.onJoinAudience();
            }
        }
    }
}
