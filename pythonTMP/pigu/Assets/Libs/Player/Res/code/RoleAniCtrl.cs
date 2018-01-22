
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RoleAniCtrl : MonoBehaviour {

    float speed;
    float MAXSpeed = 2f;
    float curAtkTime = 0;

    public float[] continuousFightingTime = { 1.2f,1.2f,1.2f};

    public Animator _animator;
    public CharacterController _characterController;

    public int curAtkIndex = 0;
    public int cacheAtkIndex = 0;

    public Camera followCamera;
    public Vector3 toCamera;

    // Use this for initialization
    void Start () {

        if (continuousFightingTime == null || continuousFightingTime.Length == 0) {
            continuousFightingTime =new float[]{ 1.2f,1.2f,1.2f};
        }

        if (_animator == null)
        {
            _animator = this.GetComponent<Animator>();
        }
        if (_animator == null)
        {
            _animator = transform.GetComponentInChildren<Animator>();
        }

        Assert.IsNotNull(_animator);

        if (_animator == null) { 
            Debug.LogError("_animator 为空!");
        }

        if (_characterController == null) {
            _characterController = this.GetComponent<CharacterController>();
        }
        if (_characterController == null){
            _characterController = this.gameObject.AddComponent<CharacterController>();
        }

        if (followCamera){
           toCamera = followCamera.transform.position - transform.position;
        }
    }

    public bool IsForwardPlay() {
        if ((continuousFightingTime[curAtkIndex] - curAtkTime) < continuousFightingTime[curAtkIndex] * .5f) {
            return true;
        }
        return false;
    }

    public void Attack() {
     
            if (IsForwardPlay())
            {
                Debug.LogError("前摇动作 不能打断 curAtkTime = " + curAtkTime + ",curAtkIndex = " + curAtkIndex);
                cacheAtkIndex = curAtkIndex;
                return;
            }
            else
            {
                 Debug.LogError("后摇动作打断! curAtkTime = " + curAtkTime + "curAtkIndex = " + curAtkIndex);
            }

            if (curAtkIndex == 0)
            {
                Debug.LogError(curAtkTime + ",3 curAtkIndex = " + curAtkIndex);
                curAtkTime = continuousFightingTime[curAtkIndex];
                //_animator.SetTrigger("atk_1");
                _animator.Play("atk_1");
                curAtkIndex = 1;

                Debug.LogError("curAtkIndex = " + curAtkIndex);
                return;
            }
            else
            if (curAtkIndex == 1)
            {
                //Debug.LogError(curAtkTime + ",5 curAtkIndex = " + curAtkIndex);
                curAtkTime = continuousFightingTime[curAtkIndex];
                //_animator.SetBool("isContinuousFighting", true);
                //_animator.SetTrigger("atk_2");
                //动作硬切
                _animator.Play("atk_2");
                curAtkIndex = 2;
                //Debug.LogError("curAtkIndex = "  + curAtkIndex);
                return;
            }
            else
            if (curAtkIndex == 2)
            {
                curAtkTime = continuousFightingTime[curAtkIndex];
                //Debug.LogError(curAtkTime + ",6 curAtkIndex = " + curAtkIndex);
                //curAtkTime = 0;
                //_animator.SetTrigger("atk_3");
                //动作硬切
                _animator.Play("atk_3");
                curAtkIndex = 3;
                ///Debug.LogError("curAtkIndex = " +  curAtkIndex);
                return;
            }
            else
            if ((curAtkIndex == 3))
            {
                Debug.LogError("不能出发 连击终点 " + curAtkTime + ",4 curAtkIndex = " + curAtkIndex);
                return;
            }
            else
            {
                Debug.LogError(" 未知错误！！！！！ 无效路径" + curAtkTime + ", curAtkIndex = " + curAtkIndex);
                Assert.IsTrue(false);
            }
       // }
    }
#if UNITY_EDITOR
    public void OnGUI()
    {
        GUIStyle titleStyle2 = new GUIStyle();
        titleStyle2.fontSize = 20;
        if (curAtkTime > 0)
        {
            if (IsForwardPlay())
                titleStyle2.normal.textColor = new Color(1, 0, 0, 1);
            else
                titleStyle2.normal.textColor = new Color(0, 1, 0, 1);
        }
        else {
            titleStyle2.normal.textColor = new Color(1, 1, 1, 1);
        }

        GUI.Label(new Rect(0, 0, 240, 70), string.Format("curAtkIndex {0} ",curAtkIndex), titleStyle2);
        GUI.Label(new Rect(0,20,240,70),   string.Format("time {0}"        ,(continuousFightingTime[curAtkIndex] - curAtkTime)), titleStyle2);
    }
#endif
    public void Skill(int index) {

        _animator.SetTrigger(string.Format("skill_{0}", index + 1));
    }

    float speedAdd = 0f;

    // Update is called once per frame
    void Update () {

        if (curAtkTime > 0)
        {
            curAtkTime -= Time.deltaTime;
            //Debug.LogError(curAtkTime + ",curAtkIndex = " + curAtkIndex);
        } else {
            _animator.SetBool("isContinuousFighting", false);
            curAtkIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Skill(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Skill(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Skill(2);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Skill(3);
        }

        if (followCamera) {

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                //speed = 2f;
                //加速度控制 16 分之 1 的
                speedAdd = MAXSpeed * 1f/16f;
            }
            if (Input.GetKeyUp(KeyCode.Alpha9))
            {
                //加速度控制
                speedAdd = MAXSpeed * -1f /16f;
            }
            //减速控制
            if (speedAdd < 0 && 0 < Mathf.Abs(speed))
            {
                speed += speedAdd;

                Debug.LogError("++ speed = "+ speed);
            }
            //加速控制
            if (speedAdd > 0 && Mathf.Abs(speed) < MAXSpeed)
            {
                speed += speedAdd;

                Debug.LogError("-- speed = " + speed);
            }

            if (IsForwardPlay()) {
                //speed = 0f;
                //前摇动画
               // return;
            }

            if (speed != 0)
            {
                _characterController.Move(new Vector3(0, 0, speed * Time.deltaTime));
                followCamera.transform.position = transform.position + toCamera;
            }
            //根据速度比控制融合树
            _animator.SetFloat("Blend", speed / MAXSpeed);
        }
    }
}
