using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    enum STATE
    {
        WaitPatrol,     // ���� �� ���.
        Patrol,         // ���� ��.
        Chase,          // ���� ��.
        Attack,         // ���� ��.
    }

    [Header("Target")]
    [SerializeField] STATE state;               // ���� ����.
    [SerializeField] Transform player;
    [SerializeField] LayerMask targetMask;

    [Header("Range")]
    [SerializeField] float patrolRadius;
    [SerializeField] float searchRadius;
    [SerializeField] float attackRadius;

    [Header("Time")]
    [SerializeField] float waitNextPatrolTime;  // ���� ���� �ð�.
    [SerializeField] float waitNextAttackTime;  // ���� ���� �ð�.

    [Header("Weapon")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform bulletPivot;
    [SerializeField] float bulletSpeed;

    private bool isInSearchRange;       // Ž�� ������ ���Դ�.
    private bool isInAttackRange;       // ���� ������ ���Դ�.

    private Vector3 patrolPos;
    private Vector3 origionPos;         // ���� ��ġ.
    private NavMeshAgent agent;         // �׺� �޽�.

    private float nextPatrolTime;       // ���� ���� �ð�.
    private float nextAttackTime;       // ���� ���� �ð�.

    private void Start()
    {
        origionPos = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        isInSearchRange = Physics.CheckSphere(transform.position, searchRadius, targetMask);
        isInAttackRange = Physics.CheckSphere(transform.position, attackRadius, targetMask);

        switch(state)
        {
            case STATE.WaitPatrol:
                WaitPatrol();
                break;
            case STATE.Patrol:
                Patrol();
                break;
            case STATE.Chase:
                ChaseToPlayer();
                break;
            case STATE.Attack:
                AttackToPlayer();
                break;
        }
    }
    private void WaitPatrol()
    {
        // ���� ���� �ð����� ���.
        if(nextPatrolTime <= Time.time)
        {
            state = STATE.Patrol;

            // ������ ���� Vector3 �������� 0.0~1.0 ���� ������ �޾ƿ´�.
            patrolPos = origionPos + (Random.insideUnitSphere * patrolRadius);
            patrolPos.y = transform.position.y;
            agent.SetDestination(patrolPos);

            Debug.Log("���� ��ġ ���� : " + patrolPos.ToString());
        }        
        else if(isInSearchRange)               // ���� �ִµ� Ž�� ������ ���Դٸ�.
        {
            state = STATE.Chase;        // �����Ѵ�.
        }
    }
    private void Patrol()
    {
        // �������� �����ߴٸ�.
        if(agent.hasPath == false)
        {
            state = STATE.WaitPatrol;
            nextPatrolTime = Time.time + waitNextPatrolTime;
        }
        else if(isInSearchRange)
        {
            state = STATE.Chase;
        }
    }

    private void ChaseToPlayer()
    {
        // ���� ������ ������ ��.
        if (isInAttackRange && IsLockOn())
        {
            agent.SetDestination(transform.position);   // ���� ���°� �Ǹ� �� ��ġ�� �����.
            state = STATE.Attack;
        }
        // Ž�� ������ ����� ��.
        else if (isInSearchRange == false)
        {
            state = STATE.Patrol;
        }
        // �÷��̾ ���� ���� ��.
        else
        {
            agent.SetDestination(player.position);
        }
    }
    private void AttackToPlayer()
    {
        if(isInAttackRange == false || IsLockOn() == false)
        {
            state = STATE.Chase;
        }
        // ���� �ð��� �Ǹ� �����Ѵ�.
        else if (nextAttackTime <= Time.time)
        {
            Attack();
            nextAttackTime = Time.time + waitNextAttackTime;
        }
    }

    private bool IsLockOn()
    {
        // ���� ��� ���̰� �շ��ִٸ� true��ȯ.
        Vector3 targetDir = (player.position - transform.position).normalized;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, targetDir);

        return Physics.Raycast(ray, out hit) && hit.collider.GetComponent<Player>() != null;
    }

    private void Attack()
    {
        Bullet bullet = Instantiate(bulletPrefab, bulletPivot.position, Quaternion.identity);
        Vector3 dir = (player.position - bulletPivot.position).normalized;
        bullet.Shoot(dir, bulletSpeed);
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(patrolPos, 0.2f);

        Vector3 pivot = Application.isPlaying ? origionPos : transform.position;

        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(pivot, Vector3.up, patrolRadius);

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRadius);

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRadius);
    }
#endif
}
