using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public Dialogue[] dialogues;

	public Animator animator;

	public DialogueManager dialogueManager;

	public GameObject bobIcon;

	public GameObject samIcon;

	public float animationDelay = 0.5f;

	public float audioClipDelay = 0.5f;

	private int currentDialogueIndex;

	private bool isBobSpeaking = true;

	public void TriggerDialogue(Dialogue dialogue)
	{
		if (animator != null)
		{
			animator.SetTrigger("TriggerAnimation");
		}
		bobIcon.SetActive(isBobSpeaking);
		samIcon.SetActive(!isBobSpeaking);
		dialogueManager.StartDialogue(dialogue);
	}

	private IEnumerator PlayNextAnimationWithDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		while (dialogueManager.HasAnimations())
		{
			dialogueManager.PlayNextAnimation();
			yield return new WaitForSeconds(delay);
		}
		yield return new WaitForSeconds(audioClipDelay);
		if (dialogueManager.HasAudioClips())
		{
			dialogueManager.PlayNextAudioClip();
			yield break;
		}
		SwitchSpeaker();
		StartCoroutine(PlayNextAnimationWithDelay(animationDelay));
	}

	public void SwitchSpeaker()
	{
		isBobSpeaking = !isBobSpeaking;
		bobIcon.SetActive(isBobSpeaking);
		samIcon.SetActive(!isBobSpeaking);
	}

	public void TriggerNextDialogue()
	{
		if (!dialogueManager.IsAnimating() && !dialogueManager.IsWaitingForInput())
		{
			if (currentDialogueIndex < dialogues.Length)
			{
				Dialogue dialogue = dialogues[currentDialogueIndex];
				TriggerDialogue(dialogue);
				currentDialogueIndex++;
			}
			else
			{
				Debug.LogWarning("No more dialogues to trigger.");
			}
		}
	}
}
