using UnityEngine;
public class CustomerAvatar : MonoBehaviour {
    private Animator _avatarAnimator;

    private void Awake() {
        _avatarAnimator = GetComponent<Animator>();
    }

    public void PlayAnimation(string animationName) {
        _avatarAnimator.SetTrigger(animationName);
    }
}