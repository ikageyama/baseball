using R3;
using R3.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect : MonoBehaviour
{
    [SerializeField] List<RectTransform> skill_trans = new List<RectTransform>();

    //スキル個数テキスト
    [SerializeField] List<TextMeshProUGUI> countText = new List<TextMeshProUGUI>();

    [SerializeField] PlayerStatusManager ps;
    [SerializeField] GManager gm;
    [SerializeField] Score score;

    [SerializeField] AudioSource selectsource;
    [SerializeField] AudioClip selectclip;

    private CompositeDisposable disposables = new();
    enum Skillname
    {
        flame,//炎エフェクトを纏い1ラウンドの間パワー増加
        slow,//ピッチャーの弾を遅くするバフ
        fast,//相手のターン時にピッチャーの弾を速くするデバフ
        power,//相手のターン時にパワー減少のデバフ
    }

    //スキルの個数設定
    public void CountSwitchPlayer(int countIndex)
    {
        disposables.Clear();

        ps.flame_count[countIndex]
           .Subscribe(count => countText[(int)Skillname.flame].text = "" + count)
           .AddTo(disposables);

        ps.bspeed_count[countIndex]
            .Subscribe(count => countText[(int)Skillname.slow].text = "" + count)
            .AddTo(disposables);

        ps.dspeed_count[countIndex]
            .Subscribe(count => countText[(int)Skillname.fast].text = "" + count)
            .AddTo(disposables);

        ps.dpower_count[countIndex]
            .Subscribe(count => countText[(int)Skillname.power].text = "" + count)
            .AddTo(disposables);
    }

    void Start()
    {
        gm = FindAnyObjectByType<GManager>();
        ps = FindAnyObjectByType<PlayerStatusManager>();

        score.CountSwitchPlayer
            .Subscribe(CountSwitchPlayer)
            .AddTo(this);

        CountSwitchPlayer(0);

        ps.player_id.Value = 0;

        //スキルを選択
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Alpha1) && !gm.skillbool[(int)Skillname.flame].Value)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ => FlameSkillBool());

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Alpha2) && !gm.skillbool[(int)Skillname.slow].Value)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ => SlowSkillBool());

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Alpha3) && !gm.skillbool[(int)Skillname.fast].Value)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ => FastSkillBool());

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Alpha4) && !gm.skillbool[(int)Skillname.power].Value)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ => PowerSkillBool());
        
        //スキルを選択した場合、大きく表示する
        for (int i = 0; i < 4; i++) {
            int localCache = i;
            gm.skillbool[i]
                .Subscribe(skill =>
                {
                    if (skill)
                    {
                        skill_trans[localCache].anchoredPosition = new Vector2(-95, 0);
                        skill_trans[localCache].localScale = new Vector2(1.1f, 1.1f);
                    }

                    if (!skill)
                    {
                        skill_trans[localCache].anchoredPosition = new Vector2(0, 0);
                        skill_trans[localCache].localScale = new Vector2(1, 1);
                    }
                })
                .AddTo(this);
            }
        
    }

    void FlameSkillBool()
    {
        selectsource.PlayOneShot(selectclip);
        gm.skillbool[(int)Skillname.flame].Value = true;
        gm.skillbool[(int)Skillname.slow].Value = false;
        gm.skillbool[(int)Skillname.fast].Value = false;
        gm.skillbool[(int)Skillname.power].Value = false;
    }

    void SlowSkillBool()
    {
        selectsource.PlayOneShot(selectclip);
        gm.skillbool[(int)Skillname.flame].Value = false;
        gm.skillbool[(int)Skillname.slow].Value = true;
        gm.skillbool[(int)Skillname.fast].Value = false;
        gm.skillbool[(int)Skillname.power].Value = false;
    }
    void FastSkillBool()
    {
        selectsource.PlayOneShot(selectclip);
        gm.skillbool[(int)Skillname.flame].Value = false;
        gm.skillbool[(int)Skillname.slow].Value = false;
        gm.skillbool[(int)Skillname.fast].Value = true;
        gm.skillbool[(int)Skillname.power].Value = false;
    }

    void PowerSkillBool()
    {
        selectsource.PlayOneShot(selectclip);
        gm.skillbool[(int)Skillname.flame].Value = false;
        gm.skillbool[(int)Skillname.slow].Value = false;
        gm.skillbool[(int)Skillname.fast].Value = false;
        gm.skillbool[(int)Skillname.power].Value = true;
    }
}
