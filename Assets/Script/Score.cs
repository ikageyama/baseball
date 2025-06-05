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
    //���E���h���̃X�R�A�e�L�X�g
    public List<TextMeshProUGUI> scoretext = new List<TextMeshProUGUI>();

    //���_�G���A
    public List<GameObject> area = new List<GameObject>();

    //�A�E�g�J�E���g�̃e�L�X�g
    [SerializeField] TextMeshProUGUI countText;

    //�X�R�A�̕ێ�
    int scorehold;
    bool scoreNext;
    
    //�v���C���[�̍��v�X�R�A
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

        //�{�[���폜��
        this.UpdateAsObservable()
            .Where(_ => gm.scorerenewal.Value)
            .ThrottleFirst(TimeSpan.FromSeconds(1))
            .Subscribe(_ => 
            {
                RoundNext();
                gm.scorerenewal.Value = false;
            });

        //ID�؂�ւ�
        this.UpdateAsObservable()
            .Where(_ => gm.roundNext.Value)
            .DelayFrame(1)
            .Subscribe(_ =>
            {
                NextID();
                gm.roundNext.Value = false;
            });

        //����ւ̃f�o�t�X�L�������ɖ߂��ۂ̏���
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
                countText.text = "�A�E�g�J�E���g: " + count;

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

                //�]���J�����ֈڍs����
                if(round >= 10)
                {
                    gm.cameraswitch.Value = true;
                }
            })
            .AddTo(this);

        //�{�[�����G���A�ɓ������ۂɃX�R�A���擾����
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

    //���̃��E���h�ւ̑J�ڏ���
    void RoundNext()
    {
        GetScore(scorehold);
        scorehold = 0;
        gm.roundNext.Value = true;
        gm._round.Value++;
        scoreNext = false;
    }

    //ID�؂�ւ�
    void NextID()
    {
        psm.player_id.Value = (psm.player_id.Value == 0) ? 1 : 0;
        CountSwitchPlayer.OnNext(psm.player_id.Value);
        MeatSwitchPlayer.OnNext(psm.player_id.Value);
        AreaSwitchPlayer.OnNext(psm.player_id.Value);
    }

    //�X�R�A�擾
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
    //���v�X�R�A���擾
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
