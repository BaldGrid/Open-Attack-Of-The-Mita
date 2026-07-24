using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using TMPro;

public class InputChangerManager : MonoBehaviour
{
	public TMP_InputField seedIF;
	public TMP_InputField percentIF;
	
    void Start()
    {
        
    }
	
    void Update()
    {
		string seedString = seedIF.text;
		string percentString = percentIF.text;
		
		int.TryParse(seedString.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out int seedNumber);
		float.TryParse(percentString.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out float percentNumber);
		
        Shader.SetGlobalFloat("_Seed", seedNumber);
		Shader.SetGlobalFloat("_Percent", percentNumber);
	}
}
