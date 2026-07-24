using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapScript : MonoBehaviour
{
    public GameObject map;
    public InputActionReference toggleMapAction; // 在 Inspector 中分配
    private bool isMapOpen = false;

    void Start()
    {
        StartCoroutine(sequence());
        toggleMapAction.action.performed += OnToggleMap;
        toggleMapAction.action.Enable();
    }

    private IEnumerator sequence()
    {
        yield return new WaitForSeconds(0.05f);
        map.SetActive(false);
    }

    private void OnToggleMap(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0f)
        {
            isMapOpen = !isMapOpen;
            map.SetActive(isMapOpen);
        }
    }

    void OnDestroy()
    {
        toggleMapAction.action.performed -= OnToggleMap;
        toggleMapAction.action.Disable();
    }
}