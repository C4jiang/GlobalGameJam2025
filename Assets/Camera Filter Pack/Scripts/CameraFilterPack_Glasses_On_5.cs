////////////////////////////////////////////
// CameraFilterPack - by VETASOFT 2020 /////
////////////////////////////////////////////
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu ("Camera Filter Pack/Glasses/Futuristic Montain")]
public class CameraFilterPack_Glasses_On_5 : MonoBehaviour {
#region Variables
public Shader SCShader;
private float TimeX = 1.0f;
[Range(0f, 1f)]
public float Fade = 0.2f;
[Range(0f, 0.1f)]
public float VisionBlur = 0.005f;
public Color GlassesColor = new Color(0.1f, 0.1f, 0.1f, 1);
public Color GlassesColor2 = new Color(0.45f, 0.45f, 0.45f, 0.25f);
[Range(0f, 1f)]
public float GlassDistortion = 0.6f;
[Range(0f, 1f)]
public float GlassAberration = 0.3f;
[Range(0f, 1f)]
public float UseFinalGlassColor = 0f;

[Range(0f, 1f)]
public float UseScanLine = 0.4f;
[Range(1f, 512f)]
public float UseScanLineSize = 358f;

public Color GlassColor = new Color(0.1f, 0.3f, 1, 1);

private Material SCMaterial;
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
Texture2 = Resources.Load ("CameraFilterPack_Glasses_On6") as Texture2D;
SCShader = Shader.Find("CameraFilterPack/Glasses_On");

}

void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
{
if(SCShader != null)
{
TimeX+=Time.deltaTime;
if (TimeX>100)  TimeX=0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("UseFinalGlassColor", UseFinalGlassColor);
material.SetFloat("Fade", Fade);
material.SetFloat("VisionBlur", VisionBlur);
material.SetFloat("GlassDistortion", GlassDistortion);
material.SetFloat("GlassAberration", GlassAberration);
material.SetColor("GlassesColor", GlassesColor);
material.SetColor("GlassesColor2", GlassesColor2);
material.SetColor("GlassColor", GlassColor);
material.SetFloat("UseScanLineSize", UseScanLineSize);
material.SetFloat("UseScanLine", UseScanLine);
material.SetTexture("_MainTex2", Texture2);

Graphics.Blit(sourceTexture, destTexture, material);
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
SCShader = Shader.Find("CameraFilterPack/Glasses_On");
Texture2 = Resources.Load ("CameraFilterPack_Glasses_On6") as Texture2D;

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