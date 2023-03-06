using System;
using UnityEngine;


public class IaController : MonoBehaviour
{
    float rangePatrol = 5f;
    float rangeAttack = 3f;

    enum State
    {
        Idle,
        Chase,
        Attack,
        Defend
    }

    State state = State.Idle;

    FighterBase fighterBase;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangePatrol);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
    }

    void Start()
    {
        fighterBase = GetComponent<FighterBase>();
    }

    void Update()
    {
    }

    bool IsEnemyInRange()
    {
        return Vector2.Distance(transform.position, fighterBase.Enemy.transform.position) < rangePatrol;
    }

    bool IsEnemyInAttackRange()
    {
        return Vector2.Distance(transform.position, fighterBase.Enemy.transform.position) < rangeAttack;
    }

    bool CanEnemyAttack()
    {
        return fighterBase.Enemy.CanAttack;
    }

    void RandomAttack()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            fighterBase.Punch();
        }
        else
        {
            fighterBase.Kick();
        }
    }

    void FixedUpdate()
    {
        if (fighterBase.IsDead || fighterBase.Enemy.IsDead)
        {
            fighterBase.Move(0);
            state = State.Idle;
            return;
        }

        if (!IsEnemyInRange())
        {
            state = State.Chase;
        }
        else if (CanEnemyAttack() && IsEnemyInAttackRange())
        {
            // random state between defend and attack
            state = UnityEngine.Random.Range(0, 2) == 0 ? State.Attack : State.Defend;
        }
        else if (IsEnemyInAttackRange())
        {
            state = fighterBase.CanAttack ? State.Attack : State.Defend;
        }
        

        var enemyDirection = fighterBase.GetEnemyDirection();
        switch (state)
        {
            case State.Idle:
                fighterBase.Move(0);
                break;
            case State.Chase:
                fighterBase.Move(enemyDirection);
                break;
            case State.Attack:
                // fighterBase.Move(enemyDirection);
                RandomAttack();
                break;
            case State.Defend:
                fighterBase.Move(-enemyDirection);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}