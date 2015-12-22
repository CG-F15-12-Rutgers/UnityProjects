using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	private float left = 0.0f;
	private float right = 0.0f;
	private float up = 0.0f;
	private float down = 0.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.A))
			left = 0.5f;
		if (Input.GetKey (KeyCode.D))
			right = 0.5f;
		if (Input.GetKey (KeyCode.S))
			down = 0.5f;
		if (Input.GetKey (KeyCode.W))
			up = 0.5f;

		Vector3 movement = new Vector3 (left-right, 0.0f, down-up);
		this.transform.position = this.transform.position + movement;

		left = 0;
		right = 0;
		up = 0;
		down = 0;

	}
}
