﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        SceneManager.LoadScene("SceneLoader");
        //SceneManager.LoadScene("Play", LoadSceneMode.Additive);
    }
}
