using System.Collections;
using UnityEngine;

public class ExitButtonScript : MonoBehaviour
{
	public AudioSource audMan;

	public AudioSource MenuVoice;

	public void ExitGame()
	{
		audMan.Play();
		MenuVoice.Stop();
		StartCoroutine(WaitForAudio());
	}

	private IEnumerator WaitForAudio()
	{
		while (audMan.isPlaying)
		{
			yield return null;
			Cursor.lockState = CursorLockMode.Locked;
		}
		Application.Quit();
	}
}
