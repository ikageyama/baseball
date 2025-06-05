using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using R3;
using R3.Triggers;
using System;
using UnityEngine.UI;
using System.Threading;

public class PlayerSkillSetting : MonoBehaviour
{
    //�X�L���̊e��
    [SerializeField] List<TextMeshProUGUI> pieceText = new List<TextMeshProUGUI>();

    [SerializeField] List<Button> skillButton = new List<Button>();

    //�X�L���|�C���g
    public SerializableReactiveProperty<int>[] skillpt = new SerializableReactiveProperty<int>[2];
    [SerializeField] TextMeshProUGUI have_SP;

    [SerializeField] PlayerStatusManager ps;
    [SerializeField] SettingSceneManager sm;

    private CompositeDisposable disposables = new();
    enum skillname
    {
        flame,//���G�t�F�N�g��Z��1���E���h�̊ԃp���[����
        slow,//�s�b�`���[�̒e��x������o�t
        fast,//����̃^�[�����Ƀs�b�`���[�̒e�𑬂�����f�o�t
        power,//����̃^�[�����Ƀp���[�����̃f�o�t
    }

    //�X�L���̌��ݒ�
    public void SkillSwitchPlayer(int playerIndex)
    {
        disposables.Clear();

        skillpt[playerIndex]
            .Subscribe(pt => have_SP.text = "�����Ă���SP  " + pt)
            .AddTo(disposables);

        ps.flame_count[playerIndex]
            .Subscribe(count => pieceText[(int)skillname.flame].text = "" + count)
            .AddTo(disposables);

        ps.bspeed_count[playerIndex]
            .Subscribe(count => pieceText[(int)skillname.slow].text = "" + count)
            .AddTo(disposables);

        ps.dspeed_count[playerIndex]
            .Subscribe(count => pieceText[(int)skillname.fast].text = "" + count)
            .AddTo(disposables);

        ps.dpower_count[playerIndex]
            .Subscribe(count => pieceText[(int)skillname.power].text = "" + count)
            .AddTo(disposables);
    }

    void Start()
    {
        sm.SkillSwitchPlayer
            .Subscribe(SkillSwitchPlayer)
            .AddTo(this);

        SkillSwitchPlayer(0);

        //�X�L����ʂ̃{�^������������
        skillButton[(int)skillname.flame].OnClickAsObservable()
            .Where(_ => skillpt[ps.player_id.Value].Value >= 3)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                sm.click_Source.PlayOneShot(sm.clickSE);
                ps.flame_count[ps.player_id.Value].Value++;
                skillpt[ps.player_id.Value].Value -= 3;
            });

        skillButton[(int)skillname.slow].OnClickAsObservable()
            .Where(_ => skillpt[ps.player_id.Value].Value >= 2)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                sm.click_Source.PlayOneShot(sm.clickSE);
                ps.bspeed_count[ps.player_id.Value].Value++;
                skillpt[ps.player_id.Value].Value -= 2;
            });

        skillButton[(int)skillname.fast].OnClickAsObservable()
            .Where(_ => skillpt[ps.player_id.Value].Value >= 3)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                sm.click_Source.PlayOneShot(sm.clickSE);
                ps.dspeed_count[ps.player_id.Value].Value++;
                skillpt[ps.player_id.Value].Value -= 3;
            });

        skillButton[(int)skillname.power].OnClickAsObservable()
            .Where(_ => skillpt[ps.player_id.Value].Value >= 4)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                sm.click_Source.PlayOneShot(sm.clickSE);
                ps.dpower_count[ps.player_id.Value].Value++;
                skillpt[ps.player_id.Value].Value -= 4;
            });
    }

}
