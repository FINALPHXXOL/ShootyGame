using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICredits : MonoBehaviour
{
    public void LoadCreditsScene()
    {
        // Load credits scene
        SceneManager.LoadScene("Credits");
    }
}
