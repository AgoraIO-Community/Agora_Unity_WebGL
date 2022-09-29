using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerHandler : MonoBehaviour {

	SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		sprite.sortingOrder = -(int)Mathf.Round(transform.position.y);
	}
}
