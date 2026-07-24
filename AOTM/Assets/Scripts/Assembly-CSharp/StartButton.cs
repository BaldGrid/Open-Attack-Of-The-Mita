using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
	public enum Mode
	{
		StoryPart1 = 0,
		StoryPart2 = 1,
		Endless = 2
	}

	public Mode currentMode;

	public void StartGame()
	{
		switch (currentMode)
		{
			case Mode.StoryPart1:
				PlayerPrefs.SetString("CurrentMode", "story");
				PlayerPrefs.SetString("StoryPart", "part1");
				SceneManager.LoadSceneAsync("School"); // Part1 的场景
				break;
			case Mode.StoryPart2:
				PlayerPrefs.SetString("CurrentMode", "story");
				PlayerPrefs.SetString("StoryPart", "part2");
				SceneManager.LoadSceneAsync("School1"); // Part2 的场景
				break;
			case Mode.Endless:
				PlayerPrefs.SetString("CurrentMode", "endless");
				SceneManager.LoadSceneAsync("School");
				break;
		}
	}
}