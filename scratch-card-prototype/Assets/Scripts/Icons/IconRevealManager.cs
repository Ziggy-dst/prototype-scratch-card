using System.Collections.Generic;
using ScratchCardAsset;
using UnityEngine;
using ScratchCardAsset.Animation;

/// <summary>
/// attach to the scratch card sprite
/// </summary>
public class IconRevealManager : MonoBehaviour
{
    private SpriteRenderer cardSprite;
    private ScratchCard scratchCard;

    public ScratchAnimation scratchAnimation;
    public List<GameObject> icons;

    [Header("Reveal Setting")]
    public float brushScale = 2;
    public float time = 0.5f;

    void Start()
    {
        cardSprite = GetComponent<SpriteRenderer>();
        scratchCard = transform.parent.GetComponentInChildren<ScratchCard>();

        InitializeRevealAnimations();
    }

    private void InitializeRevealAnimations()
    {
        scratchAnimation.Scratches.Clear();
        foreach (var icon in icons)
        {
            var imageSize = Vector2.one;

            imageSize = scratchCard.ScratchData.TextureSize;

            var scratch = new BaseScratch
            {
                Position = icon.transform.position / imageSize,
                BrushScale = brushScale,
                Time = time
            };
            scratchAnimation.Scratches.Add(scratch);
        }

        // test revealing
        // foreach (var icon in icons)
        // {
        //     print(icon.transform.position);
        //     ConvertToScratchCardPosition(icon);
        // }
    }

    private int CheckAnimationIndex(GameObject icon)
    {
        return icons.IndexOf(icon);
    }

    /// <summary>
    /// triggered when a icon is revealed, figure out the right reveal animation and play
    /// </summary>
    /// <param name="iconTransform"></param>
    public void ConvertToScratchCardPosition(GameObject icon)
    {
        // card origin in world space
        Vector3 scratchCardOrigin = new Vector2(transform.position.x - cardSprite.sprite.bounds.size.x / 2,
            transform.position.y - cardSprite.sprite.bounds.size.y / 2);
        // print("origin " + scratchCardOrigin);

        Vector2 relativePos = icon.transform.position - scratchCardOrigin;
        // print("relative " + relativePos);

        // the new position in the card coordinate
        Vector2 convertedPosition = new Vector2(relativePos.x / cardSprite.sprite.bounds.size.x, relativePos.y / cardSprite.sprite.bounds.size.y);
        // print("finalPos " + convertedPosition);

        // check index of the icon on the list
        int iconIndex = CheckAnimationIndex(icon);
        print(iconIndex);

        scratchAnimation.Scratches[iconIndex].Position = convertedPosition;
    }
}
