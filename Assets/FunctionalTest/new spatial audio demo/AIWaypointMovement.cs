using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaypointMovement : MonoBehaviour {

	public GameObject path;
	public List<Transform> waypoints;
	public Sprite[] sprites;
	public int waypointIndex;
	public float lerpTime, lerpDistance;
	public Transform startingPoint, endPoint;
	SpriteRenderer sprite;
	

	// Use this for initialization
	void Start () {
		waypoints = new List<Transform>();
		waypoints.AddRange(path.GetComponentsInChildren<Transform>());
		waypoints.Remove(path.transform);
		sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		endPoint = waypoints[waypointIndex];
		transform.position = Vector2.Lerp(startingPoint.position, endPoint.position, lerpDistance / lerpTime);
		lerpDistance += Time.deltaTime;
		if(lerpDistance / lerpTime >= 1f){
			startingPoint = waypoints[waypointIndex];
			if(waypointIndex < waypoints.Count-1)
				waypointIndex += 1;
			else
				waypointIndex = 0;
			
			lerpDistance = 0f;
			sprite.sprite = sprites[waypointIndex];
		}
	}
}
