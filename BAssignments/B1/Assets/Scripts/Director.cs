using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour {

	public GameObject camera;
	private Agent[] agents;

	// Use this for initialization
	void Start () {

		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Agent");
		agents = new Agent[gos.Length];
		
		int i = 0;
		foreach(GameObject agent in gos){
			agents[i] = agent.GetComponent<Agent>();
			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)){

			bool setDestination = true;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 100)){

                foreach (Agent agent in agents){ 

					if (agent.Select(hit)){	
						setDestination = false;
						break;
                    }
                }

				if (setDestination){
                    foreach (Agent agent in agents){
						agent.Destination(hit.point);
                    }
                }
            }
        }
    }
}