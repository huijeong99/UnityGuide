using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{

    public enum WeaponType
    {
        RIFLE=0,
        SHOTGUN
    }

    public WeaponType currWeapon = WeaponType.RIFLE;

    public GameObject bullet;
    public ParticleSystem Cartridge;
    private ParticleSystem MuzzleFlash;
    private AudioSource _audio;

    public Transform firePos;
    public PlayerSfx playerSfx;

    private Shake shake;

    public Image magazineImg;
    public Text magazineText;

    public int maxBullet = 10;
    public int remainingBullet = 10;

    public float reloadTime = 2.0f;
    private bool isReloading = false;

    public Sprite[] weaponIcons;
    public Image weaponImage;
    private int enemyLayer;
    private int obstacleLayer;
    private int layerMask;

    private bool isFire = false;
    private float nextFire;
    public float fireRate = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        MuzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();

        enemyLayer = LayerMask.NameToLayer("ENEMY");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");

        layerMask = 1 << obstacleLayer | 1 << enemyLayer;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(firePos.position, firePos.forward * 20.0f, Color.green);

        if (EventSystem.current.IsPointerOverGameObject()) return;

        RaycastHit hit;

        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 20.0f, layerMask))
        {
            isFire = (hit.collider.CompareTag("ENEMY"));
        }
        else
        {
            isFire = false;
        }

        if (!isReloading && isFire)
        {
            if (Time.time > nextFire)
            {
                --remainingBullet;
                Fire();

                if (remainingBullet == 0)
                {
                    StartCoroutine(Reloading());
                }

                nextFire = Time.time + fireRate;
            }
        }

        if (!isReloading && Input.GetMouseButtonDown(0))
        {
            --remainingBullet;
            Fire();

            if (remainingBullet == 0)
            {
                StartCoroutine(Reloading());
            }
        }   
    }

    private void Fire()
    {
        StartCoroutine(shake.ShakeCamera());
        
        Instantiate(bullet, firePos.position, firePos.rotation);
        var _bullet = GameMgr.instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }

        Cartridge.Play();
        MuzzleFlash.Play();
        FireSfx();

        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
        UpdateBulletText();
    }

    private void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currWeapon];
        _audio.PlayOneShot(_sfx, 1.0f);
    }
    
    IEnumerator Reloading()
    {
        isReloading = true;
        _audio.PlayOneShot(playerSfx.reload[(int)currWeapon], 1.0f);

        yield return new WaitForSeconds(playerSfx.reload[(int)currWeapon].length + 0.3f);

        isReloading = false;
        magazineImg.fillAmount = 1.0f;
        remainingBullet = maxBullet;
        UpdateBulletText();
    }

    void UpdateBulletText()
    {
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/{1}", remainingBullet, maxBullet);
    }

    public void OnChangeWeapon()
    {
        currWeapon = (WeaponType)((int)++currWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)currWeapon];
    }
}
