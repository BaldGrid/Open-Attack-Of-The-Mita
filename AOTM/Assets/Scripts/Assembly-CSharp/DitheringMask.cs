using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DitheringMask : MonoBehaviour
{
	[SerializeField]
	private Image image;

	public bool isTransitionActive;

	public GameControllerScript gc;

	public bool hasGC;

	public bool doOnStart;

	public bool fadeIn;

	private void Start()
	{
		if (doOnStart)
		{
			FadeOutAndDisable();
		}
		if (fadeIn)
		{
			FadeIn();
		}
	}

	private void Awake()
	{
		image = GetComponent<Image>();
		image.enabled = false;
	}

	private IEnumerator TransitionRoutine(float start, float end)
	{
		for (float t = 0f; t < 1f; t += Time.deltaTime)
		{
			float a = Mathf.Lerp(start, end, t / 1f);
			Color color = image.color;
			color.a = a;
			image.color = color;
			yield return null;
		}
	}

	private void OnEnable()
	{
		StartCoroutine(FadeInRoutine());
	}

	public void FadeOutAndDisable()
	{
		StartCoroutine(FadeOutAndDisableRoutine());
	}

	public void FadeOut()
	{
		StartCoroutine(FadeOutRoutine());
	}

	public void FadeIn()
	{
		StartCoroutine(FadeInRoutine());
	}

	public void FadeOutQuit()
	{
		StartCoroutine(QuitRoutine());
	}

	public void FadeOutAndDestory(GameObject _obj)
	{
		StartCoroutine(FadeOutAndDestroyRoutine(_obj));
	}

	private IEnumerator FadeInRoutine()
	{
		isTransitionActive = true;
		DitheringBlocker.Block();
		StartCoroutine(TransitionRoutine(0f, 1f));
		yield return new WaitForSeconds(0.8f);
		DitheringBlocker.UnBlock();
		yield return new WaitForSeconds(0.4f);
		isTransitionActive = false;
	}

	private IEnumerator FadeOutAndDisableRoutine()
	{
		isTransitionActive = true;
		DitheringBlocker.Block();
		StartCoroutine(TransitionRoutine(1f, 0f));
		yield return new WaitForSeconds(0.8f);
		DitheringBlocker.UnBlock();
		isTransitionActive = false;
		GetComponentInParent<Canvas>().gameObject.SetActive(value: false);
		if (gc != null && hasGC)
		{
			gc.UnpauseGame();
		}
	}

	private IEnumerator FadeOutRoutine()
	{
		isTransitionActive = true;
		DitheringBlocker.Block();
		StartCoroutine(TransitionRoutine(1f, 0f));
		yield return new WaitForSeconds(0.8f);
		DitheringBlocker.UnBlock();
		isTransitionActive = false;
	}

	private IEnumerator QuitRoutine()
	{
		isTransitionActive = true;
		DitheringBlocker.Block();
		Time.timeScale = 0.3f;
		StartCoroutine(TransitionRoutine(1f, 0f));
		yield return new WaitForSeconds(0.8f);
		DitheringBlocker.UnBlock();
		isTransitionActive = false;
	}

	private IEnumerator FadeOutAndDestroyRoutine(GameObject subject)
	{
		isTransitionActive = true;
		DitheringBlocker.Block();
		StartCoroutine(TransitionRoutine(1f, 0f));
		yield return new WaitForSeconds(0.8f);
		DitheringBlocker.UnBlock();
		isTransitionActive = false;
		Object.Destroy(subject);
	}
}
