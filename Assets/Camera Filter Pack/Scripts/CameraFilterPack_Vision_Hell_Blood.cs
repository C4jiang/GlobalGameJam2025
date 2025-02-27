///////////////////////////////////////////
//  CameraFilterPack - by VETASOFT 2020 ///
///////////////////////////////////////////

using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[AddComponentMenu ("Camera Filter Pack/Vision/Hell_Blood")]
public class CameraFilterPack_Vision_Hell_Blood : MonoBehaviour {
#region Variables
public Shader SCShader;
private float TimeX = 1.0f;
 
private Material SCMaterial;
[Range(0f, 1f)]
public float Hole_Size = 0.57f;
[Range(0f, 0.5f)]
public float Hole_Smooth = 0.362f;
[Range(-2f, 2f)]
public float Hole_Speed = 0.85f;
[Range(-10f, 10f)]
public float Intensity = 0.24f;
public Color ColorBlood = new Color(1, 0, 0, 1);
[Range(-1f, 1f)]
public float BloodAlternative1= 0f;
    [Range(-1f, 1f)]
    public float BloodAlternative2 = 0f;
    [Range(-1f, 1f)]
    public float BloodAlternative3 = -1f;

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
SCShader = Shader.Find("CameraFilterPack/Vision_Hell_Blood");

}

void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
{
if(SCShader != null)
{
TimeX+=Time.deltaTime;
if (TimeX>100)  TimeX=0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("_Value", Hole_Size);
material.SetFloat("_Value2", Hole_Smooth);
material.SetFloat("_Value3", Hole_Speed*15);
material.SetColor("ColorBlood", ColorBlood);
material.SetFloat("_Value4", Intensity);
            material.SetFloat("BloodAlternative1", BloodAlternative1);
            material.SetFloat("BloodAlternative2", BloodAlternative2);
            material.SetFloat("BloodAlternative3", BloodAlternative3);
            material.SetVector("_ScreenResolution",new Vector4(sourceTexture.width,sourceTexture.height,0.0f,0.0f));
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
SCShader = Shader.Find("CameraFilterPack/Vision_Hell_Blood");
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
