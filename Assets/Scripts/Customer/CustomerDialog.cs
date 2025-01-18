using UnityEngine;
public class CustomerDialog : MonoBehaviour {
    private EDialogType _curDialogType;
    private void Awake() {
        _curDialogType = EDialogType.None;
    }

    public void PlayDialog(EDialogType dialogType) {
        _curDialogType = dialogType;
        // TODO: 播放对话，这里用协程可能会好点
        OnDialogComplete();
    }

    private void OnDialogComplete() {
        Messenger.Broadcast(MsgType.DialogComplete, _curDialogType);
        _curDialogType = EDialogType.None;
    }
}

