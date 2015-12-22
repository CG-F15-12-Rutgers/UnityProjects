using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	
	bool selected;

	// Use this for initialization
	void Start () {
		selected = false;
	}
	
	// Update is called once per frame
	void Update () {

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");		
		Vector3 movement = new Vector3 (-moveHorizontal,0.0f,-moveVertical);

		if (Input.GetMouseButtonDown(0)){

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit, 100)){
				if (hit.collider.transform == transform){
					selected = !selected;
				}
			}
		}

		MeshRenderer renderer = GetComponent<MeshRenderer>();
		if (selected) {
			renderer.material.color = Color.grey;
		} else {
			renderer.material.color = Color.white;
		}

		if (selected) {
		transform.position = transform.position + movement;

		}
	}
}
