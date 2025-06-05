using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using R3;
using R3.Triggers;
using System;
using UnityEngine.SceneManagement;

public class SettingSceneManager : MonoBehaviour
{
�@�@//True�ɂȂ��2P�̐ݒ�Ɉڂ�
    public bool next_player;

    [SerializeField] List<Button> scenebutton;

    [SerializeField] GameObject status_canvas;
    [SerializeField] GameObject skill_canvas;

    //2P�̐ݒ��ʂֈړ�
    [SerializeField] GameObject nextplayer_button;

    //�v���C��ʂֈړ�
    [SerializeField] GameObject main_button;

    public AudioSource click_Source;
    public AudioClip clickSE;

    [SerializeField] PlayerStatusManager psm;

    public Subject<int> OnSwitchPlayer = new();
    public Subject<int> SkillSwitchPlayer = new();
    enum SceneTransitions
    {
        next,
        nextplayer,
        main,
    };
    void Awake()
    {
        status_canvas.SetActive(true);
        skill_canvas.SetActive(false);
        main_button.SetActive(false);
    }

    void Start()
    {
        scenebutton[(int)SceneTransitions.next].OnClickAsObservable()
            .Subscribe(_ =>
            {
                status_canvas.SetActive(false);
                skill_canvas.SetActive(true);
                if (next_player)
                {
                    nextplayer_button.SetActive(false);
                    main_button.SetActive(true);
                }
            })
            .AddTo(this);

        //1P�ݒ��ʂ���2P�ݒ��ʂ�
        scenebutton[(int)SceneTransitions.nextplayer].OnClickAsObservable()
            .Subscribe(_ =>
            {
                status_canvas.SetActive(true);
                skill_canvas.SetActive(false);
                psm.player_id.Value++;
                OnSwitchPlayer.OnNext(psm.player_id.Value);
                SkillSwitchPlayer.OnNext(psm.player_id.Value);
                next_player = true;
            });

        //�v���C�V�[����
        scenebutton[(int)SceneTransitions.main].OnClickAsObservable()
            .Subscribe(_ =>
            {
                SceneManager.LoadScene("PlayScene");
            });
    }

}
