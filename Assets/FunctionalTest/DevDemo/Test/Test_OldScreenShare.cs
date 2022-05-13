using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

public class Test_OldScreenShare : MonoBehaviour {

	public void StartScreenSharing()
    {
        IRtcEngine engine = IRtcEngine.QueryEngine();

        Rectangle rt = new Rectangle();
        rt.x = 0;
        rt.y = 0;
        rt.width = 100;
        rt.height = 100;
        ScreenCaptureParameters scp = new ScreenCaptureParameters();
        scp.captureMouseCursor = true;
        scp.bitrate = 100;
        scp.frameRate = 30;
        scp.dimensions.height = 100;
        scp.dimensions.width = 100;

        engine.StartScreenCaptureByDisplayId(1, rt, scp);
    }

    public void StopScreenSharing()
    {
        IRtcEngine engine = IRtcEngine.QueryEngine();
        engine.StopScreenCapture();
    }

}
