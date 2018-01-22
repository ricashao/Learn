// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ProjectorShadowShader"   
	{   
	  Properties   
	  {  
	    _Color ("Main Color", Color) = (1,1,1,1)  
	    _ShadowTex ("Cookie", 2D) = "" {}  
	   }  
     
	 Subshader   
	 {  
	   Tags {"Queue"="Transparent" }  
	   Pass   
	   {  
	        ZWrite Off  
	    	Fog { Mode Off }  
	        ColorMask RGB  
	        Blend DstColor Zero  
	        //Blend SrcAlpha OneMinusSrcAlpha  
	        //Blend One One  
	        //Blend One OneMinusSrcAlpha
	        Offset -1, -1  
	   
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
	       //sampler2D _MaskTex;  
	       fixed4 frag (v2f i) : SV_Target  
	       {  
	          //fixed4 texM = tex2Dproj(_MaskTex, UNITY_PROJ_COORD(i.uvShadow));  
	          fixed4 texS = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));  

	          if((texS.r != texS.g) || (texS.b != texS.g) ||(texS.r != texS.b)) {
	          	texS.rgb = 0.6;
	          	//texS.a = 0.1;
	          }
	          //else{
	           //texS.rgb = 0; 
	          //}
	          //texS.rgb += texM.rgb;  
	          //float4 result;
	          //if(texS.r > 0)
	          //result.rgb = 0.6;;
			  return texS;  
            
            //return result;
           }  
	       ENDCG  
	      }  
	 }  
}  