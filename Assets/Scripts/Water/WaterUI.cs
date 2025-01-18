using UnityEngine;
using UnityEngine.UI;
public class WaterUI : MonoBehaviour {
    public Transform NormalWaterParent;
    public Transform UnstableWaterParent;

    private void Awake() {
        Messenger.AddListener<int, int>(MsgType.RefreshWaterCnt, RefreshWaterCnt);
    }

    private void RefreshWaterCnt(int normalCnt, int unstableCnt) {
        for (int i = 0; i < NormalWaterParent.childCount; i++) {
            var color = i < normalCnt? Color.blue: Color.white;
            NormalWaterParent.GetChild(i).gameObject.GetComponent<Image>().color = color;
        }
        for (int i = 0; i < UnstableWaterParent.childCount; i++) {
            var color = i < unstableCnt? Color.blue: Color.white;
            UnstableWaterParent.GetChild(i).gameObject.GetComponent<Image>().color = color;
        }
    }
}