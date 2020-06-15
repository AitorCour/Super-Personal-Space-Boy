using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private InputManager inputManager;
    private UI_Manager ui;
    private GlitchEffect glitch;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();
        glitch = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GlitchEffect>();
        inputManager.Initialize();
        ui.Initialize();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        //glitch.AllUp();
        glitch.enabled = true;
    }
    public void Resume()
    {
        Time.timeScale = 1;
        //glitch.AllNormal();
        glitch.enabled = false;
    }
}
