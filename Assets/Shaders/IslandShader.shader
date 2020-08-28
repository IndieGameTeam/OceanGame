Shader "Toon/IslandShader"
{
    Properties
    {
        _VerticalOffset("Vertical offset", Float) = 0
        _Transition("Transition", Range(0.1, 100)) = 1
        _TopColor("TopColor", Color) = (0, 1, 0, 1)
        _BottomColor("BottomColor", Color) = (1, 1, 0, 1)
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "LightMode" = "ForwardBase"
        }

        Pass
        {
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
                float4 vertex : POSITION;
                float4 localPosition : POSITION1;
            };

            float _VerticalOffset;
            float _Transition;
            float4 _TopColor;
            float4 _BottomColor;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul((float3x3)UNITY_MATRIX_M, v.normal));
                o.localPosition = v.vertex;

                return o;
            }

            float transition(float v)
            {
                return 1 / (1 + exp(_VerticalOffset - v * _Transition));
            }

            float4 frag(v2f i) : SV_Target
            {
                float NdotL = dot(i.worldNormal, _WorldSpaceLightPos0);
                float light = saturate(floor(NdotL * 1.5) * 0.5 + 0.75);

                return lerp(_BottomColor, _TopColor, transition(i.localPosition.y)) * light;
            }

            ENDCG
        }
    }
}
