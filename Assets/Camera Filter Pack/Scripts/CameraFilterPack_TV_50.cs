///////////////////////////////////////////
//  CameraFilterPack - by VETASOFT 2020 ///
///////////////////////////////////////////
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu ("Camera Filter Pack/TV/50s")]
public class CameraFilterPack_TV_50 : MonoBehaviour {
#region Variables
public Shader SCShader;
private float TimeX = 1.0f;
 
private Material SCMaterial;
[Range(0f, 1f)]
public float Fade = 1f;
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
SCShader = Shader.Find("CameraFilterPack/TV_50");


}

void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
{
if(SCShader != null)
{
TimeX+=Time.deltaTime;
if (TimeX>100)  TimeX=0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("_Fade", Fade);
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
SCShader = Shader.Find("CameraFilterPack/TV_50");

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