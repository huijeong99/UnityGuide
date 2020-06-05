using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}

public class playerCtrl : MonoBehaviour
{
    private float h;
    private float v;
    private float r;

    private Transform tr;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 80.0f;

    public PlayerAnim playerAni;
    public Animation anime;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        anime = GetComponent<Animation>();
        anime.clip = playerAni.idle;
        anime.Play();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        //tr.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime, Space.Self);
        //tr.Translate(-Vector3.left * moveSpeed * h * Time.deltaTime, Space.Self);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        //캐릭터 회전
        //transform.Rotate(Vector3.up * Time.deltaTime);
        //transform.Rotate(0, Time.deltaTime, 0);
        //transform.Rotate(Vector3.up, Time.deltaTime);

        //마우스를 이용한 회전 계산
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        //애니메이션 설정
        if (v >= 0.1f)
        {
            anime.CrossFade(playerAni.runF.name, 0.3f);
        }

        else if (v <= -0.1f)
        {
            anime.CrossFade(playerAni.runB.name, 0.3f);
        }

        else if (h >= 0.1f)
        {
            anime.CrossFade(playerAni.runR.name, 0.3f);
        }
        
        else if (h <= -0.1f)
        {
            anime.CrossFade(playerAni.runL.name, 0.3f);
        }
        else
        {
            anime.CrossFade(playerAni.idle.name, 0.3f);
        }
    }
}
