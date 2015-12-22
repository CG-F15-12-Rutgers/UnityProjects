using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	
	private bool door_open;
	private bool move;
	GameObject door;

	// Use this for initialization
	void Start () {
		door_open = false;
	}
		
	public void Open()
	{
		move = true;
		door_open = true;
	}

	public void Close()
	{
		door_open = false;
		move = true;
	}

	// Update is called once per frame
	void Update () {
		
		if (door_open && move)
			this.transform.position = new Vector3(10.0f,-10.5f,2.0f);
		else if ((!door_open) && move)
			this.transform.position = new Vector3(10.0f,-1.5f,2.0f);
		move = false;
	}
}
	