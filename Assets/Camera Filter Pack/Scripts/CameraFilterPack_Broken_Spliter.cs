// Camera Filter Pack v4.0.0
//
// by VETASOFT 2020
//

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Broken/Spliter")]
public class CameraFilterPack_Broken_Spliter : MonoBehaviour
{
#region Variables
public Shader SCShader;
private float TimeX = 1.0f;

private Material SCMaterial;
[Range(0, 1)]
private float __Speed = 1.0f;
[Range(0, 1)]
 public float _PosX = 0.5f;
[Range(0, 1)]
 public float _PosY = 0.5f;

#endregion
#region Properties
Material material
{
get
{
if (SCMaterial == null)
{
SCMaterial = new Material(SCShader);
SCMaterial.hideFlags = HideFlags.HideAndDontSave;
}
return SCMaterial;
}
}
#endregion
void Start()
{
SCShader = Shader.Find("CameraFilterPack/CameraFilterPack_Broken_Spliter");

}
void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
{
if (SCShader != null)
{
TimeX += Time.deltaTime;
if (TimeX > 100) TimeX = 0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("_Speed", __Speed);
material.SetFloat("PosX", _PosX);
material.SetFloat("PosY", _PosY);

material.SetVector("_ScreenResolution", new Vector4(sourceTexture.width, sourceTexture.height, 0.0f, 0.0f));
Graphics.Blit(sourceTexture, destTexture, material);
}
else
{
Graphics.Blit(sourceTexture, destTexture);
}
}
void Update()
{
#if UNITY_EDITOR
if (Application.isPlaying != true)
{
SCShader = Shader.Find("CameraFilterPack/CameraFilterPack_Broken_Spliter");
}
#endif
}
void OnDisable()
{
if (SCMaterial)
{
DestroyImmediate(SCMaterial);
}
}
}
