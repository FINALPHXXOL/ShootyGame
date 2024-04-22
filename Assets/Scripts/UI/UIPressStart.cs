using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPressStart : MonoBehaviour
{
    public void LoadMenuScene()
    {
        // Load menu scene
        SceneManager.LoadScene("MainMenu");
    }
}