using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAnimatorController : MonoBehaviour {

    private Animator _animator;
  
     void Start()
     {
         _animator = this.GetComponent<Animator>();
     }

    float time;

    void Update()
    {
        // 单次动画播放
        if (Input.GetKeyDown(KeyCode.X))
        {
            //立即播放
            _animator.Play("Attack2");
            time = 0f;
        }

        // 单次动画播放
        if (Input.GetKeyDown(KeyCode.D))
        {
            //触发播放
            _animator.SetTrigger("Attack2Trigger");
            time = 0f;
        }

        // 单次动画播放
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetBool("Attack1", true);
            time = 0f;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _animator.SetBool("Attack1", false);
        }
        // 状态循环 
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetBool("Run", true);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            _animator.SetBool("Run", false);
        }

        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
        if (state.shortNameHash == Animator.StringToHash("Run") && Input.GetKeyDown(KeyCode.J))
        {
            _animator.SetTrigger("jump");
        }
        // 单次动画播放 完成
        if (state.shortNameHash != Animator.StringToHash("Idle"))
        {
            _animator.SetFloat("Idle", time);
            time += Time.deltaTime;
        }

    }
}
