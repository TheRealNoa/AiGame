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
    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject flscript = GameObject.Find("Flashlight");
        flashlight = flscript.GetComponent<FlashlightToggle>();
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
        else if (attacked && !attack_state && !flashlight.specialIsOn)
        {
            anim.SetBool("Attack", false);
            anim.SetBool("AttackToIdle", true);
            AttackToIdle_state = true;
            attacked = false;
        }
        else if (flashlight.specialIsOn)
        {
            anim.SetBool("Attack", false);
            anim.SetBool("FallingBack", true);
            specialFallBack = false;
            specialFallBackToIdle = false;
        } 
    }
    public void setAttackState(bool state)
    {
        this.attack_state = state;
    }
}
