using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimChange : MonoBehaviour {

    public Animator _animator;

    void Start()
    {
        if(_animator == null)
        _animator = this.GetComponent<Animator>();

        _animator.Play("Blend Tree");

       // _animator
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _animator.Play("atk_1"); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _animator.Play("atk_2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _animator.Play("atk_3");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _animator.Play("skill_1");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _animator.Play("skill_2");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _animator.Play("skill_3");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.Play("skill_4");
        }
       
    }
}
