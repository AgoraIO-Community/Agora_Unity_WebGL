using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMT_Item : MonoBehaviour {

	public int itemIndex = 0;
	public Text txtName;
	public Text txtDeviceId;
	//public AudioManTest amt;
	//public APM_Test amtest;

	public Test_PlaybackDeviceManager tpdm;
	public Test_RecordingDeviceManager trdm;
	public Test_VideoDeviceManager tvdm;

	public void SetDevice()
    {
		if( tpdm != null )
        {
			tpdm.SetAudioRecordingDeviceWithIndex(itemIndex);
		}
		if (trdm != null)
		{
			trdm.SetAudioRecordingDeviceWithIndex(itemIndex);
		}
		if (tvdm != null)
		{
			tvdm.SetVideoDeviceWithIndex(itemIndex);
		}
		//if (amt != null)
		//{
		//	amt.SetAudioRecordingDeviceWithIndex(itemIndex);
		//}
		//if( amtest != null )
		//      {
		//	amtest.SetAudioRecordingDeviceWithIndex(itemIndex);
		//}
	}
}
