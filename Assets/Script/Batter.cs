using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using R3.Triggers;
using System;

public class Batter : MonoBehaviour
{
    [SerializeField] GameObject decision;
    [SerializeField] Animator anim;
    [SerializeField] CapsuleCollider col;
    
    public AudioSource audiosource;
    public AudioSource catcher_audio;
    public AudioClip hitSE;
    public AudioClip catchSE;

    //�o�b�g�̃A�j���[�V�������I��鎞
    bool swingfinish;

    public SerializableReactiveProperty<float> hit_angle = new SerializableReactiveProperty<float>();

    public SerializableReactiveProperty<bool> angle_finish = new SerializableReactiveProperty<bool>();

    void Start()
    {
        //�o�b�g��U��
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .ThrottleFirst(TimeSpan.FromSeconds(2))
            .Subscribe(_ => Swing());

        //�U��I�������Ƀ|�W�V���������ɖ߂�
        this.UpdateAsObservable()
            .Where(_ => swingfinish)
            .Delay(TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ =>
            {
                this.gameObject.transform.position = new Vector3(-13, 0.05f, 14.5f);
                this.gameObject.transform.eulerAngles = new Vector3(0, 142, 0);
                swingfinish = false;
            });
    }

    void Swing()
    {
        anim.SetBool("swing", true);
    }

    //�����蔻���True�ɂ���
    void SwingDecisionTrue()
    {
        angle_finish.Value = true;
        col.enabled = true;
    }

    //�����蔻���False�ɂ���
    void SwingDecisionFalse()
    {
        angle_finish.Value = false;
        col.enabled = false;
    }

    void SwingFinish()
    {
        anim.SetBool("swing", false);
        swingfinish = true;
    }
}
