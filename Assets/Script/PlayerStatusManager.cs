using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using R3;
using R3.Triggers;
using TMPro;
using System;

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager instance = null;

    //ステータスレベル
    public SerializableReactiveProperty<int>[] _meatlevel = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] _rangelevel = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] _powerlevel = new SerializableReactiveProperty<int>[2];
    
    //ステータス
    public SerializableReactiveProperty<float>[] _meat = new SerializableReactiveProperty<float>[2];
    public SerializableReactiveProperty<float>[] _range = new SerializableReactiveProperty<float>[2];
    public SerializableReactiveProperty<int>[] _power = new SerializableReactiveProperty<int>[2];

    //各レベルのステータス値参照用
    public List<float> meatlist = new List<float>();
    public List<float> rangelist = new List<float>();
    public List<int> powerlist = new List<int>();

    //スキルカウント
    public SerializableReactiveProperty<int>[] flame_count = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] bspeed_count = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] dspeed_count = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] dpower_count = new SerializableReactiveProperty<int>[2];

    //各スキルのエフェクト
    public List<GameObject> skilleffect = new List<GameObject>();

    //プレイヤーのデータ切り替え変数
    public SerializableReactiveProperty<int> player_id = new SerializableReactiveProperty<int>();

    [SerializeField] GManager gm;

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
        gm = FindAnyObjectByType<GManager>();

        gm.game_end
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
