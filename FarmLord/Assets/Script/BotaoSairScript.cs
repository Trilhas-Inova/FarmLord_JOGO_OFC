using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class BotaoSairScript : MonoBehaviour
{
    public void BotaoSair()
    {
#if UNITY_EDITOR
       UnityEditor.EditorApplication.isPlaying = false;
#else
       Application.Quit();
#endif
    }
}
