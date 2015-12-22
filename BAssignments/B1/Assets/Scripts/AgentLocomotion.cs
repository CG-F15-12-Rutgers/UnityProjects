using UnityEngine;
using System.Collections;

public class AgentLocomotion : MonoBehaviour {

    protected Animator animator;
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = true; 

	private NavMeshAgent agent;
	private float run = 5;
	private Vector3 direction;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent> ();
		
	}
	
	// Update is called once per frame
	void Update () {
        
		if (animator){

			Vector3 direction = agent.nextPosition - transform.position;

			float h = Vector3.Dot (transform.right, direction);
			float v = Vector3.Dot (transform.forward, direction);

			animator.SetFloat("Walk", v);
			animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);
		
			// Check sprinting
			if(agent.velocity.sqrMagnitude > run) {
				animator.SetFloat("Sprint", 1);
			}else{
				animator.SetFloat("Sprint", 0);
			}
			
			// Check jumping
			animator.SetBool("Jump", false);
			if (agent.velocity.y > 1 || agent.velocity.y < -1){
				animator.SetBool("Jump", true);
			}        			
		}   

    }
}

		 