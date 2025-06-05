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

    //�X�e�[�^�X���x��
    public SerializableReactiveProperty<int>[] _meatlevel = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] _rangelevel = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] _powerlevel = new SerializableReactiveProperty<int>[2];
    
    //�X�e�[�^�X
    public SerializableReactiveProperty<float>[] _meat = new SerializableReactiveProperty<float>[2];
    public SerializableReactiveProperty<float>[] _range = new SerializableReactiveProperty<float>[2];
    public SerializableReactiveProperty<int>[] _power = new SerializableReactiveProperty<int>[2];

    //�e���x���̃X�e�[�^�X�l�Q�Ɨp
    public List<float> meatlist = new List<float>();
    public List<float> rangelist = new List<float>();
    public List<int> powerlist = new List<int>();

    //�X�L���J�E���g
    public SerializableReactiveProperty<int>[] flame_count = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] bspeed_count = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] dspeed_count = new SerializableReactiveProperty<int>[2];
    public SerializableReactiveProperty<int>[] dpower_count = new SerializableReactiveProperty<int>[2];

    //�e�X�L���̃G�t�F�N�g
    public List<GameObject> skilleffect = new List<GameObject>();

    //�v���C���[�̃f�[�^�؂�ւ��ϐ�
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
    //���̃X�N���v�g�i�N���X�j���j�������Ƃ��C���X�^���X���j������B
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }

    }

}
