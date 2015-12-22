using UnityEngine;
using System.Collections;

public class MyController : MonoBehaviour {
	
	
	protected Animator animator;
	public bool ApplyGravity = true; 

	private float DirectionDampTime = .25f;
	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (animator)
		{		
			// get Axises values
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			animator.SetFloat("Walk", v);
			animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);	


			// Check sprinting

			if (Input.GetKey (KeyCode.LeftShift)) {
				animator.SetFloat("Sprint", 1);
			}
			else {animator.SetFloat("Sprint", 0);}


			// Check jumping
			animator.SetBool("Jump", false);
			if (Input.GetKeyDown(KeyCode.Space)) 
			{
				animator.SetBool("Jump", true);
			}        			
		}   		  
	}
}