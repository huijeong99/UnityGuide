using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private const string bulletTag="BULLET";
    private float hp = 100.0f;
    private GameObject bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == bulletTag)
        {
            ShowBloodEffect(collision);
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            if (hp<=0.0f){
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            }
        }
    }

    void ShowBloodEffect(Collision collision)
    {
        Vector3 pos = collision.contacts[0].point;
        Vector3 _normal = collision.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}
