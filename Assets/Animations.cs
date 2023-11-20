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
    // Start is called before the first frame update
    FlashlightToggle flashlight;
    SC_NPCFollow enemyFollow;
    SC_NPCFollow.State currentState;
    SC_NPCFollow.State fleeState;
    SC_NPCFollow.State chaseState;
    SC_NPCFollow.State patrolState;

    public NavMeshAgent agent;



    public bool chased;
    public bool fled;
    public bool damaged;
    public bool patrolled;

    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject flscript = GameObject.Find("Flashlight");
        flashlight = flscript.GetComponent<FlashlightToggle>();
        GameObject enemy = GameObject.Find("Enemy");
        enemyFollow = enemy.GetComponent<SC_NPCFollow>();

        agent = enemy.GetComponent<NavMeshAgent>();

        //SC_NPCFollow.State currentState = enemyFollow.GetCurrentState();
    }

    // Update is called once per frame
    void Update()
    {

        SC_NPCFollow.State fleeState = SC_NPCFollow.State.Flee;
        SC_NPCFollow.State chaseState = SC_NPCFollow.State.Chase;
        SC_NPCFollow.State patrolState = SC_NPCFollow.State.Patrol;


        SC_NPCFollow.State currentState = enemyFollow.GetCurrentState();
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
                anim.SetBool("WalkToAttack", false);
                anim.SetBool("RunToAttack", true);
                damaged = true;
            }else
            if (agent.remainingDistance <= enemyFollow.attackDistance && !patrolled) 
            {
                anim.SetBool("AttackToRun", false);
                anim.SetBool("WalkToRun", false);
                anim.SetBool("WalkToAttack", true);
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
                anim.SetBool("RunToWalk", true);
                patrolled = true;
            }
            else if (damaged) 
            {
                anim.SetBool("IdleToWalk", false);
                anim.SetBool("RunToWalk", false);
                anim.SetBool("AttackToWalk", true);
                patrolled = true;
            }

        }
            
        
    }
    public void setAttackState(bool state)
    {
        this.attack_state = state;
    }
}
