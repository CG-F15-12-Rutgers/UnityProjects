using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

	public void LoadScene(int level)
	{
		Application.LoadLevel(level);
	}
}