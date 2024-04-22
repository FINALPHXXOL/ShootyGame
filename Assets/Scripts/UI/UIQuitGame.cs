using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuitGame : MonoBehaviour
{
    public void QuitGame()
    {
    #if UNITY_STANDALONE
            Application.Quit();
    #endif
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
