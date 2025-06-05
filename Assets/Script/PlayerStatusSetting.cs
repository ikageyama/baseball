using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using R3;
using R3.Triggers;
using System;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.SocialPlatforms;

public class PlayerStatusSetting : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> levelText = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> paramaterText = new List<TextMeshProUGUI>();

    //�X�e�[�^�X���x���A�b�v�p�̃{�^��
    [SerializeField] List<Button> paraButton = new List<Button>();

    //�e�v���C���[�̃X�e�[�^�X�����\��
    public SerializableReactiveProperty<int>[] levelcount = new SerializableReactiveProperty<int>[2];

    //���x���\�񐔂�\������e�L�X�g
    [SerializeField] TextMeshProUGUI have_lcount;

    [SerializeField] PlayerStatusManager ps;
    [SerializeField] SettingSceneManager sm;

    private CompositeDisposable disposables = new();
   
    enum statusName
    {
        mt,// 0�@�~�[�g�͈�
        range,// 1�@�G���A�͈�
        power,// 2�@�p���[
    }
    
    //�X�e�[�^�X�̐ݒ�
    public void OnSwitchPlayer(int playerIndex)
    {
        disposables.Clear();

        levelcount[playerIndex]
           .Subscribe(count => have_lcount.text = "���x���A�b�v�\�� " + count)
           .AddTo(disposables);

        ps._meatlevel[playerIndex]
               .Subscribe(meatlevel =>
               {
                   levelText[(int)statusName.mt].text = "" + meatlevel;
                   paramaterText[(int)statusName.mt].text = "" + ps._meat[playerIndex].Value;
               })
               .AddTo(disposables);

        ps._rangelevel[playerIndex]
           .Subscribe(rangelevel =>
           {
               levelText[(int)statusName.range].text = "" + rangelevel;
               paramaterText[(int)statusName.range].text = "" + ps._range[playerIndex].Value;
           })
           .AddTo(disposables);

        ps._powerlevel[playerIndex]
            .Subscribe(powerlevel =>
            {
                levelText[(int)statusName.power].text = "" + powerlevel;
                paramaterText[(int)statusName.power].text = "" + ps._power[playerIndex].Value;
            })
            .AddTo(disposables);
    }

    void Start()
    {
        sm.OnSwitchPlayer
            .Subscribe(OnSwitchPlayer)
            .AddTo(this);

        OnSwitchPlayer(0);

        //�~�[�g�\�͂̃��x���A�b�v����
        paraButton[(int)statusName.mt].OnClickAsObservable()
            .Where(_ => levelcount[ps.player_id.Value].Value >= 1 && ps._meatlevel[ps.player_id.Value].Value < 5)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                sm.click_Source.PlayOneShot(sm.clickSE);
                levelcount[ps.player_id.Value].Value--;
                ps._meat[ps.player_id.Value].Value = ps.meatlist[ps._meatlevel[ps.player_id.Value].Value];
                ps._meatlevel[ps.player_id.Value].Value++;
            });

        //���_�͈͂̃��x���A�b�v����
        paraButton[(int)statusName.range].OnClickAsObservable()
            .Where(_ => levelcount[ps.player_id.Value].Value >= 1 && ps._rangelevel[ps.player_id.Value].Value < 5)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                sm.click_Source.PlayOneShot(sm.clickSE);
                levelcount[ps.player_id.Value].Value--;
                ps._range[ps.player_id.Value].Value = ps.rangelist[ps._rangelevel[ps.player_id.Value].Value];
                ps._rangelevel[ps.player_id.Value].Value++;
            });

        //�p���[�\�͂̃��x���A�b�v����
        paraButton[(int)statusName.power].OnClickAsObservable()
            .Where(_ => levelcount[ps.player_id.Value].Value >= 1 && ps._powerlevel[ps.player_id.Value].Value < 5)
            .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                sm.click_Source.PlayOneShot(sm.clickSE);
                levelcount[ps.player_id.Value].Value--;
                ps._power[ps.player_id.Value].Value = ps.powerlist[ps._powerlevel[ps.player_id.Value].Value];
                ps._powerlevel[ps.player_id.Value].Value++;
            });
    }

}
