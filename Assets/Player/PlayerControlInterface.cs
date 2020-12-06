using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerControl {
    void onUpAction();
    void onDownAction();
    void onRightAction();
    void onLeftAction();
}