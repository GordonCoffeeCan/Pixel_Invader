using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ControllerNames : MonoBehaviour {
    [SerializeField]private string[] joystickNames = new string[2];

    private void Update() {
        if (Input.GetJoystickNames().Length > 0) {
            for (int i = 0; i < Input.GetJoystickNames().Length; i++) {
                joystickNames[i] = Input.GetJoystickNames()[i];
            }
        }
    }
}
