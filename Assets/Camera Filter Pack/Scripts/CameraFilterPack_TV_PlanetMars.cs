///////////////////////////////////////////
//  CameraFilterPack - by VETASOFT 2020 ///
///////////////////////////////////////////

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu ("Camera Filter Pack/TV/Planet Mars")]
public class CameraFilterPack_TV_PlanetMars : MonoBehaviour {
#region Variables
public Shader SCShader;
 
[Range(0, 1)]
public float Fade = 1;
private float TimeX = 1.0f;
[Range(-10, 10)]
public float Distortion = 1.0f;
private Material SCMaterial;



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
SCShader = Shader.Find("CameraFilterPack/TV_PlanetMars");


}

void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
{
if(SCShader != null)
{
TimeX+=Time.deltaTime;
if (TimeX>100)  TimeX=0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("_Distortion", Distortion);
material.SetFloat("Fade", Fade);
material.SetVector("_ScreenResolution",new Vector4(sourceTexture.width,sourceTexture.height,0.0f,0.0f));
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
SCShader = Shader.Find("CameraFilterPack/TV_PlanetMars");

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