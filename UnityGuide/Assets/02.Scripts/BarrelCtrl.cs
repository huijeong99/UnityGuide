using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{

    public GameObject expEffect;
    public Mesh[] meshes;//찌그러진 드럼통의 메쉬를 저장할 배열
    public Texture[] textures;//드럼통의 텍스처를 저장할 배열

    private int hitCount = 0;
    private Rigidbody rb;
    private MeshFilter meshFiler;
    private MeshRenderer _renderer;

    public float expRadius = 10.0f;//폭발반경

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshFiler = GetComponent<MeshFilter>();
        //meshrenderer컴포넌트 추출
        _renderer = GetComponent<MeshRenderer>();
        //난수를 발생시켜 불규칙한 텍스처 적용
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            if (++hitCount == 3)
            {
                expBarrel();
            }
        }
    }

    void expBarrel()
    {
       GameObject effect= Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2.0f);

        //rb.mass = 1.0f;
        //rb.AddForce(Vector3.up * 1000.0f);

        //폭발력 생성
        IndirectDamage(transform.position);

        int idx = Random.Range(0, meshes.Length);
        meshFiler.sharedMesh = meshes[idx];
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];
    }

    void IndirectDamage(Vector3 pos)
    {
        //주변에 있는 드럼통을 추출
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);

        foreach(var coll in colls)
        {
            //폭발범위 내에 있는 드럼통의 RigidBody 컴포넌트를 추출
            var _rb = coll.GetComponent<Rigidbody>();
            //드럼통의 무게 변경
            _rb.mass = 1.0f;
            //폭발력을 전다함
            _rb.AddExplosionForce(1200.0f, pos, expRadius, 1000.0f);
        }
    }
}
