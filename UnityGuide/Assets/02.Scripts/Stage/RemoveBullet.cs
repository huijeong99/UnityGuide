﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "BULLET")
        {
            ShowEffect(coll);
            //Destroy(coll.gameObject);
            coll.gameObject.SetActive(false);
        }
    }

    private void ShowEffect(Collision coll)
    {
        //충돌지점의 위치 정보 가져오기
        ContactPoint contact = coll.contacts[0];
        //충돌지점의 각도 가져오기
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        GameObject spark= Instantiate(sparkEffect, contact.point+(-contact.normal*0.05f), rot);
        spark.transform.SetParent(this.transform);//부모객체로 설정
    }
}
