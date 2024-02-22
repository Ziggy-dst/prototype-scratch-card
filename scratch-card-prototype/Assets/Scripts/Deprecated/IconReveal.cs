using System.Collections;
using System.Collections.Generic;
using ScratchCardAsset.Animation;
using UnityEngine;

public class IconReveal : MonoBehaviour
{
    private SpriteRenderer scratchCard;
    public ScratchAnimation scratchAnimation;

    void Start()
    {
        scratchCard = transform.parent.parent.GetComponent<SpriteRenderer>();
        ConvertToScratchCardPosition();
    }

    private void ConvertToScratchCardPosition()
    {
        // card origin in world space
        Vector3 scratchCardOrigin = new Vector2(scratchCard.transform.position.x - scratchCard.sprite.bounds.size.x / 2,
            scratchCard.transform.position.y - scratchCard.sprite.bounds.size.y / 2);
        Vector2 relativePos = transform.position - scratchCardOrigin;
        Vector2 convertedPosition = new Vector2(relativePos.x / scratchCard.sprite.bounds.size.x, relativePos.y / scratchCard.sprite.bounds.size.y);
        scratchAnimation.Scratches[0].Position = convertedPosition;
    }
}
