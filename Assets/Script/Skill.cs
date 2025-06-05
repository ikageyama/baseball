using R3;
using R3.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] PlayerStatusManager ps;
    [SerializeField] Pitcher pit;
    [SerializeField] GManager gm;

    //�X�L�����g�p���Ă��邩��bool�^
    [SerializeField] List<bool> skilluse = new List<bool>();
    [SerializeField] List<bool> finishskill = new List<bool>();

    [SerializeField] AudioSource decisionsource;
    [SerializeField] AudioClip decisionSE;

    //�o�b�^�[�̈ʒu
    [SerializeField] GameObject butter_trans;

    public List<GameObject> effect = new List<GameObject>();

    enum Skillname
    {
        flame,//���G�t�F�N�g��Z��1���E���h�̊ԃp���[����
        slow,//�s�b�`���[�̒e��x������o�t
        fast,//����̃^�[�����Ƀs�b�`���[�̒e�𑬂�����f�o�t
        power,//����̃^�[�����Ƀp���[�����̃f�o�t
    }

    void Start()
    {
        ps = FindAnyObjectByType<PlayerStatusManager>();
        gm = FindAnyObjectByType<GManager>();

        //�X�L���ΓZ���̔���
        this.UpdateAsObservable()
            .Where(_ => gm.skillbool[(int)Skillname.flame].Value && Input.GetKeyDown(KeyCode.Return) && ps.flame_count[ps.player_id.Value].Value >= 1 && !gm.magicuse)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                effect[(int)Skillname.flame] = Instantiate(ps.skilleffect[(int)Skillname.flame]);
                effect[(int)Skillname.flame].transform.position = butter_trans.transform.position;
                decisionsource.PlayOneShot(decisionSE);
                ps.flame_count[ps.player_id.Value].Value -= 1;
                skilluse[0] = true;
                gm.magicuse = true;
                ps._power[ps.player_id.Value].Value += 200;
                gm.skillbool[(int)Skillname.flame].Value = false;
            });

        //�X�L���X���E�{�[���̔���
        this.UpdateAsObservable()
            .Where(_ => gm.skillbool[(int)Skillname.slow].Value && Input.GetKeyDown(KeyCode.Return) && ps.bspeed_count[ps.player_id.Value].Value >= 1 && !gm.magicuse)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                effect[(int)Skillname.slow] = Instantiate(ps.skilleffect[(int)Skillname.slow]);
                effect[(int)Skillname.slow].transform.position = butter_trans.transform.position;
                decisionsource.PlayOneShot(decisionSE);
                ps.bspeed_count[ps.player_id.Value].Value -= 1;
                skilluse[1] = true;
                gm.magicuse = true;
                pit.sp.Value -= 0.08f;
                gm.skillbool[(int)Skillname.slow].Value = false;
            });

        //�X�L���t�@�X�g�{�[���̎g�p
        this.UpdateAsObservable()
            .Where(_ => gm.skillbool[(int)Skillname.fast].Value && Input.GetKeyDown(KeyCode.Return) && ps.dspeed_count[ps.player_id.Value].Value >= 1 && !gm.magicuse)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                decisionsource.PlayOneShot(decisionSE);
                ps.dspeed_count[ps.player_id.Value].Value -= 1;
                skilluse[2] = true;
                gm.magicuse = true;
                gm.skillbool[(int)Skillname.fast].Value = false;
            });

        //�X�L���p���[�z���̎g�p
        this.UpdateAsObservable()
            .Where(_ => gm.skillbool[(int)Skillname.power].Value && Input.GetKeyDown(KeyCode.Return) && ps.dpower_count[ps.player_id.Value].Value >= 1 && !gm.magicuse)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                decisionsource.PlayOneShot(decisionSE);
                ps.dpower_count[ps.player_id.Value].Value -= 1;
                skilluse[3] = true;
                gm.magicuse = true;
                gm.skillbool[(int)Skillname.power].Value = false;
            });

        //���̑���^�[���ɃX�L���t�@�X�g�{�[������
        this.UpdateAsObservable()
   �@�@�@�@ .Where(_ => skilluse[2] && gm.roundNext.Value)
    �@�@�@�@.Subscribe(_ =>
   �@�@�@�@ {
                 effect[(int)Skillname.fast] = Instantiate(ps.skilleffect[(int)Skillname.fast]);
                 effect[(int)Skillname.fast].transform.position = butter_trans.transform.position;
                 pit.sp.Value += 0.08f;
                 finishskill[0] = true;
                 skilluse[2] = false;
                 gm.skillfinish.Value = true;
            });

        //���̑���^�[���ɃX�L���p���[�z������
        this.UpdateAsObservable()
            .Where(_ => skilluse[3] && gm.roundNext.Value)
            .Subscribe(_ =>
            {
                effect[(int)Skillname.power] = Instantiate(ps.skilleffect[(int)Skillname.power]);
                effect[(int)Skillname.power].transform.position = butter_trans.transform.position;
                ps._power[(ps.player_id.Value == 0) ? 1 : 0].Value -= 150;
                finishskill[1] = true;
                skilluse[3] = false;
                gm.skillfinish.Value = true;
            });

        //�X�L���ΓZ���A�X���E�{�[�����I��
        this.UpdateAsObservable()
            .Where(_ => gm.roundNext.Value)
            .Subscribe(_ =>
            {
                if (skilluse[0])
                {
                    Destroy(effect[(int)Skillname.flame]);
                    ps._power[ps.player_id.Value].Value -= 200;
                    skilluse[0] = false;
                }
                if (skilluse[1])
                {
                    Destroy(effect[(int)Skillname.slow]);
                    pit.sp.Value += 0.08f;
                    skilluse[1] = false;
                }

                gm.magicuse = false;
            });

        //�X�L���t�@�X�g�{�[���A�p���[�z�����I��
        this.UpdateAsObservable()
            .Where(_ => gm.nextturn.Value)
            .Subscribe(_ =>
            {
                if (finishskill[0])
                {
                    Destroy(effect[(int)Skillname.fast]);
                    pit.sp.Value -= 0.08f;
                    finishskill[0] = false;
                }

                if (finishskill[1])
                {
                    Destroy(effect[(int)Skillname.power]);
                    ps._power[(ps.player_id.Value == 0) ? 1 : 0].Value += 150;
                    finishskill[1] = false;
                }

                gm.nextturn.Value = false;
            });
    }

}
