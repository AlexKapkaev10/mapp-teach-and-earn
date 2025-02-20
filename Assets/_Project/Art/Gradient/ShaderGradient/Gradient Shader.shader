Shader "Custom/LinearGradient"
{
    Properties 
    {
        _TopColor ("Color Top", Color) = (1,1,1,1)
        _BottomColor ("Color Bottom", Color) = (1,1,1,1)
        _Factor ("Gradient Factor", Range(0, 0.5)) = 0
        _MainTex ("Main Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
        }

        Pass
        {
            Cull Off
            Lighting Off
            ZWrite Off
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
 
            struct appdata_t 
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };
 
            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = lerp(_BottomColor, _TopColor, v.texcoord.y + _Factor);
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                 fixed4 texColor = tex2Dlod(_MainTex, float4(i.uv, 0, 0));
                 return fixed4(i.color.rgb, texColor.a * i.color.a);
            }
            ENDCG
        }
    }
}
