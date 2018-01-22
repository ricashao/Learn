using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillReleaseRangeAbEffect : SkillReleaseRange
{
	public string assetBundlePath  =  "ground 13.effect";
    public AssetBundle assetBundle;
	public string assetName = "ground 13";

    GameObject effectGameObject;
    // Use this for initialization
    override public void Start () {

		WWW download = new WWW( AppContentPath() + assetBundlePath );

		while (!download.isDone) { }
		assetBundle = download.assetBundle;
		if (assetBundle != null)
		{
			GameObject prefabGameObject = assetBundle.LoadAsset <GameObject>( assetName );
			effectGameObject = Instantiate<GameObject>(prefabGameObject);
		}
        //GameObject prefabGameObject =  assetBundle.LoadAsset<GameObject>(assetName);
        //effectGameObject = Instantiate<GameObject>(prefabGameObject);
    }

    override public void UpdateCircleVertices()
    {
        if (target != null)
        {
            //根据目标对象坐标设置释放点
            releasePot = target.position;
        }

        effectGameObject.transform.position = releasePot;

    }

    // Update is called once per frame
    override public void Update()
    {
        if (autoFindTarget && target == null)
        {
            GameObject targetGo = GameObject.FindGameObjectWithTag( targetTag);
            if (targetGo)
                target = targetGo.transform;
        }
        if (skillAttackRange == null)
        {
            skillAttackRange = GetComponent<SkillAttackRange>();
        }
        else
        {
            distance = skillAttackRange.distance;
        }

        if (gameObject != null){
            UpdateCircleVertices();
        }
    }

  
}
