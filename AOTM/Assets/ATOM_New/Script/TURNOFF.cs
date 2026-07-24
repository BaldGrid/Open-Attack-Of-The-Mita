using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleObjectWithTab_NewInputSystem : MonoBehaviour
{
    [Header("设置")]
    public GameObject targetObject;
    
    [Header("输入设置")]
    public InputAction toggleAction = new InputAction("ToggleObject", 
        InputActionType.Button, "<Keyboard>/tab");  // 绑定 Tab 键

    [Header("状态")]
    [SerializeField] private bool isObjectActive = true;

    void OnEnable()
    {
        // 启用输入动作
        toggleAction.Enable();
        toggleAction.performed += OnTogglePerformed;
    }

    void OnDisable()
    {
        // 禁用输入动作
        toggleAction.performed -= OnTogglePerformed;
        toggleAction.Disable();
    }

    void Start()
    {
        if (targetObject != null)
        {
            isObjectActive = targetObject.activeSelf;
        }
        else
        {
            Debug.LogError("请分配一个要控制的对象！");
        }
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        ToggleObject();
    }

    void ToggleObject()
    {
        if (targetObject == null) return;

        isObjectActive = !isObjectActive;
        targetObject.SetActive(isObjectActive);
        
        Debug.Log(targetObject.name + " 状态: " + (isObjectActive ? "开启" : "关闭"));
    }

    public void EnableObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            isObjectActive = true;
        }
    }

    public void DisableObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            isObjectActive = false;
        }
    }
}