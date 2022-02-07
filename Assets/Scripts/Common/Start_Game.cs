using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Start_Game : MonoBehaviour
{
    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.enterKey.isPressed)
        {
            SceneManager.LoadScene(1);
        } else if (keyboard.escapeKey.isPressed)
        {
            Application.Quit();
        }
    }
}