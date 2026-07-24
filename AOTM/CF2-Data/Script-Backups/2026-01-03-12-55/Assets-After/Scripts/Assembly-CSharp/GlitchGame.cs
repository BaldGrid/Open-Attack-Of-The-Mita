using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlitchGame : MonoBehaviour
{
	public GameControllerScript gc;

	public BaldiScript baldiScript;

	public Vector3 playerPosition;

	public GameObject mathGame;

	public RawImage results;

	public Texture correct;

	public Texture incorrect;

	public TMP_InputField playerAnswer;

	public TMP_Text questionText;

	public TMP_Text questionText2;

	public TMP_Text questionText3;

	public AudioClip bal_plus;

	public AudioClip bal_minus;

	public AudioClip bal_times;

	public AudioClip bal_divided;

	public AudioClip bal_equals;

	public AudioClip bal_howto;

	public AudioClip bal_intro;

	public AudioClip bal_screech;

	public AudioClip[] bal_numbers = new AudioClip[10];

	public AudioClip[] bal_praises = new AudioClip[5];

	public AudioClip[] bal_problems = new AudioClip[3];

	public Button firstButton;

	private float endDelay;

	private int problem;

	private int audioInQueue;

	private float num1;

	private float num2;

	private float num3;

	private int sign;

	private float solution;

	private bool questionInProgress;

	private bool impossibleMode;

	private bool joystickEnabled;

	private int problemsWrong;

	public AudioClip[] audioQueue = new AudioClip[20];

	public AudioSource baldiAudio;

	public AudioClip[] learnMusics;

	private string[] hintText = new string[3] { "Y0U AR£ N@XT!", "I WILL FIND Y@U", "TH£R@ IS N@ £SCAP£" };

	private void Start()
	{
		gc.ActivateLearningGame2();
		NewProblem();
	}

	private void Update()
	{
		if ((ControlFreak2.CF2Input.GetKeyDown("return") || ControlFreak2.CF2Input.GetKeyDown("enter")) & questionInProgress)
		{
			questionInProgress = false;
			CheckAnswer();
		}
		if (problem > 1)
		{
			endDelay -= 1f * Time.unscaledDeltaTime;
			if (endDelay <= 0f)
			{
				GC.Collect();
				ExitGame();
			}
		}
	}

	private void NewProblem()
	{
		playerAnswer.text = string.Empty;
		problem++;
		playerAnswer.ActivateInputField();
		if (problem <= 1)
		{
			if (gc.spoopMode)
			{
				gc.learnMusic.Stop();
			}
			QueueAudio(bal_problems[problem - 1]);
			if (((gc.mode == "story") & (problem <= 1 || gc.notebooks <= 0)) || problem <= 1 || gc.notebooks != 0)
			{
				impossibleMode = true;
				num1 = UnityEngine.Random.Range(1f, 9999f);
				num2 = UnityEngine.Random.Range(1f, 9999f);
				num3 = UnityEngine.Random.Range(1f, 9999f);
				sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
				QueueAudio(bal_screech);
				if (sign == 0)
				{
					questionText.text = "H£ WILL N£V£R <0M£ BACK" + problem + " \n \n" + num1 + "+(" + num2 + "X" + num3 + "=?";
					QueueAudio(bal_plus);
					QueueAudio(bal_screech);
					QueueAudio(bal_times);
					QueueAudio(bal_screech);
				}
				else if (sign == 1)
				{
					questionText.text = "H£ WILL N£V£R <0M£ BACK" + problem + " \n \n (" + num1 + "/" + num2 + ")+" + num3 + "=?";
					QueueAudio(bal_divided);
					QueueAudio(bal_screech);
					QueueAudio(bal_plus);
					QueueAudio(bal_screech);
				}
				num1 = UnityEngine.Random.Range(1f, 9999f);
				num2 = UnityEngine.Random.Range(1f, 9999f);
				num3 = UnityEngine.Random.Range(1f, 9999f);
				sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
				if (sign == 0)
				{
					questionText2.text = "H£ WILL N£V£R <0M£ BACK" + problem + " \n \n" + num1 + "+(" + num2 + "X" + num3 + "=?";
				}
				else if (sign == 1)
				{
					questionText2.text = "H£ WILL N£V£R <0M£ BACK" + problem + " \n \n (" + num1 + "/" + num2 + ")+" + num3 + "=?";
				}
				num1 = UnityEngine.Random.Range(1f, 9999f);
				num2 = UnityEngine.Random.Range(1f, 9999f);
				num3 = UnityEngine.Random.Range(1f, 9999f);
				sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
				if (sign == 0)
				{
					questionText3.text = "H£ WILL N£V£R <0M£ BACK" + problem + " \n \n" + num1 + "+(" + num2 + "X" + num3 + "=?";
				}
				else if (sign == 1)
				{
					questionText3.text = "H£ WILL N£V£R <0M£ BACK" + problem + " \n \n (" + num1 + "/" + num2 + ")+" + num3 + "=?";
				}
				QueueAudio(bal_equals);
			}
			questionInProgress = true;
		}
		else
		{
			endDelay = 2f;
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 2f));
			questionText.text = hintText[num];
			questionText2.text = string.Empty;
			questionText3.text = string.Empty;
		}
	}

	public void OKButton()
	{
		CheckAnswer();
	}

	public void CheckAnswer()
	{
		if (problem > 1)
		{
			return;
		}
		if ((playerAnswer.text == solution.ToString()) & !impossibleMode)
		{
			results.texture = correct;
			baldiAudio.Stop();
			ClearAudioQueue();
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
			QueueAudio(bal_praises[num]);
			NewProblem();
			return;
		}
		problemsWrong++;
		results.texture = incorrect;
		if (!gc.spoopMode)
		{
			gc.ActivateSpoopMode();
		}
		if (gc.mode == "story")
		{
			if (problem == 1)
			{
				baldiScript.GetAngry(1f);
			}
			else
			{
				baldiScript.GetTempAngry(0.25f);
			}
		}
		else
		{
			baldiScript.GetAngry(1f);
		}
		ClearAudioQueue();
		baldiAudio.Stop();
		NewProblem();
	}

	private void QueueAudio(AudioClip sound)
	{
		audioQueue[audioInQueue] = sound;
		audioInQueue++;
	}

	private void PlayQueue()
	{
		baldiAudio.clip = audioQueue[0];
		baldiAudio.Play();
		UnqueueAudio();
	}

	private void UnqueueAudio()
	{
		for (int i = 1; i < audioInQueue; i++)
		{
			audioQueue[i - 1] = audioQueue[i];
		}
		audioInQueue--;
	}

	private void ClearAudioQueue()
	{
		audioInQueue = 0;
	}

	private void ExitGame()
	{
		if ((problemsWrong <= 0) & (gc.mode == "endless"))
		{
			baldiScript.GetAngry(-1f);
		}
		gc.DeactivateLearningGame2(base.gameObject);
	}

	public void ButtonPress(int value)
	{
		if (value >= 0 && value <= 9)
		{
			playerAnswer.text += value;
		}
		else if (value == -1)
		{
			playerAnswer.text += "-";
		}
		else
		{
			playerAnswer.text = string.Empty;
		}
	}
}
