using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 必须添加在摄像机上
/// </summary>
public class SkillReleaseRange : MonoBehaviour {
    //特效 effect 
    //public string assetBundlePath  =  "ground 13.effect";
    //public AssetBundle assetBundle;
    //public string assetName =  "ground 13";
    GameObject effectGameObject;

    public bool visible = false;
    //目标 Transform
    public Transform target;
    //释放点
    public Vector3 releasePot = new Vector3(0, 0, 0);
    //释放距离
    public float distance = 4;
    //范围夹角
    public float rangeAngle = 360;
    //画线的材质 不设定系统会用当前材质画线 结果不可控
    public Material mat = null;
    //把弧长分为 18 等分需要 19 点控制 所以数组长度 19 + 1（原点） 
    public Vector3[] circleVertices = new Vector3[ 19 + 1 ];

    public string targetTag = "Player";

    public bool autoFindTarget = false;

    public SkillAttackRange skillAttackRange;

    // Use this for initialization
    virtual public void Start ()
    {
        InitCircleVertices();
        UpdateCircleVertices();

   
    }
    /// <summary>
	/// 创建顶点数组
    /// </summary>
    virtual public void InitCircleVertices()
    {
        for (int i = 0; i < circleVertices.Length; i++)
        {
            circleVertices[i] = new Vector3(0, 0.1f, 0);
        }
    }
    /// <summary>
    /// 更新顶点数组
    /// </summary>
    virtual public void UpdateCircleVertices()
    {
        if (target != null)
        {
            //根据目标对象坐标设置释放点
            releasePot = target.position;
        }
        int ds = 0;
        if (effectGameObject != null) {
            effectGameObject.transform.position = releasePot;
            effectGameObject.transform.localScale = new Vector3(distance ,  1, distance);
            effectGameObject.SetActive( visible );
        }

        //把弧长分为 18 等分需要 19 点控制 所以数组长度 19 + 1（原点） 
        //angleSegment 角度的等分
        float angleSegment = rangeAngle / (circleVertices.Length - 2);
        //用距离设置半径
        float R = distance;

        for (int i = 1; i < circleVertices.Length; i++)
        {
            //float deg = (i - 1) * 10f; 
            //下标每次 angleSegment 角度的等分,角度递增
            float deg = (i - 1) * angleSegment;
            float x = Mathf.Cos(deg * Mathf.Deg2Rad) * R;
            float y = Mathf.Sin(deg * Mathf.Deg2Rad) * R;
            //Vector3 p = new Vector3(x, 0, y);

            circleVertices[i].x = x;
            circleVertices[i].z = y;
        }
    }

    // Update is called once per frame
    virtual public void Update () {

        if (autoFindTarget && target == null)
        {
            GameObject targetGo = GameObject.FindGameObjectWithTag(targetTag);
            if (targetGo)
                target = targetGo.transform;
        }
        if (skillAttackRange == null)
        {
            skillAttackRange = GetComponent<SkillAttackRange>();
        }
        else {
            distance = skillAttackRange.distance;
        }

        UpdateCircleVertices();
    }

    virtual public void DrawRange() {
        GL.Begin(GL.LINES);
        for (int i = 1; i < circleVertices.Length - 1; ++i)
        {
            GL.Vertex(circleVertices[i] + releasePot);
            GL.Vertex(circleVertices[i + 1] + releasePot);
        }
        //终点连接起始点
        //GL.Vertex(circleVertices[circleVertices.Length - 1] + releasePot);
        //GL.Vertex(circleVertices[0] + releasePot);

        GL.End();
    }

    virtual public void OnPostRender()
    {
        if (!visible) return;

        GL.Color(Color.red);
        GL.PushMatrix();

        if (mat) mat.SetPass(0);

        DrawRange();

        GL.PopMatrix();
    }

    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.dataPath + "/StreamingAssets/";
                break;
        }
        return path;
    }
}
