Shader "Toon/Unlit/ToonWater"
{
    Properties
    {
        _Smoothness("Smoothness", Float) = 4
        _Speed("Foam speed", Vector) = (0.1, 0.1, 0, 0)
        _Color ("Color", Color) = (0, 0.6, 0.8, 0.7)
        _FoamNoise ("Foam noise", 2D) = "white" {}
        _FoamDistortion("Foam distortion", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 foamUV : TEXCOORD0;
                float2 distortionUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            float _Smoothness;
            float4 _Color;
            float4 _Speed;

            sampler2D _FoamDistortion;
            float4 _FoamDistortion_ST;

            sampler2D _FoamNoise;
            float4 _FoamNoise_ST;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.foamUV = TRANSFORM_TEX(v.uv, _FoamNoise);
                o.distortionUV = TRANSFORM_TEX(v.uv, _FoamDistortion);

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_FoamNoise, i.foamUV);
                float4 col0 = tex2D(_FoamDistortion, i.distortionUV + _Time * _Speed);

                col = pow(saturate(col + col0 - 0.5), _Smoothness);
                col = _Color + (1 - _Color) * col;
                col.a = _Color.a + (1 - _Color.a) * col.r;

                return col;
            }
            ENDCG
        }
    }
}
