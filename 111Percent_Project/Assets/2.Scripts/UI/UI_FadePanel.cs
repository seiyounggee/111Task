using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

//Todo.... DOTWEEN �÷����� �߰�
#if DoTween_Plugin_Included
using DG.Tweening;
#endif

public class UI_FadePanel : UIBase
{
    public enum FadeType { None, FadeIn, FadeOut } //Fade In-> ������� Fade Out -> ��ο�����

    [SerializeField] Image blackImg = null;

    private bool isFadedOut = false; //FadeOut�� ��������...
    private FadeType fadeType = FadeType.None;
    private float fadeTime = 2f;

    private bool isFadePlaying = false;

    private Action fadeCallback = null;

    internal override void OnEnable()
    {
        transform.SetAsLastSibling(); //Fade�� �׻� ���ʿ� ��ġ��Ű��
    }

    internal override void OnDisable()
    {
        transform.SetAsLastSibling(); //Fade�� �׻� ���ʿ� ��ġ��Ű��

        Clear();
    }

    public void Setup(FadeType type, float time = 1.5f, Action callback = null)
    {
        if (isFadePlaying == true)
            return;

        isFadePlaying = true;

        fadeType = type;
        fadeTime = time;

        fadeCallback = callback;

        switch (type)
        {
            case FadeType.FadeIn: //�������
                {
                    if (isFadedOut == false) //��ο����� ���� ���¿��� ������ ���... �̷��� �ȵǴµ�?
                    {
                        Debug.Log("<color=red>Error...! Currently Not Faded Out... </color>");
                        Clear();
                        gameObject.SafeSetActive(false);
                        return;
                    }
                    else
                    {
                        #if DoTween_Plugin_Included
                        blackImg.DOFade(1f, 0f); //��ο� ���¿��� ����
                        #endif
                    }
                }
                break;

            case FadeType.FadeOut: //��ο�����
                {
                    #if DoTween_Plugin_Included
                    blackImg.DOFade(0f, 0f); //���� ���¿��� ����
                    #endif
                    isFadedOut = true;
                }
                break;
        }
    }

    public override void Show()
    {
        base.Show();
        Activate();
    }

    private void Activate()
    {
        gameObject.SafeSetActive(true);

        switch (fadeType)
        {
            case FadeType.None:
                {
                    Clear();
                    gameObject.SafeSetActive(false);
                }
                break;

            case FadeType.FadeIn: //�������
                {
                    #if DoTween_Plugin_Included
                    blackImg.DOFade(0f, fadeTime).OnComplete(OnCompleteFadeIn);
                    #endif
                }
                break;

            case FadeType.FadeOut: //��ο�����
                {
                    #if DoTween_Plugin_Included
                    blackImg.DOFade(1f, fadeTime).OnComplete(OnCompleteFadeOut);
                    #endif
                }
                break;
        }

    }

    private void OnCompleteFadeOut()
    {
        fadeCallback?.Invoke();
        isFadePlaying = false;
    }

    private void OnCompleteFadeIn()
    {
        fadeCallback?.Invoke();
        isFadePlaying = false;
        Clear();
        gameObject.SafeSetActive(false);
    }

    private void Clear()
    {
        fadeCallback = null;
        isFadedOut = false;
        fadeType = FadeType.None;
    }
}