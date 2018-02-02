// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Texture Splatting" {

	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		_FlowTex("流光图 (A)", 2D) = "white" {}
	_ScrollXSpeed("横向速度", Range(0, 10)) = 2
		_ScrollYSpeed("竖向速度", Range(0, 10)) = 0
		_ScrollDirection("方向", Range(-1, 1)) = -1
		_FlowColor("流光颜色",Color) = (1,1,1,1)

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Stencil
	{
		Ref[_Stencil]
		Comp[_StencilComp]
		Pass[_StencilOp]
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
	{
		Name "Default"
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0

#include "UnityCG.cginc"
#include "UnityUI.cginc"

#pragma multi_compile __ UNITY_UI_ALPHACLIP

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;

		UNITY_VERTEX_INPUT_INSTANCE_ID

	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
		float4 worldPosition : TEXCOORD2;

		UNITY_VERTEX_OUTPUT_STEREO
	};

	fixed4 _Color;
	fixed4 _TextureSampleAdd;
	float4 _ClipRect;


	sampler2D _FlowTex;
	fixed _ScrollXSpeed;
	fixed _ScrollYSpeed;
	fixed _ScrollDirection;
	float4 _FlowColor;


	v2f vert(appdata_t IN)
	{
		v2f OUT;
		UNITY_SETUP_INSTANCE_ID(IN);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
		OUT.worldPosition = IN.vertex;
		OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
		OUT.texcoord = IN.texcoord;
		//OUT.texcoord1 = TRANSFORM_TEX(IN.texcoord1, _FlowTex);
		OUT.texcoord1 = IN.texcoord;


		OUT.color = IN.color * _Color;
		return OUT;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f IN) : SV_Target
	{
		//改变流光图的uv
		fixed2 scrolledUV = IN.texcoord1;
	fixed xScrollValue = _ScrollXSpeed;// *_Time.y;// _Time.y等同于Time.timeSinceLevelLoad
	fixed yScrollValue = _ScrollYSpeed;// *_Time.y;
	scrolledUV += fixed2(xScrollValue, yScrollValue) * _ScrollDirection;


	half4  color = (tex2D(_MainTex, IN.texcoord));

	//color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

	//颜色混合
	half4 c = tex2D(_FlowTex, scrolledUV);
	//half4 d = tex2D(_MainTex, IN.uv_MainTex);
	//color.rgb = c.rgb * _FlowColor.rgb + color.rgb;
	//color.a = d.a;
	color.a = c.a;

#ifdef UNITY_UI_ALPHACLIP
	clip(color.a - 0.001);
#endif

	return color;
	}
		ENDCG
	}
	}
}