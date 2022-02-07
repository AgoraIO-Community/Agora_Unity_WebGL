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
        UpdateText();

        button.onClick.AddListener(() =>
        {
            OnOffState = !OnOffState;
            UpdateText();
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

    public void SetState(bool onOffState)
    {
        OnOffState = onOffState;
        UpdateText();
    }

    void UpdateText()
    {
        if (text != null)
        {
            text.text = OnOffState ? OffStateText : OnStateText;
        }
    }
}
