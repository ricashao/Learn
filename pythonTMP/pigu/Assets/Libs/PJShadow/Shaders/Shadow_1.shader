﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Shadow_1" {
	Properties {
		_ShadowTex ("Shadow", 2D) = "gray" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" }
		Pass {
			ZWrite Off
			Fog { Color (1, 1, 1) }
			AlphaTest Greater 0
			ColorMask RGB
			Blend DstColor Zero
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos:POSITION;
				float4 sproj:TEXCOORD0;
			};

			float4x4 viewMatrix;
			float4x4 projMatrix;

			sampler2D _ShadowTex;

			v2f vert(float4 vertex:POSITION){
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				float4 pos = mul(unity_ObjectToWorld, vertex);
				pos = mul(viewMatrix, pos);
				pos = mul(projMatrix, pos);
				o.sproj = ComputeScreenPos(pos);
				return o;
			}

			float4 frag(v2f i):COLOR{
				float4 c = tex2Dproj(_ShadowTex, i.sproj);
				return c;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
