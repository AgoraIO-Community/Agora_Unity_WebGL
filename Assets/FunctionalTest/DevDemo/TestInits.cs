using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using agora_utilities;

public class TestInits : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	
	public void InitQuad()
    {
        GameObject quad = GameObject.Find("Quad");
        if (ReferenceEquals(quad, null))
        {
            Debug.Log("failed to find Quad");
            return;
        }
        else
        {
            Debug.Log("Found quad");
            VideoSurface vs = quad.AddComponent<VideoSurface>();
            //vs.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            //vs.name = "quad";
            //vs.useVIndex = 1;
            vs.SetEnable(true);
        }
    }

    public void InitQuad2()
    {
        GameObject quad2 = GameObject.Find("Quad2");
        if (ReferenceEquals(quad2, null))
        {
            Debug.Log("failed to find Quad2");
            return;
        }
        else
        {
            Debug.Log("Found quad2");
            VideoSurface vs = quad2.AddComponent<VideoSurface>();
            //vs.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            //vs.name = "quad2";
            //vs.useVIndex = 1;
            vs.SetEnable(true);
        }
    }


    public void InitCube()
    {
        GameObject cube = GameObject.Find("Cube");
        if (ReferenceEquals(cube, null))
        {
            Debug.Log("failed to find cube");
            return;
        }
        else
        {
            Debug.Log("Found cube");
            VideoSurface vs = cube.AddComponent<VideoSurface>();
            //vs.name = "cube";
            //vs.useVIndex = 1;
            vs.SetEnable(true);
        }
    }

}
