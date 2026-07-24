using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
	private Image image;

	private float delay;

	public Sprite[] images = new Sprite[1];

	public Sprite rare;

	private float chance;

	private AudioSource audioDevice;

	public AudioClip ClassicError;

	public TMP_Text ErrorText;

	private void Start()
	{
		image = GetComponent<Image>();
		audioDevice = GetComponent<AudioSource>();
		delay = 5f;
		chance = Random.Range(1f, 99f);
		ErrorText.text = "";
		if (chance < 98f)
		{
			int num = Mathf.RoundToInt(Random.Range(0f, 1f));
			image.sprite = images[num];
		}
		else
		{
			image.sprite = rare;
			audioDevice.PlayOneShot(ClassicError);
			ErrorText.text = "Oh no!\n\nA critical error has \noccured!\n\nThe game will now close.";
		}
	}

	private void Update()
	{
		delay -= 1f * Time.deltaTime;
		if (delay <= 0f)
		{
			if (chance < 98f)
			{
				SceneManager.LoadScene("MainMenu");
			}
			else if (delay <= -3f)
			{
				Application.Quit();
			}
		}
	}
}
