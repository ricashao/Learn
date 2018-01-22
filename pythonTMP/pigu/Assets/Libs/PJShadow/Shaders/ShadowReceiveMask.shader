Shader "Custom/ShadowReceiveMask"
{
	Properties 
	  {  
	    _Color ("Main Color", Color) = (1,1,1,1)  
	    _ShadowTex ("Cookie", 2D) = "" {}  
	    _MaskTex ("Mask", 2D) = "" {}  
	   }  
     
	 Subshader   
	 {  
	   Tags {"Queue"="Transparent" }  
	   Pass   
	   {  
	        ZWrite Off  		//关闭深度缓冲
	    	Fog { Mode Off }    //关闭雾
	        ColorMask RGB       //屏蔽 a 通道
	        Blend DstColor Zero //混合
	        //Blend SrcAlpha OneMinusSrcAlpha  
	        //Blend One One  
	        //Blend One OneMinusSrcAlpha
	        Offset -1, -1  		//抖动z 避免 z fighting
	   
	        CGPROGRAM  
	        #pragma vertex vert  
	        #pragma fragment frag  
	        #include "UnityCG.cginc"  
	           
	       struct v2f   
	       {  
	            float4 uvShadow : TEXCOORD0;  
	            float4 pos : SV_POSITION;  
	       };  
	            
	       float4x4 unity_Projector;  

	       v2f vert (float4 vertex : POSITION)  
	       {  
	          v2f o;  
	          o.pos = UnityObjectToClipPos (vertex);  
	          o.uvShadow = mul (unity_Projector, vertex);  
	          return o;  
	       }  
	          
	       sampler2D _ShadowTex;  
	       sampler2D _MaskTex;  

	       fixed4 frag (v2f i) : SV_Target  
	       {  
	          fixed4 texM = tex2Dproj(_MaskTex, UNITY_PROJ_COORD(i.uvShadow));  
	          fixed4 texS = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));  

	          //抓取图片的灰度判断，是否有颜色
	          if((texS.r != texS.g) || (texS.b != texS.g) ||(texS.r != texS.b)) {
	          	texS.rgb = 0.6;
	          	//texS.a = 0.1;
	          }

              return texS;  
           }  
	       ENDCG  
	      }  
	 }  
}
