using UnityEngine;

public class VolumeAnimator : MonoBehaviour
{
	public string animationName;

	public AnimationCurve sensitivity;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private AudioSource audioSource;

	private AudioClip currentClip;

	public float bufferTime = 0.1f;

	private float[] clipData;

	private float volume;

	private float potentialVolume;

	private int lastSample;

	private int sampleBuffer;

	private void Update()
	{
		if (audioSource.isPlaying)
		{
			if (audioSource.clip != currentClip)
			{
				currentClip = audioSource.clip;
				clipData = new float[currentClip.samples * currentClip.channels];
				currentClip.GetData(clipData, 0);
				lastSample = 0;
				sampleBuffer = Mathf.RoundToInt((float)currentClip.samples / currentClip.length * bufferTime);
			}
			volume = 0f;
			for (int i = Mathf.Max(lastSample - sampleBuffer, 0); i < audioSource.timeSamples * currentClip.channels && i < clipData.Length; i++)
			{
				potentialVolume = sensitivity.Evaluate(Mathf.Abs(clipData[i]));
				if (potentialVolume > volume)
				{
					volume = potentialVolume;
				}
			}
			lastSample = audioSource.timeSamples * currentClip.channels;
			animator.Play(animationName, -1, Mathf.Max(volume));
		}
		else
		{
			animator.Play(animationName, -1, 0f);
		}
	}
}
