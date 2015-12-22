using UnityEngine;
using System.Collections;

public class Devils : MonoBehaviour {
	
	public Vector3 position1;
	public Vector3 position2;

	float speed = 3;
	Vector3 goal;
	Vector3 direction;
	
	void Start () {
		goal = position2;
	}
	
	// Update is called once per frame
	void Update () {

		direction = goal - transform.position;
		direction.Normalize();
		transform.position += speed * direction * Time.deltaTime;

		if ((position1 - transform.position).sqrMagnitude < 1){
			goal = position2;
		}
		
		if ((position2 - transform.position).sqrMagnitude < 1){
			goal = position1;
		}
	}
}

