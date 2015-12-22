using UnityEngine;
using System.Collections;
using TreeSharpPlus;
public class Ball: MonoBehaviour {

	Rigidbody body;
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
	}

	void OnInteractionStart(Transform t)
	{
		body.isKinematic = true;
		body.useGravity = false;
	}

	// Update is called once per frame
	void Update () {

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");		
		Vector3 movement = new Vector3 (-moveHorizontal, 0.0f, -moveVertical);

		this.transform.position = this.transform.position + movement* 0.1f;
	}
}
