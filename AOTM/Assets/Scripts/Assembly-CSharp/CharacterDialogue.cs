using System;
using UnityEngine;

[Serializable]
public class CharacterDialogue
{
	public string characterName;

	[TextArea(3, 10)]
	public string[] sentences;

	public AnimationClip[] animations;

	public AudioClip[] audioClips;
}
