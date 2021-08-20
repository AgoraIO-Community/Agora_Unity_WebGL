using UnityEngine;
using UnityEngine.UI;

public class Logger {
    Text text;

    public Logger(Text text) {
        this.text = text;
    }

    public void UpdateLog(string logMessage) {
        Debug.Log(logMessage);
        string srcLogMessage = text.text;
        if (srcLogMessage.Length > 1000) {
            srcLogMessage = "";
        }
        srcLogMessage += "\r\n \r\n";
        srcLogMessage += logMessage;
        text.text = srcLogMessage;
    }

    public bool DebugAssert(bool condition, string message) {
        Debug.Assert(condition, message);
        if (!condition) {
            UpdateLog(message);
            return false;
        }
        return true;
    }
}