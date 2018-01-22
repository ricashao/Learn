using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 技能攻击范围类型
/// </summary>
public enum SkillAttackRangeType {
    Sector,
    Rect,
    Circle,
    Dot,
    SelfCircle
}
/// <summary>
/// 必须添加在摄像机上 thc 2017年8月11日 09:49:31
/// </summary>
public class SkillAttackRange : MonoBehaviour {

    public bool visible = false;
    //当前技能范围类型
    public SkillAttackRangeType skillAttackRangeType = SkillAttackRangeType.Sector;
    //是否在范围内
    public bool isIn = true;
    //攻击目标 Transform
    public Transform testAttackTarget;
    //攻击目标点
    public Vector3 testPot = new Vector3(1,0,1);
	//目标(人物或者释放点) Transform
    public Transform target;
    //释放点
    public Vector3 _releasePot = new Vector3(0,0,0);
    public Vector3 releasePot
    {
        set
        {
            _releasePot = value;  
        }
        get
        {
            return _releasePot;
        }
    }
    //释放方向
    public Vector3 direction  = new Vector3(1,0,1);
    //方向夹角 （释放者y轴转向）
    public float directionAngle = 0;
    //释放距离
    public float distance = 4;
    //1.如果是扇形 rangeParameter = 开度夹角
    //2.如果是矩形 rangeParameter = 宽度的1/2
    //3.如果是圆   rangeParameter = 半径
    public float rangeParameter = 90;
    //矩形宽度
    //public float width;
    //画线的材质 不设定系统会用当前材质画线 结果不可控
    public Material mat = null;
    //把弧长分为 36 等分需要 37 点控制 所以数组长度 37 + 1（原点） 
    public Vector3[] vertices = new Vector3[37 + 1];
    //当前范围测试点
    Vector3 curTestPot = new Vector3(1,0,1);
    //释放目标查找标识
    public string targetTag = "Player";
    //是否自动查找目标
    public bool autoFindTarget = true;
	//释放点正前方向量
	Vector3 releaseForward = new Vector3();
    // Use this for initialization
    void Start () {
        //mat = new Material(Shader.Find("Unlit/DrawLineShader"));
        //mat.shader.hideFlags = HideFlags.HideAndDontSave;
        Initvertices();

        float s0 = Mathf.Deg2Rad;  
        float s1 = Mathf.Rad2Deg;  

    }

    /// <summary>
    /// 创建顶点数组
    /// </summary>
    void Initvertices()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(0, 0.1f, 0);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void LookAt(Vector3 p)
    {
        if (target) {
            target.LookAt(p);
        }
    }

    public void SetAttackTarget(Vector3 p)
    {
        //releasePot = GameMain.getInstance().m_SelfPlayer.objInstantiate.transform.position;
        releasePot = p;
        target.transform.position = releasePot;
        LookAt(p);
        directionAngle = target.eulerAngles.y;
    }
    /// <summary>
    /// 是否在扇形范围内
    /// </summary>
    /// <returns></returns>
    public bool isInSector(Vector3 pot) {
        //发起点
        releaseForward = releasePot;
         //默认z+ 为前 z- 为后 左手坐标系
        float z = Mathf.Cos(directionAngle * Mathf.Deg2Rad) * distance;
         //默认x+ 为右 x- 为左 左手坐标系
        float x = Mathf.Sin(directionAngle * Mathf.Deg2Rad) * distance;
        releaseForward.z += z;
        releaseForward.x += x;
        
		Vector3 releasePotToTestPot = pot - releasePot;
		Vector3 releasePotToReleaseForward = releaseForward - releasePot;
   
        //向量a,b的夹角,得到的值为弧度，我们将其转换为角度，便于查看！  
		float angle = Mathf.Acos(Vector3.Dot(releasePotToReleaseForward.normalized, releasePotToTestPot.normalized)) * Mathf.Rad2Deg;
        //当前距离
        float curDistance = Vector3.Distance(releasePot, pot);

        //夹角判断 rangeParameter = rangeAngle
        if (rangeParameter * .5 < Mathf.Abs(angle)){
			//Debug.Log("directionAngle"+directionAngle +",angle = " + angle + ",isInSector = " + isIn);
            return false;
        }
        //距离判断
        if (distance < curDistance ) {
			//Debug.Log("directionAngle"+directionAngle +"curDistance = " + curDistance + ",isInSector = " + isIn);
            return false;
        }
		//Debug.Log("directionAngle"+directionAngle +"angle = " + angle + ",curDistance = " + curDistance + ",isInSector = " + isIn);
        return true;
    }
    /// <summary>
    /// 是否是在矩形中
    /// </summary>
    /// <param name="pot"></param>
    /// <returns></returns>
    public bool isInRect(Vector3 pot)
    {
        float width = rangeParameter;
        //释放点正前方向量
        //手动计算转向
        releaseForward = releasePot;
        //默认z+ 为前 z- 为后 左手坐标系
        float z = Mathf.Cos(directionAngle * Mathf.Deg2Rad) * distance;
        //默认x+ 为右 x- 为左 左手坐标系
        float x = Mathf.Sin(directionAngle * Mathf.Deg2Rad) * distance;
        releaseForward.z += z;
        releaseForward.x += x;

        curTestPot = pot;
        //检测正前方距离
        float curDisZ = Mathf.Abs(pot.z - releasePot.z);
        if (curDisZ > distance)
        {
            //Debug.Log("curDisZ = " + curDisZ + " > distance = " + distance + ",isInRect = " + isIn);
            return false;
        }

        //向量a,b的夹角,得到的值为弧度，我们将其转换为角度，便于查看！  
        //float angle = Mathf.Abs(Mathf.Acos(Vector3.Dot(pot.normalized, releaseForward.normalized)) * Mathf.Rad2Deg );
		//释放点到测试点的向量
		Vector3 releasePotToTestPot = pot - releasePot;
		//释放点正前方向量
		Vector3 releasePotToReleaseForward = releaseForward - releasePot;
		//float c = Vector3.Dot(pot, releaseForward);
		//向量a,b的夹角,得到的值为弧度，我们将其转换为角度，便于查看！  
		//float angle = Mathf.Acos(Vector3.Dot(pot.normalized, releaseForward.normalized)) * Mathf.Rad2Deg;
		float angle = Mathf.Acos(Vector3.Dot(releasePotToReleaseForward.normalized, releasePotToTestPot.normalized)) * Mathf.Rad2Deg;

        //检测夹角
        if (angle > 90f) {
            //Debug.Log("angle = "+ angle + " > 90 ,isInRect = " + isIn);
            return false;
        }
        //检测 当前目标点与释放点绝对距离作为斜边，领边为中轴向量的，对边 a 为 pot 到中轴向量的距离
        float curDistance = Vector3.Distance(releasePot, pot);
        //对边a 大于宽度
        float a =  Mathf.Sin(angle * Mathf.Deg2Rad ) * curDistance;
        if (a > width) {
            //Debug.Log("a = " + a + " > width = " + width + ",isInRect = " + isIn);
            return false;
        }
        //斜边b 大于距离
		float b = Mathf.Cos(angle * Mathf.Deg2Rad) * curDistance;
        if (b > distance)
        {
            //Debug.Log("b = " + b + " > distance = " + distance + ",isInRect = " + isIn);
            return false;
        }

        //Debug.Log("a = " + a + " < width = " + width + ",isInRect = " + isIn);
        return true;
    }
    /// <summary>
    /// 是否在圆形内部
    /// </summary>
    /// <param name="pot"></param>
    /// <returns></returns>
    public bool isInCircle(Vector3 pot)
    {
        float radius = rangeParameter;
        //当前释放点距离于目标点的距离
        float curDistance = Vector3.Distance(releasePot, pot);
        if (curDistance > radius) {
            //Debug.Log("curDistance = " + curDistance + " > radius = " + radius + ",isInRect = " + isIn);
            return false;
        }
        return true;
    }
    /// <summary>
    /// 更新矩阵
    /// </summary>
    void UpdateRectVertices()
    {
        if (target != null)
        {
            //根据目标对象角度设置朝向
            directionAngle = target.eulerAngles.y;
            //根据目标对象坐标设置释放点
            releasePot = target.position;
        }
        float width = rangeParameter;
        //左
        float deg = 180f - directionAngle;
        float x = Mathf.Cos(deg * Mathf.Deg2Rad) * width;
        float z = Mathf.Sin(deg * Mathf.Deg2Rad) * width;
        vertices[0].x = x;
        vertices[0].z = z;
        //左前
        //基础角   
        deg =  Mathf.Atan2(width , distance);
        float c = width / Mathf.Sin(deg);  

        deg = Mathf.Atan2(width , distance) + (90f - directionAngle) * Mathf.Deg2Rad;
        x = Mathf.Cos(deg) * c;
        z = Mathf.Sin(deg) * c;
        vertices[1].x = x;
        vertices[1].z = z;
        //右前
        deg = (90f - directionAngle) * Mathf.Deg2Rad - Mathf.Atan(width / distance);
        x = Mathf.Cos(deg) * c;
        z = Mathf.Sin(deg) * c;
        vertices[2].x = x;
        vertices[2].z = z;
        //右
        deg = 0f - directionAngle;
        x = Mathf.Cos(deg * Mathf.Deg2Rad) * width;
        z = Mathf.Sin(deg * Mathf.Deg2Rad) * width;
        vertices[3].x = x;
        vertices[3].z = z;
    }
    /// <summary>
    /// 更新顶点数组
    /// </summary>
    void Updatevertices(){
        if (target != null) {
            //根据目标对象角度设置朝向
            directionAngle = target.eulerAngles.y ;
            //根据目标对象坐标设置释放点
            releasePot = target.position;
        }
        //1.把弧长分为 18 等分需要 19 点控制 所以数组长度 19 + 1（原点） 
        //2.angleSegment 角度的等分
        //3.rangeParameter 扇形开度夹角
        float angleSegment = rangeParameter / (vertices.Length - 2);
        //1.计算基础偏移角度，使正方向左右开合度相等 
        //2.-directionAngle 根据当前目标y轴转向修正方向角
        float baseAngle = (90f - rangeParameter * .5f) - directionAngle;
        //绘制圆点
        Vector3 p0 = new Vector3(0, 0, 0);
        vertices[0] = p0;
        //用距离设置半径
        float R = distance;

        for (int i = 1; i < vertices.Length; i++)
        {
            //float deg = (i - 1) * 10f; 
            float deg = baseAngle + (i - 1) * angleSegment ;
            //下标每次 angleSegment 角度的等分,角度递增
            float x = Mathf.Cos(deg * Mathf.Deg2Rad) * R;
            float y = Mathf.Sin(deg * Mathf.Deg2Rad) * R;

            vertices[i].x = x;
            vertices[i].z = y;
        }
    }
    /// <summary>
    /// 更新顶点数组
    /// </summary>
    void UpdateCircleVertices()
    {
        if (target != null)
        {
            //根据目标对象角度设置朝向
            directionAngle = target.eulerAngles.y;
            //根据目标对象坐标设置释放点
            releasePot = target.position;
        }
        //1.把弧长分为 18 等分需要 19 点控制 所以数组长度 19 + 1（原点） 
        //2.angleSegment 角度的等分
        float angleSegment = 360 / (vertices.Length - 2);
        //绘制圆点
        Vector3 p0 = new Vector3(0, 0, 0);
        vertices[0] = p0;
        //用距离设置半径
        float R = rangeParameter;

        for (int i = 1; i < vertices.Length; i++)
        {
            //float deg = (i - 1) * 10f; 
            //下标每次 angleSegment 角度的等分,角度递增
            float deg = (i - 1) * angleSegment;
            float x = Mathf.Cos(deg * Mathf.Deg2Rad) * R;
            float y = Mathf.Sin(deg * Mathf.Deg2Rad) * R;

            vertices[i].x = x;
            vertices[i].z = y;
        }
    }
    /// <summary>
    /// 绘制顶点
    /// </summary>
    /// <param name="length"></param>
    void DrawVertices(int length)
    {
        GL.Begin(GL.LINES);
        for (int i = 0; i < length - 1; ++i)
        {
            GL.Vertex(vertices[i] + releasePot);
            GL.Vertex(vertices[i + 1] + releasePot);
        }
        //终点连接起始点
        GL.Vertex(vertices[length - 1] + releasePot);
        GL.Vertex(vertices[0] + releasePot);
        GL.End();
    }
    /// <summary>
    /// 绘制扇形方法
    /// </summary>
    void DrawVertices() {
        DrawVertices(vertices.Length);
    }
    /// <summary>
    /// 绘制距离方向方法
    /// </summary>
    void DrawDirection()
    {
        //绘制释放方向 和距离
        GL.Begin(GL.LINES);
        GL.Vertex(releasePot);
        //GL.Vertex(curTestPot);
		GL.Vertex (releaseForward);
        GL.End();
    }
    // Update is called once per frame
    void Update()
    {
        if (!visible) return;

        if (autoFindTarget && target == null)
        {
            GameObject targetGo = GameObject.FindGameObjectWithTag(targetTag);
            if (targetGo)
                target = targetGo.transform;   
        }

        switch (skillAttackRangeType) {
            case SkillAttackRangeType.Sector:

                Updatevertices();

                if (testAttackTarget)
                {
                    isIn = isInSector(testAttackTarget.position);
                }else
                {
                    isIn = isInSector(testPot);
                }
                break;
            case SkillAttackRangeType.Rect:

                UpdateRectVertices();

                if (testAttackTarget)
                {
                    isIn = isInRect(testAttackTarget.position);
                }else
                {
                    isIn = isInRect(testPot);
                }
               
                break;
            case SkillAttackRangeType.Circle:

                UpdateCircleVertices();

                if (testAttackTarget)
                {
                    isIn = isInCircle(testAttackTarget.position);
                }else
                {
                    isIn = isInCircle(testPot);
                }

                break;
        }
    }
    /// <summary>
    /// 必须添加在摄像机上才会调用 OnPostRender() 方法
    /// </summary>
    void OnPostRender()
    {
        if (!visible) return;

        GL.Color(Color.red);
        GL.PushMatrix();

        if (mat) mat.SetPass(0);

        switch (skillAttackRangeType)
        {
            case SkillAttackRangeType.Sector:
                DrawVertices();
                break;
            case SkillAttackRangeType.Rect:
                DrawVertices(4);
                break;
            case SkillAttackRangeType.Circle:
                DrawVertices();
                break;
        }
        DrawDirection();

        GL.PopMatrix();
    }

}
