using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EyeIcon : IconBase
{
    public Sprite eyeOpenSprite;
    
    void Update()
    {
        
    }
    
    protected override void PlayFeedbackAnimation()
    {
        spriteRenderer.sprite = eyeOpenSprite;
    }

    protected override void ApplyEffect()
    {
        GameManager.Instance.UIManager.ShowInfoPanel();
    }
}
