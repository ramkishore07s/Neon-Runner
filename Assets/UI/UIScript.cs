using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour, UiInterface
{
    public TextMeshProUGUI distanceText, coinsText;
    public Animation MainMenuAnimation;

    private int score = 0;
    private float coins = 0;
    public float time;

    void Update() {
        time += Time.deltaTime;
    }
    void UiInterface.OnCoinCollected() {
        coins += 0.5f;
        coinsText.SetText((int) coins + "");
    }
    void UiInterface.UpdateDistance(float d) {
        //distanceText.SetText("Score: " + (int) (time));
    }

    void UiInterface.ShowMenu() {
        MainMenuAnimation.Play("ShowMainMenu");
    }

    void UiInterface.HideMenu() {
        MainMenuAnimation.Play("HideMainMenu");
    }

    
}
