///////////////////////////////////////////
//  CameraFilterPack - by VETASOFT 2020 ///
///////////////////////////////////////////

using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[AddComponentMenu ("Camera Filter Pack/Noise/TV")]
public class CameraFilterPack_Noise_TV : MonoBehaviour {
#region Variables
public Shader SCShader;
private float TimeX = 1.0f;
 
private Material SCMaterial;
[Range(0f, 1f)]
public float Fade = 1f;
[Range(0f, 10f)]
private float Value2 = 1f;
[Range(0f, 10f)]
private float Value3 = 1f;
[Range(0f, 10f)]
private float Value4 = 1f;

private Texture2D Texture2;


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

Texture2 = Resources.Load ("CameraFilterPack_TV_Noise") as Texture2D;

SCShader = Shader.Find("CameraFilterPack/Noise_TV");

}

void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
{
if(SCShader != null)
{
TimeX+=Time.deltaTime;
if (TimeX>100)  TimeX=0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("_Value", Fade);
material.SetFloat("_Value2", Value2);
material.SetFloat("_Value3", Value3);
material.SetFloat("_Value4", Value4);
material.SetVector("_ScreenResolution",new Vector4(sourceTexture.width,sourceTexture.height,0.0f,0.0f));
material.SetTexture("Texture2", Texture2);

Graphics.Blit(sourceTexture, destTexture, material);
}
else
{
Graphics.Blit(sourceTexture, destTexture);
}
}
void Update ()
{

#if UNITY_EDITOR
if (Application.isPlaying!=true)
{
SCShader = Shader.Find("CameraFilterPack/Noise_TV");
Texture2 = Resources.Load ("CameraFilterPack_TV_Noise") as Texture2D;

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
