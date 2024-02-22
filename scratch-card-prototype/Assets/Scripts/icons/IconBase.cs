using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class IconBase : MonoBehaviour
{
    public IconRevealManager iconRevealManager;

    [Header("Feedbacks")]
    public bool playFeedbacks = true;
    public GameObject feedbackPrefab;
    public List<AudioClip> feedbackSoundList;
    
    [Header("Icon")]
    public bool fullScratchToReveal = false;
    [HideInInspector] public bool isRevealed = false;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        isRevealed = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnReveal()
    {
        if (isRevealed) return;
        
        // automatic scratch animation
        iconRevealManager.ConvertToScratchCardPosition(gameObject);
        
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
