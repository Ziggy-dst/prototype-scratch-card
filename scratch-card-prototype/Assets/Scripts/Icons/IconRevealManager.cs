using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScratchCardAsset.Animation;

/// <summary>
/// Attach to each scratch card
/// </summary>
public class IconRevealManager : MonoBehaviour
{
    private int iconCount = 0;
    public ScratchAnimation scratchAnimation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeRevealAnimations()
    {
        for (int i = 0; i < iconCount; i++)
        {
            // scratchAnimation.Scratches[0];
        }
    }
}
