using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSpriteShadow : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        OpenSpriteRenderShadow();
    }

    void OpenSpriteRenderShadow() {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
            spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
            spriteRenderer.receiveShadows = true;
            Debug.LogError("OpenSpriteRenderShadow");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
