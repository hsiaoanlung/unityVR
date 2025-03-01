﻿using System.Collections;
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
    public float endDelay = 1.5f;

    public Collider spawnCollider;
    public ObjectPool targetObjectPool;
    public float spawnProbabilty = 0.7f;
    public float spawnInterval = 1f;

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
            Debug.Log("End PlayPhase");
            yield return StartCoroutine(EndPhase());
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
        float spawnTimer = 0f;
        while(gameTimer>0f)
        {
            if(spawnTimer<=0f)
            {
                if(Random.value<spawnProbabilty)
                {
                    spawnTimer = spawnInterval;
                    Spawn();
                }
            }
            yield return null;
            gameTimer -= Time.deltaTime;
            spawnTimer -= Time.deltaTime;
            timerbar.fillAmount = gameTimer / gameDuration;
        }
        IsPlaying = false;
        yield return StartCoroutine(uiController.HidePlayerUI());
    }
    private void Spawn()
    {
        GameObject target = targetObjectPool.GetGameObjectFromPool();
        target.transform.position = SpawnPosition();
    }
    private Vector3 SpawnPosition()
    {
        Vector3 center = spawnCollider.bounds.center;
        Vector3 extents = spawnCollider.bounds.extents;
        float x = Random.Range(center.x - extents.x, center.x + extents.x);
        float y = Random.Range(center.y - extents.y, center.y + extents.y);
        float z = Random.Range(center.z - extents.z, center.z + extents.z/2);
        return new Vector3(x, y, z);
    }
    private IEnumerator EndPhase()
    {
        reticle.Hide();
        yield return StartCoroutine(uiController.ShowOutroUI());
        yield return new WaitForSeconds(endDelay);
        yield return StartCoroutine(selectionRadial.WaitForSelectionRadialToFill());
        yield return StartCoroutine(uiController.HideOutroUI());
    }
    
}

