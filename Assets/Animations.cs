using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Animations : MonoBehaviour
{
    private Animator anim;
    public string idle;
    public string attack;
    public bool attack_state;
    public bool attacked;
    public bool ranAway;

    public bool isBlocking;
    // Start is called before the first frame update
    FlashlightToggle flashlight;
    EnemyFollow enemyFollow;
    EnemyFollow.State currentState;
    EnemyFollow.State fleeState;
    EnemyFollow.State chaseState;
    EnemyFollow.State patrolState;

    public NavMeshAgent agent;



    public bool chased;
    public bool fled;
    public bool damaged;
    public bool patrolled;

    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject enemy = GameObject.Find("Enemy");
        enemyFollow = enemy.GetComponent<EnemyFollow>();

        agent = enemy.GetComponent<NavMeshAgent>();

        //EnemyFollow.State currentState = enemyFollow.GetCurrentState();
    }

    // Update is called once per frame
    void Update()
    {


        GameObject flscript = GameObject.Find("Flashlight");
        if(flscript != null )
        {
            flashlight = flscript.GetComponent<FlashlightToggle>();
        }

        EnemyFollow.State fleeState = EnemyFollow.State.Flee;
        EnemyFollow.State chaseState = EnemyFollow.State.Chase;
        EnemyFollow.State patrolState = EnemyFollow.State.Patrol;
        EnemyFollow.State respawningState = EnemyFollow.State.NotSpawned;


        EnemyFollow.State currentState = enemyFollow.GetCurrentState();
        if (currentState == fleeState)
        {
            anim.SetBool("RunToWalk", false);
            anim.SetBool("RunToAttack", false);
            anim.SetBool("IdleToWalk", false);
            anim.SetBool("AttackToRun", false);
            anim.SetBool("WalkToRun", true);
            if (damaged)
            {
                anim.SetBool("RunToAttack", false);
                anim.SetBool("WalkToRun", false);
                anim.SetBool("AttackToRun", true);
                damaged = false;
            }

            fled = true;
        }
        if(currentState == chaseState)
        {
            if (agent.remainingDistance > enemyFollow.attackDistance && !damaged)
            {
                anim.SetBool("RunToAttack", false);
                anim.SetBool("IdleToWalk", false);
                anim.SetBool("RunToWalk", false);
                anim.SetBool("WalkToAttack", false);
                anim.SetBool("WalkToRun", true);
            }else
            if(agent.remainingDistance <= enemyFollow.attackDistance && patrolled)
            {
                anim.SetBool("AttackToRun", false);
                anim.SetBool("WalkToRun", false);
                anim.SetBool("WalkToAttack", true);
                anim.SetBool("RunToAttack", true);
                damaged = true;
                patrolled = false;
            }else
            if (agent.remainingDistance <= enemyFollow.attackDistance && !patrolled && !damaged) 
            {
                anim.SetBool("AttackToRun", false);
                anim.SetBool("WalkToRun", false);
                anim.SetBool("WalkToAttack", true);
                anim.SetBool("RunToAttack", true);
                damaged = true;

            }
            else
                if (agent.remainingDistance > enemyFollow.attackDistance && damaged)
                {
                    anim.SetBool("RunToAttack", false);
                    anim.SetBool("WalkToAttack", false);
                    anim.SetBool("IdleToWalk", false);
                    anim.SetBool("RunToWalk", false);
                    anim.SetBool("AttackToRun", true);
                    damaged = false;
                }

        }
        else if(currentState == patrolState)
        {
            if (!fled)
            {
                anim.SetBool("RunToAttack", false);
                anim.SetBool("WalkToRun", false);
                anim.SetBool("IdleToWalk", true);
                patrolled = true;
            }
            else if(fled)
            {
                anim.SetBool("RunToAttack", false);
                anim.SetBool("WalkToRun", false);
                anim.SetBool("RunToWalk", false);
                anim.SetBool("IdleToWalk", true);
                patrolled = true;
            }
            else if (damaged) 
            {
                anim.SetBool("IdleToWalk", false);
                anim.SetBool("RunToWalk", false);
                anim.SetBool("AttackToWalk", true);
                patrolled = true;
            }
            else
            {
                anim.SetBool("IdleToWalk", true);
                anim.SetBool("RunToWalk", false);
                anim.SetBool("AttackToWalk", false);
            }

        }
        else if(currentState == respawningState)
        {
            anim.SetBool("RunToAttack", false);
            anim.SetBool("WalkToAttack", false);
            anim.SetBool("IdleToWalk", true);
            anim.SetBool("RunToWalk", false);
            anim.SetBool("AttackToRun", false);
        }
       if (enemyFollow.isFleeingDueToFlashlight)
        {
            Debug.LogWarning("Enemy stunned.");
            anim.SetBool("RunToWalk", false);
            anim.SetBool("RunToAttack", false);
            anim.SetBool("IdleToWalk", false);
            anim.SetBool("AttackToRun", false);
            anim.SetBool("WalkToRun", false);
            anim.SetBool("RunToBlock", true);
            anim.SetBool("AttackToBlock", true);
            isBlocking = true;
        }else if(isBlocking && !enemyFollow.isFleeingDueToFlashlight)
        {
            anim.SetBool("RunToWalk", false);
            anim.SetBool("RunToAttack", false);
            anim.SetBool("IdleToWalk", false);
            anim.SetBool("AttackToRun", false);
            anim.SetBool("WalkToRun", false);
            anim.SetBool("RunToBlock", false);
            anim.SetBool("AttackToBlock", false);
            anim.SetBool("BlockToRun", true);
            isBlocking = false;
        }
        
    }
    public void setAttackState(bool state)
    {
        this.attack_state = state;
    }
}
