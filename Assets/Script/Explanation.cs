using R3;
using R3.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Explanation : MonoBehaviour
{
    [SerializeField] GameObject exp_canvas;

    [SerializeField] bool exp_tab;

    void Start()
    {
        //Tab‚ð‰Ÿ‚µ‚½Žž‚Ì‰æ–ÊØ‚è‘Ö‚¦
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Tab))
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                if (exp_tab)
                {
                    exp_canvas.SetActive(false);
                    exp_tab = false;
                }
                else
                {
                    exp_canvas.SetActive(true);
                    exp_tab = true;
                }
            });
    }
}
