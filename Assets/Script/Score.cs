using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;
using R3.Triggers;
using System;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    //ラウンド毎のスコアテキスト
    public List<TextMeshProUGUI> scoretext = new List<TextMeshProUGUI>();

    //得点エリア
    public List<GameObject> area = new List<GameObject>();

    //アウトカウントのテキスト
    [SerializeField] TextMeshProUGUI countText;

    //スコアの保持
    int scorehold;
    bool scoreNext;
    
    //プレイヤーの合計スコア
    int P1score;
    int P2score;

    [SerializeField] GManager gm;
    [SerializeField] PlayerStatusManager psm;

    public Subject<int> CountSwitchPlayer = new();
    public Subject<int> MeatSwitchPlayer = new();
    public Subject<int> AreaSwitchPlayer = new();

    void Start()
    {
        gm = FindAnyObjectByType<GManager>();
        psm = FindAnyObjectByType<PlayerStatusManager>();

        //ボール削除後
        this.UpdateAsObservable()
            .Where(_ => gm.scorerenewal.Value)
            .ThrottleFirst(TimeSpan.FromSeconds(1))
            .Subscribe(_ => 
            {
                RoundNext();
                gm.scorerenewal.Value = false;
            });

        //ID切り替え
        this.UpdateAsObservable()
            .Where(_ => gm.roundNext.Value)
            .DelayFrame(1)
            .Subscribe(_ =>
            {
                NextID();
                gm.roundNext.Value = false;
            });

        //相手へのデバフスキルを元に戻す際の処理
        this.UpdateAsObservable()
            .Where(_ => gm.roundNext.Value && gm.skillfinish.Value)
            .DelayFrame(2)
            .Subscribe(_ => 
            {
                gm.skillfinish.Value = false;
                gm.nextturn.Value = true;
            });

        gm.outCount
            .Subscribe(count =>
            {
                countText.text = "アウトカウント: " + count;

                if (count == 3)
                {
                    RoundNext();
                    count = 0;
                }
            });

        gm._round
            .Subscribe(round => 
            {
                if (0 < round && round < 11)
                {
                    scoretext[round - 1].text = "" + gm.score[round - 1];
                    gm.outCount.Value = 0;
                }

                if(round <= 10)
                {
                    TotalScore();
                }

                //望遠カメラへ移行する
                if(round >= 10)
                {
                    gm.cameraswitch.Value = true;
                }
            })
            .AddTo(this);

        //ボールがエリアに入った際にスコアを取得する
        foreach (var n in area)
        {
            n.OnTriggerEnterAsObservable()
            .Where(trigger => trigger.gameObject.tag == "ball")
            .Subscribe(_ =>
            {
                if (n.gameObject.CompareTag("5ptArea"))
                {
                    scorehold = 5;
                }

                if (n.gameObject.CompareTag("10ptArea"))
                {
                    scorehold = 10;
                }

                if (n.gameObject.CompareTag("25ptArea"))
                {
                    scorehold = 25;
                }
                scoreNext = true;
            });
        }

    }

    //次のラウンドへの遷移処理
    void RoundNext()
    {
        GetScore(scorehold);
        scorehold = 0;
        gm.roundNext.Value = true;
        gm._round.Value++;
        scoreNext = false;
    }

    //ID切り替え
    void NextID()
    {
        psm.player_id.Value = (psm.player_id.Value == 0) ? 1 : 0;
        CountSwitchPlayer.OnNext(psm.player_id.Value);
        MeatSwitchPlayer.OnNext(psm.player_id.Value);
        AreaSwitchPlayer.OnNext(psm.player_id.Value);
    }

    //スコア取得
    void GetScore(int getscore)
    {
        for (int i = 0; i < 10; i++) 
        {
            if (gm._round.Value == i)
            {
                gm.score[i].Value += getscore;
            }
        }
    }
    //合計スコアを取得
    void TotalScore()
    {
        
        for(int i = 0; i < 5; i++)
        {
            if (gm._round.Value - 1 == i * 2)
            {
                P1score += gm.score[gm._round.Value - 1].Value;
            }

            if (gm._round.Value - 1 == i * 2 + 1)
            {
                P2score += gm.score[gm._round.Value - 1].Value;
            }
        }

        scoretext[10].text = "" + P1score;
        scoretext[11].text = "" + P2score;
      
    }
}
