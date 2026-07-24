using Kino;
using System.Collections;
using UnityEngine;

public class Script : MonoBehaviour
{
	public AudioSource audioDevice;

	public AudioSource audioDevice2;

	private bool played;

	public AudioClip getoutof;

	public AudioClip explainnig;

	public GameObject prize;

	public float glitchEffectsValue;

	private void Start()
	{
		glitchEffectsValue += 0.9f;
	}

	private void OnEnable()
	{
		if (Camera.main.GetComponent<AnalogGlitch>() != null)
		{
			Camera.main.GetComponent<AnalogGlitch>().enabled = true;
			Camera.main.GetComponent<AnalogGlitch>().colorDrift = 0f;
			Camera.main.GetComponent<AnalogGlitch>().verticalJump = 0f;
			Camera.main.GetComponent<AnalogGlitch>().scanLineJitter = 0f;
		}
	}

	private void Update()
	{
		if (!audioDevice.isPlaying & played)
		{
			played = false;
			glitchEffectsValue = 0f;
			prize.SetActive(value: false);
			audioDevice2.Play();
			audioDevice2.PlayOneShot(explainnig);
			StartCoroutine(WaitForAudioAndQuit());
		}
		/*if (!audioDevice2.isPlaying & !played)
		{
			Debug.Log("Game Crashed.");
			Application.Quit();
		}*/
	}
	private IEnumerator WaitForAudioAndQuit()
	{
		yield return new WaitWhile(() => audioDevice2.isPlaying);
        Debug.Log("Game Crashed.");
        Application.Quit();
    }
	private void FixedUpdate()
	{
		if (glitchEffectsValue > 1.5f)
		{
			glitchEffectsValue = 1.5f;
		}
		if (glitchEffectsValue >= 0f)
		{
			glitchEffectsValue -= 0.075f * Time.fixedDeltaTime;
		}
		Camera.main.GetComponent<AnalogGlitch>().colorDrift = glitchEffectsValue;
		Camera.main.GetComponent<AnalogGlitch>().scanLineJitter = glitchEffectsValue;
		Camera.main.GetComponent<AnalogGlitch>().verticalJump = glitchEffectsValue / 2f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.name == "Player") & !played)
		{
			audioDevice.Play();
			audioDevice.PlayOneShot(getoutof);
			played = true;
		}
	}
}
