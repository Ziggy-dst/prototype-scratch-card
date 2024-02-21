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
        //TODO:Icon Manager to tell the number; A UI to let choose type
    }
}
