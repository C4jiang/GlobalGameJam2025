using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AutoDissolve : MonoBehaviour {
    public AnimationCurve curve;
    public float duration = 1f;

    public void DODissolve() {
        Image dissolveImage = GetComponent<Image>();
        if(dissolveImage == null) {
            return;
        }
        var material = dissolveImage.material;
        material.SetFloat("_DissolvePower", 1);
        material.DOFloat(0, "_DissolvePower", duration).SetDelay(0.5f).SetEase(curve).OnComplete(() => {
            dissolveImage.gameObject.SetActive(false);
            material.SetFloat("_DissolvePower", 1);
        });
    }
}