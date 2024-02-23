using System.Collections;
using System.Collections.Generic;
using Managers;
using ScratchCardAsset;
using ScratchCardAsset.Animation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class IconBase : MonoBehaviour
{
    private ScratchCardManager cardManager;
    public Action<IconBase> onIconReveal;

    [Header("Feedbacks")]
    public bool playFeedbacks = true;
    public GameObject feedbackPrefab;
    public List<GameObject> particleList;
    public List<AudioClip> feedbackSoundList;
    public bool isAutoRevealed = false;
    public float autoRevealPressure = 3;
    
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
        cardManager = card.parent.GetComponentInChildren<ScratchCardManager>();
        cardSprite = card.GetComponent<SpriteRenderer>();
    }

    public void OnReveal()
    {
        if (isRevealed) return;
        StartCoroutine(Reveal());
        isRevealed = true;
    }

    IEnumerator Reveal()
    {
        yield return new WaitForSeconds(0.5f);
        
        // automatic scratch animation
        if(isAutoRevealed) cardManager.Card.ScratchHole(ConvertToScratchCardTexturePosition(), autoRevealPressure);

        if (playFeedbacks)
        {
            PlayFeedbackAnimation();
            if (feedbackSoundList.Count > 0)
            {
                GameManager.Instance.AudioManager.PlaySound(feedbackSoundList[UnityEngine.Random.Range(0, feedbackSoundList.Count)]);
            }
        }
        
        ApplyEffect();
        
        onIconReveal?.Invoke(this);
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
