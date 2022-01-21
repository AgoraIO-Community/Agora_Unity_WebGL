using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleStateButton : MonoBehaviour
{
    public bool OnOffState { get; private set; }
    public string OnStateText { get; private set; }
    public string OffStateText { get; private set; }

    private Button button;
    private Text text;

    public void Setup(bool initOnOff, string onStateText, string offStateText, System.Action callOnAction, System.Action callOffAction)
    {
        button = GetComponent<Button>();
        text = button.GetComponentInChildren<Text>();
        OnOffState = initOnOff;
        OnStateText = onStateText;
        OffStateText = offStateText;
        text.text = OnOffState ? OffStateText : OnStateText;

        button.onClick.AddListener(() =>
        {
            OnOffState = !OnOffState;
            text.text = OnOffState ? OffStateText : OnStateText;
            if (OnOffState)
            {
                callOnAction();
            }
            else
            {
                callOffAction();
            }
        });
    }

}
