using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class IconBase : MonoBehaviour
{
    [Header("Feedbacks")]
    public bool playFeedbacks = true;
    public GameObject feedbackPrefab;
    public List<AudioClip> feedbackSoundList;
    
    [Header("Icon")]
    public bool fullScratchToReveal = false;
    private bool isRevealed = false;
    
    protected virtual void Start()
    {
        isRevealed = false;
    }
    
    void Update()
    {
        
    }

    protected void OnReveal()
    {
        if (isRevealed) return;
        
        //TODO:automatic scratch animation
        
        if (playFeedbacks)
        {
            PlayFeedbackAnimation();
            if (feedbackSoundList.Count > 0)
            {
                AudioManager.instance.PlaySound(feedbackSoundList[Random.Range(0, feedbackSoundList.Count)]);
            }
        }
        
        ApplyEffect();
        isRevealed = true;
    }

    protected virtual void PlayFeedbackAnimation()
    {
        
    }

    protected virtual void ApplyEffect()
    {
        
    }
}
