Shader "Custom/DualColorTransparent"
{
    Properties
    {
        _OutsideColor("Outside Color", Color) = (1, 0, 0, 0.5)
        _InsideColor("Inside Color", Color) = (0, 1, 0, 0.5)
        _Transparency("Transparency", Range(0, 1)) = 0.5
        _RimPower("Rim Power", Range(0.1, 8)) = 2
        _RimIntensity("Rim Intensity", Range(0, 2)) = 1
        _FresnelPower("Fresnel Power", Range(0.1, 8)) = 2
    }

        SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        // 뒷면 렌더링 (안쪽 색상)
        Pass
        {
            Name "BACK"
            Tags { "LightMode" = "ForwardBase" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float2 uv : TEXCOORD3;
            };

            fixed4 _InsideColor;
            half _Transparency;
            half _RimPower;
            half _RimIntensity;
            half _FresnelPower;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                // 프레넬 효과 (가장자리 밝기)
                float fresnel = pow(1.0 - saturate(dot(normal, viewDir)), _FresnelPower);

                // 림 라이팅 효과
                float rim = pow(fresnel, _RimPower) * _RimIntensity;

                // 안쪽 색상 계산
                fixed4 finalColor = _InsideColor;
                finalColor.rgb += rim;
                finalColor.a *= _Transparency;

                return finalColor;
            }
            ENDCG
        }

        // 앞면 렌더링 (바깥쪽 색상)
        Pass
        {
            Name "FRONT"
            Tags { "LightMode" = "ForwardBase" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float2 uv : TEXCOORD3;
            };

            fixed4 _OutsideColor;
            half _Transparency;
            half _RimPower;
            half _RimIntensity;
            half _FresnelPower;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                // 프레넬 효과 (가장자리 밝기)
                float fresnel = pow(1.0 - saturate(dot(normal, viewDir)), _FresnelPower);

                // 림 라이팅 효과
                float rim = pow(fresnel, _RimPower) * _RimIntensity;

                // 바깥쪽 색상 계산
                fixed4 finalColor = _OutsideColor;
                finalColor.rgb += rim;
                finalColor.a *= _Transparency;

                return finalColor;
            }
            ENDCG
        }
    }

        FallBack "Transparent/Diffuse"
}