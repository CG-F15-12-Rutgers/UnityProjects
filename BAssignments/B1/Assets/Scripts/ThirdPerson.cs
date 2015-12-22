using UnityEngine;
using System.Collections;

public class ThirdPerson : MonoBehaviour
{	
	// distances above and behind
			
	public GameObject player;		

	// smooth camera and initiate target position
	private float smooth = 3;
	private Vector3 targetPosition;	
	private float distanceAway = 7;			
	private float distanceUp = 7;
	
	Transform follow;
	
	void Start(){
		follow = player.transform;	
	}
	
	void LateUpdate ()
	{

		targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;

		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

		transform.LookAt(follow);
	}
}
