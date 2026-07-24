using UnityEngine;

public class ChangeAudioSpeed : MonoBehaviour
{
	public float speed = 1f;

	private AudioSource audioSource;

	private AudioClip audioClip;

	private float pitch;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioClip = audioSource.clip;
		pitch = audioSource.pitch;
	}

	private void Update()
	{
		audioSource.pitch = pitch * speed;
		audioSource.timeSamples += (int)((float)audioClip.frequency * Time.deltaTime * speed);
	}
}
