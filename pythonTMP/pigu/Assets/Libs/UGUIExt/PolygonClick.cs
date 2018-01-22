using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolygonClick : Image
{

    private RectTransform m_RectTransform = null;
    private Vector2[] m_Vertexs = null;
    protected override void Start()
    {
        base.Start();
        this.m_RectTransform = base.GetComponent<RectTransform>();
        var c = base.GetComponent<PolygonCollider2D>();
        if (c != null)
        {
            this.m_Vertexs = c.points;
            //c.enabled = false;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        this.m_RectTransform = null;
        this.m_Vertexs = null;
    }
    /// <summary>
    /// 重写方法，用于干涉点击射线有效性
    /// </summary>
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (this.m_Vertexs == null)
        {
            return base.IsRaycastLocationValid(screenPoint, eventCamera);
        }
        else
        {
            // 点击的坐标转换为相对于图片的坐标
            //
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform,
            screenPoint,
            eventCamera,
            out pos);

            // 判断点击是否在区域内
            //
            return _Contains(this.m_Vertexs, pos);
        }
    }

    /// <summary>
    /// 使用Crossing Number算法获取指定的点是否处于指定的多边形内
    /// </summary>
    private static bool _Contains(Vector2[] pVertexs, Vector2 pPoint)
    {
        var crossNumber = 0;

        for (int i = 0, count = pVertexs.Length; i < count; i++)
        {
            var vec1 = pVertexs[i];
            var vec2 = i == count - 1 // 如果当前已到最后一个顶点，则下一个顶点用第一个顶点的数据
                ? pVertexs[0]
                : pVertexs[i + 1];

            if (((vec1.y <= pPoint.y) && (vec2.y > pPoint.y))
                || ((vec1.y > pPoint.y) && (vec2.y <= pPoint.y)))
            {
                if (pPoint.x < vec1.x + (pPoint.y - vec1.y) / (vec2.y - vec1.y) * (vec2.x - vec1.x))
                {
                    crossNumber += 1;
                }
            }
        }

        return (crossNumber & 1) == 1;
    }
}
