using R3.Triggers;
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Operation : MonoBehaviour
{
    //操作画面
    [SerializeField] GameObject ope_canvas;
    //ルール画面
    [SerializeField] GameObject rules_canvas;

    //上記の各画面のフラグ
    [SerializeField] bool ope_tab;
    [SerializeField] bool rules_tab;

    void Start()
    {
        //操作説明の表示
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Tab))
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ => 
            {
                if (ope_tab)
                {
                    ope_canvas.SetActive(false);
                    ope_tab = false;
                }
                else
                {
                    rules_canvas.SetActive(false);
                    ope_canvas.SetActive(true);
                    ope_tab = true;
                    rules_tab = false;
                }
            });

        //ルール説明の表示
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.R))
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                if (rules_tab)
                {
                    rules_canvas.SetActive(false);
                    rules_tab = false;
                }
                else
                {
                    ope_canvas.SetActive(false);
                    rules_canvas.SetActive(true);
                    rules_tab = true;
                    ope_tab = false;
                }
            });
    }
}
