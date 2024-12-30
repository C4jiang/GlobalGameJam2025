using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {
    public Space RotationSpace = Space.Self;

    public Vector3 RotationSpeed = new Vector3(100f, 0f, 0f);

    public Quaternion defaultRotation = Quaternion.identity;

    public bool useRandomDir = false;

    private float rotateDir = 1;

    void OnEnable() {
        if (RotationSpace == Space.Self) {
            transform.localRotation = defaultRotation;
        }
        else {
            transform.rotation = defaultRotation;
        }

        if (useRandomDir) {
            rotateDir = Random.Range(0, 2) == 0 ? 1 : -1;
        }
    }

    protected virtual void Update() {
        transform.Rotate(RotationSpeed * rotateDir * Time.deltaTime, RotationSpace);
    }
}