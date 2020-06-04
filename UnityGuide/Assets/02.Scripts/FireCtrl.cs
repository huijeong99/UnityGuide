using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;
    public ParticleSystem Cartridge;
    private ParticleSystem MuzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        MuzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }   
    }

    private void Fire()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
        Cartridge.Play();
        MuzzleFlash.Play();
    }
}
