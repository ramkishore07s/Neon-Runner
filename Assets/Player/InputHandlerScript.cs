using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private const int ACTION_UP = 0;
    private const int ACTION_DOWN = 1;
    private const int ACTION_RIGHT = 2;
    private const int ACTION_LEFT = 3;
    private const int ACTION_NULL = 4;

    private Vector3 mouseStartPosition;
    private Vector3 mouseEndPosition;

    private IPlayerControl playerControl;
    public PlayerScript player;

    void Start() {
        playerControl = (IPlayerControl) player;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            mouseStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) {
            mouseEndPosition = Input.mousePosition;
            int action = GetAction();
            TakeAction(action);

        }
    }

    void TakeAction(int action) {
        switch(action) {
            case ACTION_UP:
                playerControl.onUpAction();
                break;
            case ACTION_DOWN:
                playerControl.onDownAction();
                break;
            case ACTION_LEFT:
                playerControl.onLeftAction();
                break;
            case ACTION_RIGHT:
                playerControl.onRightAction();
                break;
        }
    }

    int GetAction() {
        float y_scale = Mathf.Abs(mouseEndPosition.y - mouseStartPosition.y);
        float x_scale = Mathf.Abs(mouseEndPosition.x - mouseStartPosition.x);

        if (y_scale >= x_scale) {
            if (mouseEndPosition.y > mouseStartPosition.y) return ACTION_UP;
            if (mouseEndPosition.y < mouseStartPosition.y) return ACTION_DOWN;
        } else {
            if (mouseEndPosition.x > mouseStartPosition.x) return ACTION_RIGHT;
            if (mouseEndPosition.x < mouseStartPosition.x) return ACTION_LEFT;
        }

        return ACTION_NULL;
    }
}
