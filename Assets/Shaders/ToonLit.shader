Shader "Toon/Lit"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CullMode)] 
        _CullMode("Culling", Float) = 2
		_Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque"
			"LightMode" = "ForwardBase"
		}

        Pass
        {
            Cull[_CullMode]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
            };

            struct v2f
            {
				float3 worldNormal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul((float3x3)UNITY_MATRIX_M, v.normal));

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
				float NdotL = dot(i.worldNormal, _WorldSpaceLightPos0);
				float light = saturate(floor(NdotL * 1.5) * 0.5 + 0.5) * _LightColor0;

                return _Color * light;
            }
            ENDCG
        }
    }
}
