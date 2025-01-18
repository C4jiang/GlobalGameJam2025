// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

////////////////////////////////////////////
// CameraFilterPack - by VETASOFT 2020 /////
////////////////////////////////////////////

Shader "CameraFilterPack/TV_Old_Movie_3" {
Properties 
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_TimeX ("Time", Range(0.0, 1.0)) = 1.0
_Distortion ("_Distortion", Range(1.0, 10.0)) = 1.0
}
SubShader 
{
Pass
{
Cull Off ZWrite Off ZTest Always
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma target 3.0
#include "UnityCG.cginc"


uniform sampler2D _MainTex;
uniform float _TimeX;
uniform float _Value;
uniform float _Value2;
uniform float _Value3;
uniform float _Value4;
uniform float _Fade;
uniform float _Distortion;

uniform float periodRand = 1;
uniform float periodRandSave = 1;
uniform float nextTime = 0.5;

struct appdata_t
{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;
};

struct v2f
{
float2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
float4 color    : COLOR;
};

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;

return OUT;
}

float rand(float2 co){
//frac的本质是 https://zhuanlan.zhihu.com/p/158462351
return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
}

//生成0-1小数
float rand(float c){
return rand(float2(c,1.0));
}

float randomLine(float seed, float2 uv, float t, float periodRand)
{
float aa = rand(seed+1.0);

float startLine = rand(periodRand + 1.0) / 2;
float speed = rand(periodRand * 3 + 1.0);
float lor = rand(periodRand * 20  + 10.5 + 4.* float(0));
float black = rand(periodRand * 60.2457  + 7.35 + 11.* float(0));
// float startLine = 0.2;
// float speed = 2;

float lineCenter = 0;
float lineCenterL2R = startLine + t / (20 + speed * 130) - floor(t / (20 + speed * 130));
float lineCenterR2K = 1 - startLine - (t / (20 + speed * 130) - floor(t / (20 + speed * 130)));
if(lor > 0.5){
    lineCenter = lineCenterL2R;
}else{
    lineCenter = lineCenterR2K;
}

float linecolor = 1;
if(uv.x > lineCenter - 0.01 & uv.x < lineCenter + 0.01){
    linecolor = 8 * lerp(0.1,1,(sin((uv.y + aa) * 4) + 1) / 2) * (pow((0.01 - abs(uv.x - lineCenter)), 2) * 800) + 1;
    if(black < 0.2){
        linecolor = 1 / linecolor;
    }
}
return linecolor;
}


half4 _MainTex_ST;
float4 frag(v2f i) : COLOR
{
float2 uvst = UnityStereoScreenSpaceUVAdjust(i.texcoord, _MainTex_ST);
float2 uv;
uv  = uvst;
float t = float(int(_TimeX * _Value));
float2 suv = uv;// + 0.002 * float2( rand(t), rand(t + 23.0));
float3 image = tex2D( _MainTex, float2(suv.x, suv.y) );
float luma = dot( float3(0.2126, 0.7152, 0.0722), image );

float3 oldImage = luma * float3(0.7+_Value3, 0.7+_Value3/2, 0.7)*_Value2;
oldImage = oldImage * float3(0.7+_Value3, 0.7+_Value3/8, 0.7)*_Value2;

float randx=rand(t + 8.);

//初始的时候加入一层黑边
// float vI = 16.0 * (uv.x * (1.0-uv.x) * uv.y * (1.0-uv.y));
float vI = 1;

//随机lerp让屏幕闪烁
// vI *= lerp( 0.7, 1.0, randx+.5);

//单纯的提亮度
vI += 0.6;

//四边加上黑角的效果
// vI *= pow(16.0 * uv.x * (1.0-uv.x) * uv.y * (1.0-uv.y), 0.4);

periodRand = floor(t / (5 + nextTime * 10)) +10.0+3.* float(0);
if(periodRandSave != periodRand){
    nextTime = randx;
}
periodRandSave = periodRand;

// int l = int(8.0 * randx);
vI *= randomLine( t+6.0+17.* float(0), uv, t, periodRand);
vI *= randomLine( t+6.0+17.* float(0), uv, t, periodRand * 12.9898);
// vI *= randomLine( t+3.0+17.* float(0), uv);
// if ( 1 < l ) vI *= randomLine( t+6.0+17.* float(1),uv);

// int s = int( max(8.0 * rand(t+18.0) -2.0, 0.0 ));
float4 result = float4(oldImage * vI, 1.0);
result = lerp(result, tex2D(_MainTex, uvst.xy), 1-_Fade);
return result;

}

ENDCG
}

}
}