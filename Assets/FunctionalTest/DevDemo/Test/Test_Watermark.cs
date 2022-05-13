using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;


public class Test_Watermark : MonoBehaviour {

	public Text txtMsg;
	public InputField inpWidth;
	public InputField inpX;
	public InputField inpY;

	private int _width = 32;
	private int _x = 0;
	private int _y = 0;

	bool processInput()
    {
		if( inpWidth.text.Length > 0 )
        {
			_width = int.Parse(inpWidth.text);
        }
		else
        {
			txtMsg.text = "Invalid width input";
			return false;
        }

		if (inpX.text.Length > 0)
		{
			_x = int.Parse(inpX.text);
		}
		else
		{
			txtMsg.text = "Invalid X input";
			return false;
		}

		if (inpY.text.Length > 0)
		{
			_y = int.Parse(inpY.text);
		}
		else
		{
			txtMsg.text = "Invalid Y input";
			return false;
		}

		return true;
    }

	public void E_AddWaterMark_1()
	{
		if(processInput() == false)
        {
			return;
        }

		IRtcEngine engine = IRtcEngine.QueryEngine();

		RtcImage img = new RtcImage();
		img.url = "cat.png";
		img.height = _width;
		img.width = _width;
		img.x = _x;
		img.y = _y;

		engine.AddVideoWatermark(img);
		txtMsg.text = "Watermark started API 1";
	}

	public void E_AddWatermark_2()
	{
		if (processInput() == false)
		{
			return;
		}

		IRtcEngine engine = IRtcEngine.QueryEngine();

		WatermarkOptions wo = new WatermarkOptions();
		Rectangle rct = new Rectangle();
		rct.height = _width;
		rct.width = _width;
		rct.x = _x;
		rct.y = _y;
		wo.positionInLandscapeMode = rct;

		engine.AddVideoWatermark("tea.png", wo);
		txtMsg.text = "Watermark started API 2";
	}

	public void E_ClearVideoWaterMark()
	{
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.ClearVideoWatermarks();
		txtMsg.text = "Stopped watermark";
	}

	public void D_SetMirrorOptions_TRUE()
    {
		txtMsg.text = "Setting mirror options = TRUE";
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.SetMirrorApplied(true);
	}

	public void D_SetMirrorOptions_FALSE()
	{
		txtMsg.text = "Setting mirror options = FALSE";
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.SetMirrorApplied(false);
	}

}
