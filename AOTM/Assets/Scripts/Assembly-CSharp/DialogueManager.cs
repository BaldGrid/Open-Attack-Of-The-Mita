using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public GameObject tobe;

	public Text nameText;

	public Text dialogueText;

	public Image character1Icon;

	public Image character2Icon;

	public AudioSource audioSource;

	public Animator dialoguebox;

	public GameControllerScript gc;

	public bool final;

	public bool end;

	public Animator cuts;

	public DialogueTrigger dt;

	private Dialogue dialogue;

	private CharacterDialogue currentCharacterDialogue;

	private int sentenceIndex;

	private bool isAnimating;

	private bool waitForInput;

	private bool isFirstDialogueFinished;

	public void Start()
	{
		dt.TriggerNextDialogue();
		gc.dialogue = true;
		dialoguebox.SetBool("IsOpen", value: true);
	}

	public void StartDialogue(Dialogue dialogue)
	{
		this.dialogue = dialogue;
		sentenceIndex = 0;
		isFirstDialogueFinished = false;
		currentCharacterDialogue = dialogue.character1;
		UpdateCharacterUI();
		StartCoroutine(TypeSentence(currentCharacterDialogue.sentences[sentenceIndex]));
	}

	public void DisplayNextSentence()
	{
		if (isAnimating)
		{
			return;
		}
		if (sentenceIndex < currentCharacterDialogue.sentences.Length - 1)
		{
			sentenceIndex++;
			StopAllCoroutines();
			StartCoroutine(TypeSentence(currentCharacterDialogue.sentences[sentenceIndex]));
			return;
		}
		if (dialogue.character2.sentences.Length == 0)
		{
			EndDialogue();
			return;
		}
		SwitchCharacterDialogue();
		sentenceIndex = 0;
		UpdateCharacterUI();
		StartCoroutine(TypeSentence(currentCharacterDialogue.sentences[sentenceIndex]));
		if (currentCharacterDialogue == dialogue.character1 && sentenceIndex == 0)
		{
			dt.TriggerNextDialogue();
		}
	}

	private IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		char[] array = sentence.ToCharArray();
		foreach (char c in array)
		{
			dialogueText.text += c;
			yield return null;
		}
		if (!HasAnimations() && !HasAudioClips())
		{
			WaitForPlayerInput();
		}
	}

	public void WaitForPlayerInput()
	{
		waitForInput = true;
	}

	private void UpdateCharacterUI()
	{
		nameText.text = currentCharacterDialogue.characterName;
		if (currentCharacterDialogue == dialogue.character1)
		{
			character1Icon.gameObject.SetActive(value: true);
			character2Icon.gameObject.SetActive(value: false);
		}
		else if (currentCharacterDialogue == dialogue.character2)
		{
			character1Icon.gameObject.SetActive(value: false);
			character2Icon.gameObject.SetActive(value: true);
		}
	}

	public bool HasAnimations()
	{
		if (currentCharacterDialogue != null)
		{
			return currentCharacterDialogue.animations.Length != 0;
		}
		return false;
	}

	public bool HasAudioClips()
	{
		if (currentCharacterDialogue != null)
		{
			return currentCharacterDialogue.audioClips.Length != 0;
		}
		return false;
	}

	public bool IsAnimating()
	{
		return isAnimating;
	}

	public bool IsWaitingForInput()
	{
		return waitForInput;
	}

	public void PlayNextAnimation()
	{
		if (HasAnimations())
		{
			AnimationClip animationClip = currentCharacterDialogue.animations[sentenceIndex];
			if (cuts != null && animationClip != null)
			{
				StartCoroutine(PlayAnimationCoroutine(animationClip));
			}
		}
		else
		{
			DisplayNextSentence();
		}
	}

	public void PlayNextAudioClip()
	{
		if (HasAudioClips())
		{
			AudioClip audioClip = currentCharacterDialogue.audioClips[sentenceIndex];
			if (audioSource != null && audioClip != null)
			{
				audioSource.clip = audioClip;
				audioSource.Play();
				StartCoroutine(WaitForAudioClipToEnd(audioClip.length));
			}
		}
		else
		{
			DisplayNextSentence();
		}
	}

	private IEnumerator PlayAnimationCoroutine(AnimationClip animation)
	{
		isAnimating = true;
		cuts.Play(animation.name);
		yield return new WaitForSeconds(animation.length);
		isAnimating = false;
		if (!HasAnimations() && !HasAudioClips())
		{
			if (waitForInput)
			{
				waitForInput = false;
				WaitForPlayerInput();
			}
			else
			{
				DisplayNextSentence();
			}
		}
	}

	private IEnumerator WaitForAudioClipToEnd(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);
		if (!HasAnimations())
		{
			if (waitForInput)
			{
				waitForInput = false;
				WaitForPlayerInput();
			}
			else
			{
				DisplayNextSentence();
			}
		}
	}

	private void SwitchCharacterDialogue()
	{
		if (currentCharacterDialogue == dialogue.character1)
		{
			currentCharacterDialogue = dialogue.character2;
		}
		else if (currentCharacterDialogue == dialogue.character2)
		{
			currentCharacterDialogue = dialogue.character1;
		}
	}

	public void EndDialogue()
	{
		if (final)
		{
			gc.StartFirstBoss();
		}
		if (end)
		{
			tobe.SetActive(value: true);
		}
		gc.dialogue = false;
		dialoguebox.SetBool("IsOpen", value: false);
	}
}
