using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoleMoveAnimationPlayer{
    void PlayRun(float speed, float maxSpeed);
    void PlayIdle();
}

public class RoleMoveAnimationPlayer : MonoBehaviour, IRoleMoveAnimationPlayer
{
    virtual public void PlayRun(float speed, float maxSpeed) { }
    virtual public void PlayIdle() { }
}

public class DefaultMoveAnimationPlayer:RoleMoveAnimationPlayer
{
    Animator _animator;
    Animation _animation;

    void Start(){
        _animator = gameObject.GetComponentInChildren<Animator>();
        _animation = gameObject.GetComponentInChildren<Animation>();
    }

    override public void PlayRun(float speed, float maxSpeed) { 
        if (_animator){
            _animator.SetFloat("Blend",speed / maxSpeed);
        }
        else if(_animation){

        }
    }

    override public void PlayIdle() {
        if (_animator){
            _animator.SetFloat("Blend",0);
        }
        else if(_animation){

        }
    }
}
