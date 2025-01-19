using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Customer : MonoBehaviour
{
    public LevelData levelData;
    public int levelNumber;

    private CustomerDialog _curCustomDialog;
    private CustomerAvatar _curCustomAvatar;
    
    public float CustomTimer => _customTimer;
    private float _customTimer = 0f;
    private bool _customStart = false;
    
    Coroutine _curDialogCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned.");
            return;
        }

        Messenger.AddListener<int>(MsgType.CreateCustomer, OnCreateCustomer);
        Messenger.AddListener(MsgType.BubbleSuccess, OnBubbleSuccess);
        Messenger.AddListener(MsgType.BubbleFail, OnBubbleFail);
    }

    private void Update() {
        if (_customStart) {
            _customTimer += Time.deltaTime;
        }
    }

#region handler
    private void OnCreateCustomer(int levelNumber) {
        CleanCustom();
        LevelData.Level level = GetLevel(levelNumber);
        if (level != null && level.prefab != null)
        {
            GameObject prefabInstance = Instantiate(level.prefab, transform);
            prefabInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.left * 1000; // 可根据需要调整位置
            prefabInstance.GetComponent<RectTransform>().DOAnchorPos(Vector2.right * 200, 9f).SetEase(Ease.OutCubic);
            _curCustomAvatar = prefabInstance.GetComponent<CustomerAvatar>();
            
            GameObject dialogInstance = Instantiate(level.dialogPrefab, transform);
            _curCustomDialog = dialogInstance.GetComponent<CustomerDialog>();

            OnCustomIntro();
        }
        else
        {
            Debug.LogError("Level or Prefab is not found.");
            Messenger.Broadcast(MsgType.NoCustomer);
        }
    }

    private void OnBubbleSuccess() {
        if (_curDialogCoroutine != null)
        {
            StopCoroutine(_curDialogCoroutine);
            _curDialogCoroutine = null;
        }
        _curCustomAvatar.PlayAnimation("Success");
        _curCustomAvatar.GetComponent<RectTransform>().DOAnchorPos(Vector2.left * 1000, 9f).SetEase(Ease.OutCubic);
        _curDialogCoroutine = StartCoroutine(_curCustomDialog.PlayDialog(GetLevel(levelNumber).successBubbleDialogs, EDialogType.Succ));
        // todo 播放成功动画
    }

    private void OnBubbleFail() {
        if (_curDialogCoroutine != null)
        {
            StopCoroutine(_curDialogCoroutine);
            _curDialogCoroutine = null;
        }
        _curDialogCoroutine = StartCoroutine(_curCustomDialog.PlayDialog(GetLevel(levelNumber).failBubbleDialogs, EDialogType.Fail));
    }
#endregion

#region  utils
    private void OnCustomIntro()
    {
        StartCoroutine(CustomIntro());
    }

    IEnumerator CustomIntro()
    {
        _curCustomAvatar.PlayAnimation("Idle");
        yield return new WaitForSeconds(2.5f);
        _curCustomAvatar.PlayAnimation("Speak");
        yield return _curCustomDialog.PlayDialog(GetLevel(levelNumber).introDialogs, EDialogType.Intro);
        _curCustomAvatar.PlayAnimation("Idle");
        _customStart = true;
    }

    private LevelData.Level GetLevel(int levelIndex)
    {
        var fullLevels = levelData.GetLevels();
        if (levelIndex >= 0 && levelIndex < fullLevels.Length)
        {
            Debug.Log(levelIndex);
            return fullLevels[levelIndex];
        }
        else
        {
            Debug.Log("no levels!!");
            return null;
        }
    }

    private void CleanCustom() {
        // todo 清理上次的客户
        if (_curCustomAvatar != null)
        {
            Destroy(_curCustomAvatar);
            _curCustomAvatar = null;
        }
        if (_curCustomDialog != null)
        {
            Destroy(_curCustomDialog);
            _curCustomDialog = null;
        }
        _customStart = false;
        _customTimer = 0f;
    }
#endregion
}
