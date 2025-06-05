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

    //�X�L���̑I��
    public List<SerializableReactiveProperty<bool>> skillbool = new List<SerializableReactiveProperty<bool>>();

    //���E���h��
    public SerializableReactiveProperty<int> _round = new SerializableReactiveProperty<int>();

    //���̃��E���h�ւ̈ڍsbool
    public SerializableReactiveProperty<bool> roundNext = new SerializableReactiveProperty<bool>();

    //����^�[���̃X�L�����ʂ�߂��ۂ�bool
    public SerializableReactiveProperty<bool> nextturn = new SerializableReactiveProperty<bool>();
    public SerializableReactiveProperty<bool> skillfinish = new SerializableReactiveProperty<bool>();

    //�A�E�g�J�E���g�̕ϐ�
    public SerializableReactiveProperty<int> outCount = new SerializableReactiveProperty<int>();

    //�X�R�A�̍X�V
    public SerializableReactiveProperty<bool> scorerenewal = new SerializableReactiveProperty<bool>();

    //�Q�[���̏I������
    public SerializableReactiveProperty<bool> game_end = new SerializableReactiveProperty<bool>();

    //�{�[���̑��ݔ���
    public SerializableReactiveProperty<bool> balldeath = new SerializableReactiveProperty<bool>();

    //�J�����̐؂�ւ�
    public SerializableReactiveProperty<bool> cameraswitch = new SerializableReactiveProperty<bool>();

    //���@���g�p���Ă��邩�ǂ���
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

    //���̃X�N���v�g�i�N���X�j���j�������Ƃ��C���X�^���X���j������B
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }

    }
}
