using UnityEngine;

public class RestartButton : MonoBehaviour
{
    private SceneChanging _sceneChanging;

    private void Start()
    {
        _sceneChanging = FindObjectOfType<SceneChanging>();
    }
    
    public void ChangeLevelStart()
    {
        _sceneChanging.RestartButton();
    }
}
