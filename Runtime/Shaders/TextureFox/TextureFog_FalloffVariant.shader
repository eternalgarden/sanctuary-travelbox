Shader "PowderBox/TextureFog_FalloffVariant"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _FogPower("Fog power", Range(0,1)) = 1
        [KeywordEnum(InCubic, InOutCubic, InSin, InCirc, OutCirc)] _Falloff ("Falloff", Float) = 0
        _FogMin("Fog min distance", Range(0,1)) = 1
        _FogMax("Fog max distance", Range(0,1)) = 1
        _BaseFog("Minimum Fog Strength", Range(0,1)) = 1
        _ColorRamp("Color ramp", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _FALLOFF_INCUBIC _FALLOFF_INOUTCUBIC _FALLOFF_INSIN _FALLOFF_INCIRC _FALLOFF_OUTCIRC
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            sampler2D _ColorRamp;
            float _FogPower, _BaseFog;
            float _FogMax;
            float _FogMin;

            float easeInSine(float x)
            {
                return 1 - cos((x * 3.14159265395) / 2);
            }

            float easeInCirc(float x)
            {
                return 1 - sqrt(1 - pow(x, 2));
            }

            float easeOutCirc(float x)
            {
                return sqrt(1 - pow(x - 1, 2));
            }

            float easeInCubic(float x)
            {
                return x * x * x;
            }

            float easeInOutCubic(float x)
            {
                return x < 0.5 ? 4 * x * x * x : 1 - pow(-2 * x + 2, 3) / 2;
            }

            float easeDepth(float depth)
            {
                float easedDepth = depth;

                #if _FALLOFF_INCUBIC
                easedDepth = easeInCubic(easedDepth);
                #elif _FALLOFF_INOUTCUBIC
                easedDepth = easeInOutCubic(easedDepth);
                #elif _FALLOFF_INSIN
                easedDepth = easeInSine(easedDepth);
                #elif _FALLOFF_INCIRC
                easedDepth = easeInCirc(easedDepth);   
                #elif _FALLOFF_OUTCIRC
                easedDepth = easeOutCirc(easedDepth);
                #endif

                return easedDepth;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float depth = tex2D(_CameraDepthTexture, i.uv).r;
                depth = Linear01Depth(depth);
                depth = saturate(depth + _BaseFog);
                depth = pow(depth, (1 - _FogPower));
                // ? Find a way to "squish" a function
                // Everything below _FogMin will be set to 0 - no fog
                // Everything above FogMax will be set as maximum fog
                depth = smoothstep(_FogMin, _FogMax, depth);
                depth = easeDepth(depth);
                
                fixed4 fogCol = tex2D(_ColorRamp, (float2(depth, 0)));
                return (1- depth) * col + depth * fogCol;
            }
            ENDCG
        }
    }
}