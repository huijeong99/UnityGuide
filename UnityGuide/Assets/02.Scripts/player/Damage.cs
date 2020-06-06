using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";
    private float initHP = 100.0f;
    public float currHP;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    // Start is called before the first frame update
    void Start()
    {
        currHP = initHP;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            Destroy(other.gameObject);

            currHP -= 5.0f;
            Debug.Log("Player HP=" + currHP.ToString());

            if (currHP <= 0.0f)
            {
                playerDie();
            }
        }
    }

    void playerDie()
    {
        OnPlayerDie();

        //Debug.Log("PlayerDie !");
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        //
        //for(int i = 0; i < enemies.Length; i++) {
        //    enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
    }
}
