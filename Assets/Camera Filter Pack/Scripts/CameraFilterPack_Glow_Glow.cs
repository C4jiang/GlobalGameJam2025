////////////////////////////////////////////
// CameraFilterPack - by VETASOFT 2020 /////
////////////////////////////////////////////

using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[AddComponentMenu ("Camera Filter Pack/Glow/Glow")]
public class CameraFilterPack_Glow_Glow : MonoBehaviour {
#region Variables
public Shader SCShader;
private float TimeX = 1.0f;
 
private Material SCMaterial;
[Range(0, 20)]
public float Amount = 4f;
[Range(2,16)]
public int FastFilter = 4;
[Range(0,1f)]
public float Threshold = 0.5f;
[Range(0,1f)]
public float Intensity = 0.75f;
[Range(-1,1f)]
public float Precision = 0.56f;
#endregion
#region Properties
Material material
{
get
{
if(SCMaterial == null)
{
SCMaterial = new Material(SCShader);
SCMaterial.hideFlags = HideFlags.HideAndDontSave;	
}
return SCMaterial;
}
}
#endregion
void Start () 
{
SCShader = Shader.Find("CameraFilterPack/Glow_Glow");

}
void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
{
if(SCShader != null)
{
int DownScale=FastFilter;
TimeX+=Time.deltaTime;
if (TimeX>100)  TimeX=0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("_Amount", Amount);
material.SetFloat("_Value1", Threshold);
material.SetFloat("_Value2", Intensity);
material.SetFloat("_Value3", Precision);
material.SetVector("_ScreenResolution",new Vector2(Screen.width/DownScale,Screen.height/DownScale));
int rtW = sourceTexture.width/DownScale;
int rtH = sourceTexture.height/DownScale;
if (FastFilter>1)
{
RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
buffer.filterMode=FilterMode.Trilinear;
Graphics.Blit(sourceTexture, buffer, material,3);
Graphics.Blit(buffer, buffer2, material,2);
Graphics.Blit(buffer2, buffer, material,0);
material.SetFloat("_Amount", Amount*2);
Graphics.Blit(buffer, buffer2, material,2);
Graphics.Blit(buffer2, buffer, material,0);
material.SetTexture("_MainTex2", buffer);
RenderTexture.ReleaseTemporary(buffer);
RenderTexture.ReleaseTemporary(buffer2);
Graphics.Blit(sourceTexture, destTexture, material,1);
}
else
{
Graphics.Blit(sourceTexture, destTexture, material,0);
}
}
else
{
Graphics.Blit(sourceTexture, destTexture);	
}
}
// Update is called once per frame
void Update () 
{
#if UNITY_EDITOR
if (Application.isPlaying!=true)
{
SCShader = Shader.Find("CameraFilterPack/Glow_Glow");
}
#endif
}
void OnDisable ()
{
if(SCMaterial)
{
DestroyImmediate(SCMaterial);	
}
}
}