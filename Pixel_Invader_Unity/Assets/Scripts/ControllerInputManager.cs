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
