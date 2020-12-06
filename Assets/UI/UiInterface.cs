using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UiInterface
{
    void OnCoinCollected();
    void UpdateDistance(float d);
    void ShowMenu();
    void HideMenu();
}
