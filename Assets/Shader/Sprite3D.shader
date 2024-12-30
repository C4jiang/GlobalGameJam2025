Shader "3D Sprite" {
Properties {
    _MainTex ("Particle Texture", 2D) = "white" {}
    _TintColor ("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
    _Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
}

Category {
    Tags {"IgnoreProjector"="False" "RenderType"="Opaque" }//"Queue"="Transparent"
    Blend SrcAlpha OneMinusSrcAlpha
    Cull Off Lighting Off ZWrite On Fog { Mode Off }
    
    BindChannels {
        Bind "Color", color
        Bind "Vertex", vertex
        Bind "TexCoord", texcoord
    }
    
    SubShader {
        Pass {
            AlphaTest Greater [_Cutoff]
            SetTexture [_MainTex] {
                constantColor [_TintColor]
                combine texture * constant Double
            }

            SetTexture [_MainTex] {
                combine previous * primary
            }
        }
    }
}
}
