// Camera Filter Pack v4.0.0
//
// by VETASOFT 2020
//

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Glitch/NewGlitch5")]
public class CameraFilterPack_NewGlitch5 : MonoBehaviour
{
#region Variables
public Shader SCShader;
private float TimeX = 1.0f;

private Material SCMaterial;
[Range(0, 1)]
public float __Speed = 1.0f;
[Range(0, 1)]
 public float _Fade = 1.0f;
[Range(0, 1)]
 public float _Parasite = 1.0f;
[Range(0, 0)]
 public float _ZoomX = 1.0f;
[Range(0, 0)]
 public float _ZoomY = 1.0f;
[Range(0, 0)]
 public float _PosX = 1.0f;
[Range(0, 0)]
 public float _PosY = 1.0f;

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
SCShader = Shader.Find("CameraFilterPack/CameraFilterPack_NewGlitch5");

}
void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
{
if (SCShader != null)
{
TimeX += Time.deltaTime;
if (TimeX > 100) TimeX = 0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("_Speed", __Speed);
material.SetFloat("Fade", _Fade);
material.SetFloat("Parasite", _Parasite);
material.SetFloat("ZoomX", _ZoomX);
material.SetFloat("ZoomY", _ZoomY);
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
SCShader = Shader.Find("CameraFilterPack/CameraFilterPack_NewGlitch5");
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
