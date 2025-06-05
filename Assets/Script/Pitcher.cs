using R3;
using R3.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher : MonoBehaviour
{
    [SerializeField] Animator anim;

    //ボールを生成する型
    [SerializeField] GameObject ball;
    [SerializeField] Transform ballPlace;

    [SerializeField] CameraManager CManager;
    [SerializeField] GManager gm;

    //回転軸のランダム化
    [SerializeField] int _rand;
    [SerializeField] float angle;

    //ボールのスピード
    public SerializableReactiveProperty<float> sp = new SerializableReactiveProperty<float>();

    //ピッチングアニメーションの判定
    public SerializableReactiveProperty<bool> pitting_start = new SerializableReactiveProperty<bool>();
    bool pittingfinish;

    void Start()
    {
        CManager = FindAnyObjectByType<CameraManager>();
        gm = FindAnyObjectByType<GManager>();

        _rand = UnityEngine.Random.Range(-49, 50);

        gm.balldeath
            .Where(death => death && !gm.cameraswitch.Value && !gm.roundNext.Value)
            .Delay(TimeSpan.FromSeconds(5))
            .Subscribe(_ => PitcherAnim());

        gm.balldeath
           .Where(death => death && !gm.cameraswitch.Value && gm.roundNext.Value)
           .Delay(TimeSpan.FromSeconds(10))
           .Subscribe(_ => PitcherAnim());

        //投球の終了時にポジションの修正
        this.UpdateAsObservable()
            .Where(_ => pittingfinish)
            .Delay(TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ =>
            {
                this.gameObject.transform.position = new Vector3(0.3f, 0.05f, -0.5f);
                this.gameObject.transform.eulerAngles = new Vector3(0, -43, 0);
                pittingfinish = false;
            });
    }

    void PitcherAnim()
    {
        anim.SetBool("pitting", true);
        gm.balldeath.Value = false;
        gm.magicuse = false;
    }

    //ボールの生成
    void BallAnim()
    {
        Instantiate(ball, ballPlace);
        _rand = UnityEngine.Random.Range(-49, 50);
        ballPlace.eulerAngles = new Vector3(1.65f + angle * _rand, -44.5f + angle * _rand, -1.4f + angle * _rand);
        
    }

    void FinishAnim()
    {
        anim.SetBool("pitting", false);
        pittingfinish = true;
        gm.magicuse = true;
    }
}
