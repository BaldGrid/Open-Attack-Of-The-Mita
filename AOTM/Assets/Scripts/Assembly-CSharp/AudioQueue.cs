using System.Collections.Generic;
using UnityEngine;

public class AudioQueue : MonoBehaviour
{
	private Queue<AudioClip> audioClips;

	private AudioSource audioSource;

	private bool isPlaying;

	private void Awake()
	{
		audioClips = new Queue<AudioClip>();
		audioSource = GetComponent<AudioSource>();
		isPlaying = false;
	}

	public void QueueAudio(AudioClip clip)
	{
		audioClips.Enqueue(clip);
		if (!isPlaying)
		{
			PlayNextAudio();
		}
	}

	private void PlayNextAudio()
	{
		if (audioClips.Count > 0)
		{
			isPlaying = true;
			AudioClip clip = audioClips.Dequeue();
			audioSource.clip = clip;
			audioSource.Play();
		}
		else
		{
			isPlaying = false;
		}
	}
}
