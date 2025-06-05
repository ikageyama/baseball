using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using R3.Triggers;
using System;
using UnityEngine.UIElements;

public class AreaRange : MonoBehaviour
{
    //参照取得
    [SerializeField] PlayerStatusManager ps;
    [SerializeField] Score score;

    //初期値取得
    [SerializeField] List<Transform> initialtrans = new List<Transform>();
    [SerializeField] List<Vector3> initialscale = new List<Vector3>();

    [SerializeField] List<Vector3> scale = new List<Vector3>();

    //エリアのカウント数
    int count;

    private CompositeDisposable disposables = new();

    //得点エリア範囲の設定
    public void AreaSwitchPlayer(int areaIndex)
    {
        disposables.Clear();

        count = 0;
        scale.Clear();

        foreach (var t in score.area)
        {
            ps._range[areaIndex]
             .Subscribe(range =>
             {
                 scale.Add(new Vector3(initialscale[count].x * range, initialscale[count].y, initialscale[count].z * range));
                 t.transform.localScale = scale[count];
                 count++;
             })
             .AddTo(disposables);
        }
    }
    private void Awake()
    {
        //エリアの初期スケールの保存
        for (int i = 0; i < initialtrans.Count; i++)
        {
            initialscale[i] = new Vector3(initialtrans[i].localScale.x, initialtrans[i].localScale.y, initialtrans[i].localScale.z);
        }
    }

    void Start()
    {
        score = FindAnyObjectByType<Score>();
        ps = FindAnyObjectByType<PlayerStatusManager>();

        //エリア範囲が変わるたびに呼ばれる
        score.AreaSwitchPlayer
            .Subscribe(AreaSwitchPlayer)
            .AddTo(this);

        AreaSwitchPlayer(0);
        
    }

}
