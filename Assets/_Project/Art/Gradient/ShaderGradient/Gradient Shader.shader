Shader "UI/SmoothLinearGradient"
{
    Properties 
    {
        [Header(Gradient Settings)]
        _TopColor ("Top Color", Color) = (1,1,1,1)
        _BottomColor ("Bottom Color", Color) = (0,0,0,1)
        _GradientOffset ("Offset", Range(-0.5, 0.5)) = 0
        _GradientSmoothness ("Smoothness", Range(0.01, 1)) = 0.5
        
        [Header(Appearance)]
        _MainTex ("Main Texture", 2D) = "white" {}
        _TextureIntensity ("Texture Intensity", Range(0, 1)) = 1
        
        [Header(Advanced)]
        [Toggle]_UseGammaCorrection ("Gamma Correction", Float) = 1
        [Toggle(ALPHA_CLIP)] _AlphaClip ("Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            ZTest [UnityUI.ZTestMode]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local _USEGAMMACORRECTION_ON
            #pragma multi_compile_local __ ALPHA_CLIP
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _TopColor;
            fixed4 _BottomColor;
            float _GradientOffset;
            float _GradientSmoothness;
            float _TextureIntensity;

            // Объявляем _ClipRect для GLES
            float4 _ClipRect;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPosition = v.vertex;
                
                // Рассчет позиции градиента с плавностью
                float gradientPos = saturate((v.uv.y + _GradientOffset) / _GradientSmoothness);
                
                // Гамма-корректная интерполяция
                #if _USEGAMMACORRECTION_ON
                float3 bottom = pow(_BottomColor.rgb, 2.2);
                float3 top = pow(_TopColor.rgb, 2.2);
                float3 final = lerp(bottom, top, gradientPos);
                final = pow(final, 1.0/2.2);
                #else
                float3 final = lerp(_BottomColor.rgb, _TopColor.rgb, gradientPos);
                #endif
                
                o.color = float4(final, lerp(_BottomColor.a, _TopColor.a, gradientPos)) * v.color;
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed4 col = i.color;
                
                // Смешивание с текстурой
                col.rgb = lerp(col.rgb, tex.rgb * col.rgb, _TextureIntensity * tex.a);
                col.a *= tex.a;
                
                // Обрезка по прямоугольнику UI (только если определено)
                #ifdef UNITY_UI_CLIP_RECT
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif
                
                // Альфа-клиппинг
                #ifdef ALPHA_CLIP
                clip(col.a - 0.001);
                #endif
                
                return col;
            }
            ENDCG
        }
    }
    Fallback "UI/Default"
}