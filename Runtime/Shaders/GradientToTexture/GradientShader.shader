Shader "PowderBox/GradientShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma target 4.0
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "GrimoireLib.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _SomeColor;

            int _GradinentArraySize;
            float _GradientTimeArray[10];
            float4 _GradientColorArray[10];

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float downTime;
                float upTime;
                float4 downColor;
                float4 upColor;

                float2 uv = i.uv;
                
                for(int i = 0; i < _GradinentArraySize; i++)
                {
                    if(uv.x >= _GradientTimeArray[i])
                    {
                        downTime = _GradientTimeArray[i];
                        upTime = _GradientTimeArray[i+1];
                        downColor = _GradientColorArray[i];
                        upColor = _GradientColorArray[i+1];
                    }
                }

                _SomeColor = lerp(downColor, upColor, invLerp(downTime, upTime, uv.x));

                return _SomeColor;
            }
            ENDCG
        }
    }
}
