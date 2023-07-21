using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAudioAvatar : MonoBehaviour {

	public float moveSpeed = 500f, runSpeed = 10;
	public Transform arrow;
	public Rigidbody2D rig;

	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			arrow.gameObject.SetActive(true);
			updateMovement();
		} else if(Input.GetMouseButtonUp(0)) {
			rig.velocity = Vector2.zero;
			arrow.gameObject.SetActive(false);
		}
	}

	void updateMovement(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 dir = (mousePos - transform.position).normalized;
		Vector3 mouseDir = (mousePos - transform.position);
		float userDistance = Mathf.Clamp(Vector3.Distance(transform.position, mousePos), -5f, 5f);
		dir.z = 0f;
		rig.velocity = (dir * (moveSpeed + (userDistance * runSpeed)) * Time.deltaTime);
		Quaternion rot = Quaternion.LookRotation(mouseDir, Vector3.forward);
		Vector3 arrowDir = rot.eulerAngles;
		arrow.eulerAngles = new Vector3(0f, 0f, -arrowDir.z);
	}
}
