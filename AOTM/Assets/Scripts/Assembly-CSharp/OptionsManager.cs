using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
	public Slider slider;

	public Toggle rumble;

	public Toggle analog;

	private Resolution[] resolutions;

	public TMP_Dropdown resolutionDropdown;

	public Toggle isFullScreen;

	private void Start()
	{
		resolutions = Screen.resolutions;
		resolutionDropdown.ClearOptions();
		List<string> list = new List<string>();
		int value = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string item = resolutions[i].width + " x " + resolutions[i].height;
			list.Add(item);
			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
				value = i;
			}
		}
		resolutionDropdown.AddOptions(list);
		resolutionDropdown.value = value;
		resolutionDropdown.RefreshShownValue();
		if (PlayerPrefs.HasKey("OptionsSet"))
		{
			if (PlayerPrefs.GetInt("isFullScreen") == 1)
			{
				isFullScreen.isOn = true;
			}
			else
			{
				isFullScreen.isOn = false;
			}
		}
		else
		{
			PlayerPrefs.SetInt("OptionsSet", 1);
		}
		if (PlayerPrefs.HasKey("OptionsSet"))
		{
			slider.value = PlayerPrefs.GetFloat("MouseSensitivity");
			if (PlayerPrefs.GetInt("Rumble") == 1)
			{
				rumble.isOn = true;
			}
			else
			{
				rumble.isOn = false;
			}
			if (PlayerPrefs.GetInt("AnalogMove") == 1)
			{
				analog.isOn = true;
			}
			else
			{
				analog.isOn = false;
			}
		}
		else
		{
			PlayerPrefs.SetInt("OptionsSet", 1);
		}
	}

	private void Update()
	{
		PlayerPrefs.SetFloat("MouseSensitivity", slider.value);
		if (rumble.isOn)
		{
			PlayerPrefs.SetInt("Rumble", 1);
		}
		else
		{
			PlayerPrefs.SetInt("Rumble", 0);
		}
		if (analog.isOn)
		{
			PlayerPrefs.SetInt("AnalogMove", 1);
		}
		else
		{
			PlayerPrefs.SetInt("AnalogMove", 0);
		}
		if (isFullScreen.isOn)
		{
			PlayerPrefs.SetInt("isFullScreen", 1);
		}
		else
		{
			PlayerPrefs.SetInt("isFullScreen", 0);
		}
	}

	public void SetFullscreen(bool isFullscreen)
	{
		ControlFreak2.CFScreen.fullScreen = isFullscreen;
	}

	public void SetResolution()
	{ 
		int resolutionIndex = resolutionDropdown.value;
        Resolution resolution = resolutions[resolutionIndex];
        Debug.Log($"Set resolution to: {resolution.width} x {resolution.height}.");
        ControlFreak2.CFScreen.SetResolution(resolution.width, resolution.height, ControlFreak2.CFScreen.fullScreen);
	}
}
