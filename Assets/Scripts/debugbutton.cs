using UnityEngine;

public class debugbutton : MonoBehaviour
{
	public string message = "Button clicked";

	public void OnClick()
	{
		Debug.Log(message);
	}
}

