using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.PATROL;

    private Transform playerTr;
    private Transform enemyTr;
    private Animator animator;

    //공격 사정거리
    public float attackDist = 5.0f;
    //추적 사정거리
    public float traceDist = 10.0f;

    public bool isDie = false;

    private WaitForSeconds ws;
    private MoveAgent moveAgent;
    private EnemyFire enemyFire;

    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        if (player != null)
        
        playerTr = player.GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();
        enemyFire = GetComponent<EnemyFire>();

        ws=new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) yield break;

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }

            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);

                    if (enemyFire.isFire == false)
                        enemyFire.isFire = true;
                    break;
                case State.DIE:
                    isDie = true;
                    enemyFire.isFire = false;

                    moveAgent.Stop();

                    animator.SetInteger(hashDieIdx, Random.Range(0, 3));
                    animator.SetTrigger(hashDie);

                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
        }
    }

    private void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
