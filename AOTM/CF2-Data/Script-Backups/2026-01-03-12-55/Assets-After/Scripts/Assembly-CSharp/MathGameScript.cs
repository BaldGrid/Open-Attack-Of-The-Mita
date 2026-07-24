using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MathGameScript : MonoBehaviour
{
	public GameControllerScript gc;

	public BaldiScript baldiScript;

	public Vector3 playerPosition;

	public GameObject mathGame;

	public RawImage[] results = new RawImage[3];

	public Texture correct;

	public Texture incorrect;

	public TMP_InputField playerAnswer;

	public TMP_Text questionText;

	public TMP_Text questionText2;

	public TMP_Text questionText3;

	public Animator baldiFeed;

	public Transform baldiFeedTransform;

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

	private string[] hintText = new string[2] { "You might need some help", "No worries, I'll give you some tips!" };

	private string[] creppy = new string[3] { "N£V£R SAY N£V£R", "I'M H£R£", "TH£R£ IS N0 £SC@P£" };

	private bool questionInProgress;

	private bool impossibleMode;

	private bool joystickEnabled;

	public int problemsWrong;

	public AudioClip[] audioQueue = new AudioClip[20];

	public AudioSource baldiAudio;

	public VolumeAnimator volumeAnimator;

	public AudioClip[] learnMusics;

	public GameObject Baldifeeding;

	public bool choking;

	public bool givingprize;

	public KidnapTimer kidtimer;

	private void Start()
	{
		kidtimer = UnityEngine.Object.FindObjectOfType<KidnapTimer>();
		gc.ActivateLearningGame();
		if (gc.notebooks == 1)
		{
			QueueAudio(bal_intro);
			QueueAudio(bal_howto);
		}
		NewProblem();
		if (gc.spoopMode)
		{
			baldiFeedTransform.position = new Vector3(-1000f, -1000f, 0f);
		}
	}

	private void Update()
	{
		if (!baldiAudio.isPlaying && ((audioInQueue > 0) & !gc.spoopMode))
		{
			PlayQueue();
		}
		if ((ControlFreak2.CF2Input.GetKeyDown("return") || ControlFreak2.CF2Input.GetKeyDown("enter")) & questionInProgress)
		{
			questionInProgress = false;
			CheckAnswer();
		}
		if (problem > 3)
		{
			endDelay -= 1f * Time.unscaledDeltaTime;
			if (endDelay <= 0f)
			{
				GC.Collect();
				ExitGame();
				if (!gc.spoopMode)
				{
					kidtimer.enablecutscene = true;
				}
			}
		}
		

		if ((playerAnswer.text == solution.ToString()) & !impossibleMode)
		{
			results[problem - 1].texture = correct;
			baldiAudio.Stop();
			ClearAudioQueue();
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
			QueueAudio(bal_praises[num]);
			NewProblem();
			return;
		}
		
		if (!choking & (kidtimer.Timer <= 0f))
		{
			choking = true;
			gc.entrance_0.Lower();
			gc.entrance_1.Lower();
			gc.entrance_2.Lower();
			gc.entrance_3.Lower();
			gc.backsay.SetActive(value: true);
			baldiAudio.enabled = false;
			volumeAnimator.enabled = false;
			baldiFeed.SetTrigger("Choking");
			gc.learnMusic.Stop();
			gc.ActivateSpoopMode();
		}
		if (choking)
		{
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 2f));
			questionText.text = creppy[num];
		}
	}

	private void NewProblem()
	{
		playerAnswer.text = string.Empty;
		problem++;
		playerAnswer.ActivateInputField();
		if (problem <= 3)
		{
			QueueAudio(bal_problems[problem - 1]);
			if (((gc.mode == "story") & (problem <= 2 || gc.notebooks <= 1)) || ((gc.mode == "endless") & (problem <= 2 || gc.notebooks != 2)))
			{
				num1 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f));
				num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f));
				sign = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				QueueAudio(bal_numbers[Mathf.RoundToInt(num1)]);
				if (sign == 0)
				{
					solution = num1 + num2;
					questionText.text = "SOLVE MATH Q" + problem + ": \n \n" + num1 + "+" + num2 + "=";
					QueueAudio(bal_plus);
				}
				else if (sign == 1)
				{
					solution = num1 - num2;
					questionText.text = "SOLVE MATH Q" + problem + ": \n \n" + num1 + "-" + num2 + "=";
					QueueAudio(bal_minus);
				}
				QueueAudio(bal_numbers[Mathf.RoundToInt(num2)]);
				QueueAudio(bal_equals);
			}
			else
			{
				impossibleMode = true;
				num1 = UnityEngine.Random.Range(1f, 9999f);
				num2 = UnityEngine.Random.Range(1f, 9999f);
				num3 = UnityEngine.Random.Range(1f, 9999f);
				sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
				QueueAudio(bal_screech);
				if (sign == 0)
				{
					questionText.text = "SOLVE MATH Q" + problem + ": \n" + num1 + "+(" + num2 + "X" + num3 + "=";
					QueueAudio(bal_plus);
					QueueAudio(bal_screech);
					QueueAudio(bal_times);
					QueueAudio(bal_screech);
				}
				else if (sign == 1)
				{
					questionText.text = "SOLVE MATH Q" + problem + ": \n (" + num1 + "/" + num2 + ")+" + num3 + "=";
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
					questionText2.text = "SOLVE MATH Q" + problem + ": \n" + num1 + "+(" + num2 + "X" + num3 + "=";
				}
				else if (sign == 1)
				{
					questionText2.text = "SOLVE MATH Q" + problem + ": \n (" + num1 + "/" + num2 + ")+" + num3 + "=";
				}
				num1 = UnityEngine.Random.Range(1f, 9999f);
				num2 = UnityEngine.Random.Range(1f, 9999f);
				num3 = UnityEngine.Random.Range(1f, 9999f);
				sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
				if (sign == 0)
				{
					questionText3.text = "SOLVE MATH Q" + problem + ": \n" + num1 + "+(" + num2 + "X" + num3 + "=";
				}
				else if (sign == 1)
				{
					questionText3.text = "SOLVE MATH Q" + problem + ": \n (" + num1 + "/" + num2 + ")+" + num3 + "=";
				}
				QueueAudio(bal_equals);
			}
			questionInProgress = true;
		}
		else
		{
			endDelay = 5f;
			if (problemsWrong == 0)
			{
				questionText.text = "WOW! YOU'RE AWAKE!";
				gc.givingprize = true;
			}
			else if (problemsWrong >= 0)
			{
				int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				questionText.text = hintText[num];
				questionText2.text = string.Empty;
				questionText3.text = string.Empty;
				gc.givingprize = false;
			}
		}
	}

	public void OKButton()
	{
		CheckAnswer();
	}

	public void CheckAnswer()
	{
		if (problem > 3)
		{
			return;
		}
		if (playerAnswer.text.Trim() == "320999965")
		{
			// 检查场景是否存在
			try
			{
				SceneManager.LoadScene("Oh");
				return; // 加载场景后直接返回，不执行后续代码
			}
			catch (Exception e)
			{
				Debug.LogError("无法加载场景 Basement1: " + e.Message);
				// 可以给玩家一个提示
				questionText.text = "ERROR: SCENE NOT FOUND!";
				
				// 如果场景加载失败，继续执行正常的答题逻辑
				// 这样玩家可以继续游戏
			}
		}
		// 原有的42彩蛋逻辑
		if (playerAnswer.text.Trim() == "42")
		{
			solution = float.Parse(playerAnswer.text);
			impossibleMode = false;
			results[problem - 1].texture = correct;
			baldiAudio.Stop();
			ClearAudioQueue();
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
			QueueAudio(bal_praises[num]);
			NewProblem();
			return;
		}
		if ((playerAnswer.text == solution.ToString()) & !impossibleMode)
		{
			results[problem - 1].texture = correct;
			baldiAudio.Stop();
			ClearAudioQueue();
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
			QueueAudio(bal_praises[num]);
			NewProblem();
			return;
		}
		problemsWrong++;
		results[problem - 1].texture = incorrect;
		baldiAudio.Stop();
		ClearAudioQueue();
		if (gc.mode == "story")
		{
			if (problem == 3)
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
		gc.DeactivateLearningGame(base.gameObject);
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

	private IEnumerator CheatText(string text)
	{
		while (true)
		{
			questionText.text = text;
			questionText2.text = string.Empty;
			questionText3.text = string.Empty;
			yield return new WaitForEndOfFrame();
		}
	}
}