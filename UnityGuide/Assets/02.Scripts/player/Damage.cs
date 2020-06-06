using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{

    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";

    private float initHP = 100.0f;
    public float currHP;
    public Image bloodScreen;

    public Image hpBar;

    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    private Color currColor;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    // Start is called before the first frame update
    void Start()
    {
        currHP = initHP;

        hpBar.color = initColor;
        currColor = initColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            Destroy(other.gameObject);

            StartCoroutine(ShowBloodScreen());

            currHP -= 5.0f;
            Debug.Log("Player HP=" + currHP.ToString());

            DisplayHpbar();

            if (currHP <= 0.0f)
            {
                playerDie();
            }
        }
    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.2f,0.3f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }

    void playerDie()
    {
        OnPlayerDie();

        Debug.Log("PlayerDie !");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        
        for(int i = 0; i < enemies.Length; i++) {
            enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void DisplayHpbar()
    {
        if ((currHP / initHP) > 0.5f)
            currColor.r = (1 - (currHP / initHP)) * 2.0f;
        else
            currColor.g = (currHP / initHP) * 2.0f;
    
        hpBar.color = currColor;
        hpBar.fillAmount = (currHP / initHP);
    }
}
