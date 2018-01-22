// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Lab/Boss" {
	Properties{
		_FakeLight("FakeLight Intensity", float) = 1.0
		_MainTex("Base (RGB)", 2D) = "white"{}
		_MaskTex("Mask (AO, Spec, Illum)", 2D) = "white"{}
		//_SSS("SSS Tex", 2D) = "black"{}

		_Outline("Outline width (勾边宽度 0.02)", float) = 0.02
		_OutColor("Outline Color (勾边颜色)", Color) = (1,1,1,1)

		_ShadeThreshold("Shade Threshold (阴影阈值 0~1)", float) = 0.5
		_DarkValue("Dark Intensity    (阴影亮度 0~1)",float) = 0.8

		_SpecPower("Specular Power (高光级别 1~30)", Range(1, 30)) = 2
		_SpecIntensity("Specular Intensity (高光强度)", Float) = 1

		_RimEnable("Rim Enable",Range(0,1)) = 0.0
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Intensity",float) = 0.2
//begin 编辑器专用
		[Enum(NoDebug,0,AO,1,Specular,2,Illum,3)]
		_DebugVisualization("Debug Visualization", int) = 0
//end
	}
	SubShader{
		Tags{ "Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque" }

		Pass{
			Tags{ "LightMode" = "ForwardBase" }
			Cull OFF

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _LightColor0;
			sampler2D _MainTex;
			sampler2D _MaskTex;
			//sampler2D _SSS;
			half _FakeLight;
			
			half _ShadeThreshold;
			half _DarkValue;
			half _SpecPower;
			half _SpecIntensity;

			half _RimEnable;
			half _RimPower;
			fixed4 _RimColor;

// 因为指令数比较紧俏，所以在编辑器下开启3.0，这样预览用的代码加进来也不会超出指令数限制。
// 如果想要验证在2.0下是否有问题，就开启下面这个编译参数。这样编辑器预览代码也不会生效，3.0也不会生效。
//#define DISABLE_EDITOR_DEBUG 1

#if !SHADER_API_MOBILE
#if !DISABLE_EDITOR_DEBUG
			#pragma target 3.0
#endif
			bool _DebugVisualization;
#endif

			struct v2f {
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
				float3 lightDir:TEXCOORD1;
				float3 viewDir:TEXCOORD2;			
				//float4 vertColor:TEXCOORD3;
				float3 reflectDir:TEXCOORD3;
				float3 normal:TEXCOORD4;
			};

			float3 rgb2hsv(float3 c)
			{
				float4 k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
					float4 p = lerp(float4(c.bg, k.wz), float4(c.gb, k.xy), step(c.b, c.g));
					float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

					float d = q.x - min(q.w, q.y);
				float e = 1.0e-10;
				return float3(abs(q.z + (q.w - q.y) / (6.0 *d + e)), d / (q.x + e), q.x);
			}

			float3 hsv2rgb(float3 c)
			{
				float4 k = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
					float3 p = abs(frac(c.xxx + k.xyz)*6.0 - k.www);
					return c.z * lerp(k.xxx, clamp(p - k.xxx, 0.0, 1.0), c.y);
			}

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.lightDir = ObjSpaceLightDir(v.vertex);
				o.viewDir = ObjSpaceViewDir(v.vertex);
				//o.vertColor = v.color;
				o.reflectDir = reflect(-o.lightDir, o.normal);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) :COLOR
			{
//					float4 col = 1;
				float3 N = normalize(i.normal);
				float3 viewDir = normalize(i.viewDir);
				float difMag = dot(N, i.lightDir);
				float halfMag = difMag *0.5 + 0.5;
				//diffuse
				half4 main = tex2D(_MainTex, i.uv);
				half4 mask = tex2D(_MaskTex, i.uv);
				half mask_ao = mask.r;
				half mask_spec = mask.g;
				half mask_illum = mask.b;
				//float4 sss = tex2D(_SSS, i.uv);

				half4 col = main;
				col.rgb = main.rgb * _LightColor0.rgb * UNITY_LIGHTMODEL_AMBIENT.xyz;
				
				if (halfMag * mask_ao > _ShadeThreshold)
				{
					col.rgb = col.rgb + main.rgb * mask_illum * _FakeLight;
				}
				else
				{
					//col.rgb = _DarkValue * sss.rgb * _LightColor0.rgb * UNITY_LIGHTMODEL_AMBIENT.xyz;
					col.rgb = col.rgb * _DarkValue + saturate(mask_illum - 0.588) * 2.427 * main.rgb;
				}

				// specular:
				float nh = saturate(dot(viewDir, i.reflectDir));
				float spec = pow(nh, _SpecPower) * mask_spec * _SpecIntensity;
				col.rgb += _LightColor0.rgb * spec;

				// rim
				float rim = saturate(1 - dot(viewDir, N));
				fixed3 rimLight = col.rgb + _RimPower * rim * _RimColor.rgb;
				col.rgb = lerp(col.rgb, rimLight, _RimEnable);

#if !SHADER_API_MOBILE && !DISABLE_EDITOR_DEBUG
				switch(_DebugVisualization)
				{
					case 1:
						col = fixed4(mask_ao, mask_ao, mask_ao, 1);
						break;
					case 2:
						col = fixed4(mask_spec, mask_spec, mask_spec, 1);
						break;
					case 3:
						col = fixed4(mask_illum, mask_illum, mask_illum, 1);
						break;
				}
#endif
				return col;
			}
			ENDCG
		}//end pass

		Pass{
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			float _Outline;
			float4 _OutColor;

			struct v2f {
				float4 pos:SV_POSITION;
				float4 uv :TEXCOORD0;
				//float4 vertColor :TEXCOORD1;
			};

			v2f vert(appdata_full v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy);
				o.pos.xy += offset * o.pos.z * _Outline * v.color.r;

				//o.vertColor = v.color;
				return o;
			}

			fixed4 frag(v2f i) :COLOR
			{
				//return i.vertColor;
				return _OutColor;
			}

			ENDCG
		}//end of pass

	}

	Fallback "Transparent/Cutout/VertexLit"
}
