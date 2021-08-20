using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif

public class PermissionHelper{

	public static void RequestMicrophontPermission()
	{
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
		if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
		{                 
			Permission.RequestUserPermission(Permission.Microphone);
		}
#endif
	}

	public static void RequestCameraPermission()
	{
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
		if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
		{                 
			Permission.RequestUserPermission(Permission.Camera);
		}
#endif	
	}

}
