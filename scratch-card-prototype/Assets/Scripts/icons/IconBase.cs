using System.Collections;
using System.Collections.Generic;
using Managers;
using ScratchCardAsset;
using ScratchCardAsset.Animation;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class IconBase : MonoBehaviour
{
    private IconRevealManager iconRevealManager;
    private ScratchCardManager cardManager;

    [Header("Feedbacks")]
    public bool playFeedbacks = true;
    public GameObject feedbackPrefab;
    public List<AudioClip> feedbackSoundList;
    
    [Header("Icon")]
    public bool fullScratchToReveal = false;
    [HideInInspector] public bool isRevealed = false;

    protected SpriteRenderer spriteRenderer;
    protected SpriteRenderer cardSprite;

    protected virtual void Start()
    {
        isRevealed = false;
        spriteRenderer = GetComponent<SpriteRenderer>();

        Transform card = transform.parent.parent;
        iconRevealManager = card.GetComponent<IconRevealManager>();
        cardManager = card.parent.GetComponentInChildren<ScratchCardManager>();
        cardSprite = card.GetComponent<SpriteRenderer>();
    }

    public void OnReveal()
    {
        if (isRevealed) return;
        
        // automatic scratch animation
        cardManager.Card.ScratchHole(ConvertToScratchCardTexturePosition(), 2);

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

    public Vector2 ConvertToScratchCardTexturePosition()
    {
        Vector3 scratchCardOrigin = new Vector2(cardSprite.transform.position.x - cardSprite.sprite.bounds.size.x / 2,
            cardSprite.transform.position.y - cardSprite.sprite.bounds.size.y / 2);
        Vector2 relativePos = transform.position - scratchCardOrigin;
        Vector2 uvPosition = new Vector2(relativePos.x / cardSprite.sprite.bounds.size.x, relativePos.y / cardSprite.sprite.bounds.size.y);

        Vector2 convertedPosition = new Vector2(Mathf.FloorToInt(uvPosition.x * cardSprite.sprite.texture.width),
            Mathf.FloorToInt(uvPosition.y * cardSprite.sprite.texture.height));
        // print(convertedPosition);

        return convertedPosition;
    }

    protected virtual void PlayFeedbackAnimation()
    {
        
    }

    protected virtual void ApplyEffect()
    {
        
    }
}