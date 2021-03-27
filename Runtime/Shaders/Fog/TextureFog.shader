Shader "PowderBox/TextureFog"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _FogAmount("Fog amount", float) = 1
        _FogMin("Fog min distance", Range(1,100)) = 1
        _FogMax("Fog max distance", Range(1,100)) = 1
        _ColorRamp("Color ramp", 2D) = "white" {}
        _FogIntensity("Fog intensity", float) = 1
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
            float _FogAmount;
            float _FogIntensity;
            float _FogMax;
            float _FogMin;
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // another way to do it
                fixed depth = tex2D(_CameraDepthTexture, i.uv).r;
                depth = Linear01Depth(depth);
                // depth = depth * _ProjectionParams.z;
                float depthValue = depth;
                // float depthValueMul = (depthValue) * _FogAmount;
                float depthValueMul = saturate(pow(depthValue,_FogAmount/100));
                float depthSmooth = smoothstep(_FogMin/100, _FogMax/100, (depthValueMul));
                // depthSmooth = pow(depthValue, _FogAmount);
                fixed4 fogCol = tex2D(_ColorRamp, (float2(depthSmooth, 0)));
                // return lerp(col, fogCol, depthValueMul);
                // return lerp(col, fogCol, fogCol.a);
                // return float4(depthSmooth,depthSmooth,depthSmooth,1);
                // return float4(depthValueMul,depthValueMul,depthValueMul,1);
                return (1- depthSmooth) * col + depthSmooth * fogCol;
            }
            ENDCG
        }
    }
}