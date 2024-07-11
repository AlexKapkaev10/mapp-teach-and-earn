Shader "Custom/LinearGradient"
{
    Properties 
    {
        _TopColor ("Color Top", Color) = (1,1,1,1)
        _BottomColor ("Color Bottom", Color) = (1,1,1,1)
        _Factor ("Gradient Factor", Range(0, 0.5)) = 0
        _MainTex ("Main Texture", 2D) = "white" {}
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil Ref", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
        }

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Pass
        {
            Cull Off
            Lighting Off
            ZWrite Off
            ZTest [unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
                        
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _TopColor;
            fixed4 _BottomColor;
            fixed _Factor;
 
            struct v2f 
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };
 
            v2f vert (appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                o.color = lerp(_BottomColor, _TopColor, v.texcoord.y + _Factor);
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                 float4 color;
                 color.rgb = i.color.rgb;
                 color.a = tex2D (_MainTex, i.uv).a * i.color.a;
                 return color;
            }
            ENDCG
        }
    }
}