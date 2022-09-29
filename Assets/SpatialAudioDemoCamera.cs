using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAudioDemoCamera : MonoBehaviour {

	public BoxCollider2D targetBox, cameraBox;
	private float minX, minY, maxX, maxY;
	public Transform user;

	// Use this for initialization
	void Start () {
		cameraBox = GetComponent<BoxCollider2D>();
		Bounds cameraBounds = cameraBox.bounds;
		Bounds targetBounds = targetBox.bounds;
		minX = targetBounds.min.x + cameraBounds.size.x / 2;
		minY = targetBounds.min.y + cameraBounds.size.y / 2;
		maxX = targetBounds.max.x - cameraBounds.size.x / 2;
		maxY = targetBounds.max.y - cameraBounds.size.y / 2;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(user.position.x, user.position.y, transform.position.z);
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
		Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
	}
}
