using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {
	
	NavMeshAgent agent;
	bool selected = false; 
	
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void setRenderer(){
		MeshRenderer renderer = GetComponent<MeshRenderer> ();

		if (selected) {
			renderer.material.color = Color.grey;
		} else {
			renderer.material.color = Color.white;
		}
	}

	public bool Select(RaycastHit hit){
		
		if (hit.collider.transform == agent.transform) {

			selected = !selected;
			setRenderer ();
			return true;

		} else
			return false;
	}

	public void Destination(Vector3 destination){
		
		if (selected) {
			
			agent.SetDestination(destination);
			selected = !selected;
			setRenderer();
		}
	}
}
