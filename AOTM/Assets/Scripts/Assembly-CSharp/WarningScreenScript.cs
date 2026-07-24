using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningScreenScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (ControlFreak2.CF2Input.anyKeyDown)
		{
			SceneManager.LoadScene("MainMenu");
		}
	}
}
