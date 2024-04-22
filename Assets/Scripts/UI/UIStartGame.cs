using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartGame : MonoBehaviour
{
    public void LoadGameScene()
    {
        // Load game scene
        
        SceneManager.LoadScene("GameplayScene");
    }
}
