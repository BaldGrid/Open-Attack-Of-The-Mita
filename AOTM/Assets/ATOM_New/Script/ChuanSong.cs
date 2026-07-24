using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleAutoTeleport : MonoBehaviour
{
    public string sceneToLoad = "BossFight2";
    
    void OnEnable()
    {
        // 直接加载场景
        if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError($"无法加载场景: {sceneToLoad}");
        }
    }
}