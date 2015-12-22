using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	public GameObject agent1;
	public GameObject agent2;
	public GameObject agent3;

	private float smooth = 10;
	private Vector3 offset;
	
	void Start (){
		offset =(transform.position - (agent1.transform.position+agent2.transform.position+agent3.transform.position)/3);

	}
	
	void Update (){
		// transform.position = offset + (agent1.transform.position+agent2.transform.position+agent3.transform.position)/3;
		transform.position = Vector3.Lerp(transform.position, (offset + (agent1.transform.position+agent2.transform.position+agent3.transform.position)/3)/20*20, Time.deltaTime * smooth);
	}
}
