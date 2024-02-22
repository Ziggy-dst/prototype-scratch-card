using System.Collections;
using System.Collections.Generic;
using Managers;
using ScratchCardAsset.Animation;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class IconBase : MonoBehaviour
{
    private IconRevealManager iconRevealManager;

    [Header("Feedbacks")]
    public bool playFeedbacks = true;
    public GameObject feedbackPrefab;
    public List<AudioClip> feedbackSoundList;
    
    [Header("Icon")]
    public bool fullScratchToReveal = false;
    [HideInInspector] public bool isRevealed = false;

    protected SpriteRenderer spriteRenderer;
    private ScratchAnimator scratchAnimator;

    protected virtual void Start()
    {
        isRevealed = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        iconRevealManager = transform.parent.parent.GetComponent<IconRevealManager>();
        scratchAnimator = GetComponent<ScratchAnimator>();
    }

    public void OnReveal()
    {
        if (isRevealed) return;
        
        // automatic scratch animation
        iconRevealManager.ConvertToScratchCardPosition(gameObject);
        scratchAnimator.Play();
        
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
