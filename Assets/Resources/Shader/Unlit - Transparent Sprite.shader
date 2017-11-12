Shader "Unlit/Transparent Sprite"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite On
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				// 给轮廓的边缘赋值好ID，就可以算出每个具体像素的ID
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{
			    // tex2D 就是拿id去查询数据表的函数
				fixed4 color = tex2D(_MainTex, IN.uv) * IN.color;
				return color;
			}
			ENDCG
		}
	}

	//备胎设为Unity自带的普通漫反射  
    Fallback" Diffuse "  
}
