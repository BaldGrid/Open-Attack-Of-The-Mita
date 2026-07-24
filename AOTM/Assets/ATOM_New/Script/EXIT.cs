using UnityEngine;

public class ExitOnAnyKey : MonoBehaviour
{
    void Update()
    {
        // 检测任意按键按下
        if (Input.anyKeyDown)
        {
            // 退出游戏
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}