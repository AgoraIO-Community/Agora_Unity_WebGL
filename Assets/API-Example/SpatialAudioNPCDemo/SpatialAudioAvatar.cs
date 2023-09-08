using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAudioAvatar : MonoBehaviour {

	public float moveSpeed = 500f, runSpeed = 750f, mouseSpeed = 250f, tempRot, arrowMoveTime;
	public Transform arrow;
	public Rigidbody2D rig;

	public bool mouseMove = false, keyMove = false, arrowMove = false, run = false;

	private Dictionary<Vector2, float> keyArrowRotations = new Dictionary<Vector2, float>();

	private Vector2 currentDir = new Vector2(100, 100);

	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D>();

		keyArrowRotations.Add(new Vector2(0, 1), 0);
		keyArrowRotations.Add(new Vector2(1, 1), 315);
		keyArrowRotations.Add(new Vector2(-1, 1), 45);
		keyArrowRotations.Add(new Vector2(0, -1), 180);
		keyArrowRotations.Add(new Vector2(-1, -1), 135);
		keyArrowRotations.Add(new Vector2(1, -1), 225);
		keyArrowRotations.Add(new Vector2(-1, 0), 90);
		keyArrowRotations.Add(new Vector2(1, 0), 270);
	}
	
	// Update is called once per frame
	void Update () {

		run = Input.GetKey(KeyCode.LeftShift);


		if (Input.GetMouseButton(0) && !SpatialAudioDemoManager.isHoveringOverButton && !NPCSettings.instance.isOn)
		{
			arrow.gameObject.SetActive(true);
			updateMouseMovement();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			rig.velocity = Vector2.zero;
			arrow.gameObject.SetActive(false);
			mouseMove = false;
		}

		if (!mouseMove)
		{
			updateKeyboardMovement();
		}
	}

	void updateMouseMovement(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 dir = (mousePos - transform.position).normalized;
		Vector3 mouseDir = (mousePos - transform.position);
		float userDistance = Mathf.Clamp(Vector3.Distance(transform.position, mousePos), -5f, 5f);
		dir.z = 0f;
		rig.velocity = (dir * (mouseSpeed * userDistance) * Time.deltaTime);
		Quaternion rot = Quaternion.LookRotation(mouseDir, Vector3.forward);
		Vector3 arrowDir = rot.eulerAngles;
		arrow.eulerAngles = new Vector3(0f, 0f, -arrowDir.z);
		mouseMove = true;
	}

	void updateKeyboardMovement()
    {
		float hor = Input.GetAxisRaw("Horizontal");
		float ver = Input.GetAxisRaw("Vertical");

		Vector2 dir = new Vector2(hor, ver);

		if (dir != Vector2.zero)
		{
			rig.velocity = (dir * (run ? runSpeed : moveSpeed) * Time.deltaTime);
			arrow.gameObject.SetActive(true);
		} else
        {
			rig.velocity = Vector2.zero;
			keyMove = false;
			arrow.gameObject.SetActive(false);
			arrowMove = false;
        }

		if (dir != Vector2.zero && dir != currentDir && keyArrowRotations.ContainsKey(dir) && !arrowMove)
		{
			tempRot = arrow.transform.eulerAngles.z;
			arrowMove = true;
			arrowMoveTime = 0f;
		}

		if(arrowMove && arrowMoveTime < 1f && keyArrowRotations.ContainsKey(dir))
        {
			Quaternion newAngle = Quaternion.Euler(new Vector3(0, 0, keyArrowRotations[dir]));
			arrow.transform.rotation = Quaternion.RotateTowards(arrow.transform.rotation, newAngle, 3f);
			arrowMoveTime += .01f;
		} else if(arrowMove && Mathf.Round(arrowMoveTime) >= 1)
        {
			arrowMove = false;
        }
	}
}
