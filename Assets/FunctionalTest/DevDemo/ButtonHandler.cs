using UnityEngine;
using UnityEngine.UI;

namespace agora_gs_test
{
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
                    gameController.onJoinButtonClicked();
                }
                else if (button.name == "LeaveButton")
                {
                    gameController.onLeaveButtonClicked();
                }
            }
        }
    }
}
