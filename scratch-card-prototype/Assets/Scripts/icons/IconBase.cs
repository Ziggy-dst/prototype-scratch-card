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
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    protected void OnReveal()
    {
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
    }

    protected virtual void PlayFeedbackAnimation()
    {
        
    }

    protected virtual void ApplyEffect()
    {
        
    }
}
