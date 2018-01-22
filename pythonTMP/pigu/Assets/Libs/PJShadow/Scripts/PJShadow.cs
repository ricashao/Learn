using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PJShadow : MonoBehaviour {
	
	public bool isShowInEditMode = false;
    /// <summary>
    /// 绘制层 人物layer
    /// </summary>
    public LayerMask layerMask;

    public string playerTag = "Player";
    public Transform playerTransform;

    public Vector3 toPlayerTransform = new Vector3(-1.2f, 13.09f, -15.41f);

    public float shadowSize = 1;
    public int blurTimes = 3;
    public float blurOffset;
    public Material blurMat;
    public bool useBlur;

    static bool isRendering;

    Camera curCam;

    RenderTexture shadowTex;
    Projector projector;
    Camera pjCam;

    public bool isDrawGUI = false;

	void Awake(){
		
	}

    void Start(){
        projector = gameObject.GetComponent<Projector>();
    }

    void OnEnable()
    {
        RenderObjects();
    }

    void OnDisable()
    {
        Clear();
    }

    void OnGUI()
    {
        if(isDrawGUI)
            GUI.DrawTexture(new Rect(0, 0, Screen.width * 0.8f, Screen.height * 0.8f), shadowTex);
    }

    void Clear()
    {
        if (shadowTex)
        {
            DestroyImmediate(shadowTex);
            shadowTex = null;
        }
    }

    Camera CreateLightSpaceCam(Projector projector)
    {
        if (projector == null) return null;

        if (!shadowTex)
        {
            shadowTex = new RenderTexture((int)(Screen.width * shadowSize), (int)(Screen.height * shadowSize), 0);
            shadowTex.hideFlags = HideFlags.DontSave;
        }

        if (curCam == null)
        {
            //GameObject go = new GameObject("ProjectorCam", typeof(Camera), typeof(BlurDemo));
			GameObject go = new GameObject("ProjectorCam", typeof(Camera));
            //go.hideFlags = HideFlags.HideAndDontSave;
            go.hideFlags = HideFlags.DontSave;
            curCam = go.GetComponent<Camera>();
            curCam.enabled = false;
            curCam.transform.localPosition = Vector3.zero;
            curCam.transform.localRotation = Quaternion.Euler(Vector3.zero);
            curCam.gameObject.AddComponent<FlareLayer>();
            curCam.clearFlags = CameraClearFlags.SolidColor;
            curCam.backgroundColor = new Color(1, 1, 1, 0);
            curCam.farClipPlane = projector.farClipPlane;
            curCam.nearClipPlane = projector.nearClipPlane;
            curCam.orthographic = projector.orthographic;
            curCam.fieldOfView = projector.fieldOfView;
            curCam.aspect = projector.aspectRatio;
            curCam.orthographicSize = projector.orthographicSize;
			curCam.allowHDR = false;
			curCam.allowMSAA = false;
            curCam.depthTextureMode = DepthTextureMode.None;
			curCam.renderingPath = RenderingPath.VertexLit;
            curCam.cullingMask = layerMask;
            curCam.transform.position = transform.position;
            curCam.transform.rotation = transform.rotation;
        }
		if (projector.material.HasProperty("_ShadowTex")) projector.material.SetTexture("_ShadowTex", shadowTex);
        
        return curCam;
    }

    void UseBlur(BlurDemo blur)
    {
        blur.useBlur = useBlur;
        blur.blurMat = blurMat;
        blur.times = blurTimes;
        blur.offset = blurOffset/128.0f;
    }

    void Update()
    {
        if ( playerTransform != null )
        {
            RenderObjects();
        }
        Follow();
    }

    void RenderObjects()
    {
        if(Screen.width == 0 || Screen.height == 0)
            return;

        if (pjCam == null) {
            pjCam = CreateLightSpaceCam(projector);
        }
        if (pjCam == null || projector == null) {
            return;
        }

        pjCam.transform.position = transform.position;
        pjCam.transform.rotation = transform.rotation;
        
        //UseBlur(pjCam.GetComponent<BlurDemo>());
        pjCam.Render();
        pjCam.targetTexture = shadowTex;
    }

    void Follow()
    {
        if (playerTransform == null)
        {
            GameObject go = GameObject.FindWithTag(playerTag);
            if (go)
            {
                playerTransform = go.transform;
            }
        }
        else
        {
            transform.position = playerTransform.position + toPlayerTransform;
            transform.LookAt(playerTransform.position);
        }
    }

}
