using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator anim;
    public string idle;
    public string attack;
    public bool attack_state;
    public bool attacked;
    public bool AttackToIdle_state;
    public bool specialFallBack;
    public bool specialFallBackToIdle;
    // Start is called before the first frame update
    FlashlightToggle flashlight;
    SC_NPCFollow enemyFollow;
    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject flscript = GameObject.Find("Flashlight");
        flashlight = flscript.GetComponent<FlashlightToggle>();
        GameObject enemy = GameObject.Find("Enemy");
        enemyFollow = enemy.GetComponent<SC_NPCFollow>();
    }

    // Update is called once per frame
    void Update()
    {

        if (attack_state && !flashlight.specialIsOn)
        {
            anim.SetBool("FallingBack", false);
            anim.SetBool("Attack", true);
            anim.SetBool("AttackToIdle", false);
            AttackToIdle_state = false;
            attacked = true;

        }
    }
    public void setAttackState(bool state)
    {
        this.attack_state = state;
    }
}
