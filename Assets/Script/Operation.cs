using R3.Triggers;
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Operation : MonoBehaviour
{
    //������
    [SerializeField] GameObject ope_canvas;
    //���[�����
    [SerializeField] GameObject rules_canvas;

    //��L�̊e��ʂ̃t���O
    [SerializeField] bool ope_tab;
    [SerializeField] bool rules_tab;

    void Start()
    {
        //��������̕\��
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

        //���[�������̕\��
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
