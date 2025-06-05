using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using R3;
using R3.Triggers;
using TMPro;
using System;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    public SerializableReactiveProperty<int>[] score = new SerializableReactiveProperty<int>[12];

    //スキルの選択
    public List<SerializableReactiveProperty<bool>> skillbool = new List<SerializableReactiveProperty<bool>>();

    //ラウンド数
    public SerializableReactiveProperty<int> _round = new SerializableReactiveProperty<int>();

    //次のラウンドへの移行bool
    public SerializableReactiveProperty<bool> roundNext = new SerializableReactiveProperty<bool>();

    //相手ターンのスキル効果を戻す際のbool
    public SerializableReactiveProperty<bool> nextturn = new SerializableReactiveProperty<bool>();
    public SerializableReactiveProperty<bool> skillfinish = new SerializableReactiveProperty<bool>();

    //アウトカウントの変数
    public SerializableReactiveProperty<int> outCount = new SerializableReactiveProperty<int>();

    //スコアの更新
    public SerializableReactiveProperty<bool> scorerenewal = new SerializableReactiveProperty<bool>();

    //ゲームの終了判定
    public SerializableReactiveProperty<bool> game_end = new SerializableReactiveProperty<bool>();

    //ボールの存在判定
    public SerializableReactiveProperty<bool> balldeath = new SerializableReactiveProperty<bool>();

    //カメラの切り替え
    public SerializableReactiveProperty<bool> cameraswitch = new SerializableReactiveProperty<bool>();

    //魔法を使用しているかどうか
    public bool magicuse = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        game_end
            .Where(end => end)
            .Subscribe(_ => Destroy(gameObject));
    }

    //このスクリプト（クラス）が破棄されるときインスタンスも破棄する。
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }

    }
}
