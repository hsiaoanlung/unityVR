using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;
using UnityEngine.UI;//命名空間
public class ShootingGalleryController : MonoBehaviour//Class類別
{
    public UIController uiController;//欄位
    public Reticle reticle;
    public SelectionRadial selectionRadial;
    public SelectionSlider selectionSlider;

    public Image timerbar;
    public float gameDuration = 30f;

    public bool IsPlaying//屬性
    {
        private set;
        get;
    }
    private IEnumerator Start()
    {
        SessionData.SetGameType(SessionData.GameType.SHOOTER180);
        while(true)
        {
            Debug.Log("Start StartPhase");
            yield return StartCoroutine(StartPhase());
            Debug.Log("Start PlayPhase");
            yield return StartCoroutine(PlayPhase());
            Debug.Log("Complete");
        }
    }
    private IEnumerator StartPhase()
    {
        yield return StartCoroutine(uiController.ShowIntroUi());
        reticle.Show();
        selectionRadial.Hide();
        yield return StartCoroutine(selectionSlider.WaitForBarToFill());
        yield return StartCoroutine(uiController.HideIntroUI());
    }

    private IEnumerator PlayPhase()
    {
        yield return StartCoroutine(uiController.ShowPlayerUI());
        IsPlaying = true;
        reticle.Show();
        SessionData.Restart();
        float gameTimer = gameDuration;
        while(gameTimer>0f)
        {
            yield return null;
            gameTimer -= Time.deltaTime;
            timerbar.fillAmount = gameTimer / gameDuration;
        }
        IsPlaying = false;
    }
}
