using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {

    private string horizontal = "";
    private string vertical = "";
    private string bullet = "";
    private string bomb = "";
    private string laser = "";
    private string shield = "";
    private string shareX = "";
    private string shareY = "";

    public void SetupPlayerInput(int _playerID) {
        if (_playerID == 1) {
            horizontal = "P1Horizontal";
            vertical = "P1Vertical";
            bullet = "P1Fire";
            bomb = "P1Bomb";
            laser = "P1Laser";
            shield = "P1Shield";
            shareX = "P1ShareX";
            shareY = "P1ShareY";

            if (GameManager.instance.currentGameMode != "" && GameManager.instance.currentGameMode == "CoopMode" && Input.GetJoystickNames().Length <= 1) {
                horizontal = "P1Horizontal_Keyboard";
                vertical = "P1Vertical_Keyboard";
                bullet = "P1Fire_Keyboard";
                bomb = "P1Bomb_Keyboard";
                laser = "P1Laser_Keyboard";
                shield = "P1Shield_Keyboard";
                shareX = "P1ShareX_Keyboard";
                shareY = "P1ShareY_Keyboard";
            }
            
        }

        if (_playerID == 2) {
            horizontal = "P2Horizontal";
            vertical = "P2Vertical";
            bullet = "P2Fire";
            bomb = "P2Bomb";
            laser = "P2Laser";
            shield = "P2Shield";
            shareX = "P2ShareX";
            shareY = "P2ShareY";

            if (Input.GetJoystickNames().Length <= 1) {
                horizontal = "P2Horizontal_Joystick_1";
                vertical = "P2Vertical_Joystick_1";
                bullet = "P2Fire_Joystick_1";
                bomb = "P2Bomb_Joystick_1";
                laser = "P2Laser_Joystick_1";
                shield = "P2Shield_Joystick_1";
                shareX = "P2ShareX_Joystick_1";
                shareY = "P2ShareY_Joystick_1";
            }
            
        }
    }

    public float MoveHorizontal() {
        return (Input.GetAxis(horizontal));
    }

    public float MoveVertical() {
        return (Input.GetAxis(vertical));
    }

    public bool ShootBullet() {
        return (Input.GetButton(bullet) || Input.GetAxis(bullet) > 0.1f);
    }

    public bool ShootBomb() {
        return (Input.GetButtonDown(bomb));
    }

    public bool ShootLaser() {
        return (Input.GetButtonDown(laser));
    }

    public bool OnShield() {
        return (Input.GetButton(shield) || Input.GetAxis(shield) > 0.1f);
    }

    public float OnShareX() {
        return (Input.GetAxis(shareX));
    }

    public float OnShareY() {
        return (Input.GetAxis(shareY));
    }
}
