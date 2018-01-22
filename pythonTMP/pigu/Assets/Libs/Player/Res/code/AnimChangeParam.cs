using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimChangeParam : MonoBehaviour {

    public Animator _animator;

    void Start()
    {
        if (_animator == null)
            _animator = this.GetComponent<Animator>();

        //_animator.Play("Blend Tree");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //_animator.SetInteger("atk", 1);
            _animator.SetTrigger("atk_1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //_animator.SetInteger("atk", 2);
            _animator.SetTrigger("atk_2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //_animator.SetInteger("atk", 3);
            _animator.SetTrigger("atk_3");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //_animator.SetInteger("skill", 1);
            _animator.SetTrigger("skill_1");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //_animator.SetInteger("skill", 2);
            _animator.SetTrigger("skill_2");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //_animator.SetInteger("skill", 3);
            _animator.SetTrigger("skill_3");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //_animator.SetInteger("skill", 4);
            _animator.SetTrigger("skill_4");
        }

      
        /*
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
        if (state.shortNameHash == Animator.StringToHash("Run")){

        }
        else{
            _animator.SetInteger("atk", 0);
            _animator.SetInteger("skill", 0);
        }*/

    }
}
