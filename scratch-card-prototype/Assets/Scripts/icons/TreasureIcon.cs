using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TreasureIcon : IconBase
{
    public int quantityOfTreasure = 1;
    [SerializeField] private int numberOfCurseRevealedOnCurrentCard = 0;
    
    protected override void PlayFeedbackAnimation()
    {
        GameObject feedback = Instantiate(feedbackPrefab, transform.position, Quaternion.identity);
        TextMeshPro feedbackText = feedback.GetComponentInChildren<TextMeshPro>();
        Sequence feedbackSequence = DOTween.Sequence();
        
        feedbackText.text = "+ " + quantityOfTreasure + " Treasure"
                            + "\n" + "<color=green>- " + GetComponentInParent<IconManager>().revealedCurse + " Curse</color>"
                            + "\n" + "<color=yellow>+ " + GetComponentInParent<IconManager>().revealedCurse + " Gold</color>";
        
        feedbackSequence
            .Append(feedbackText.transform.DOScale(Vector3.zero, 0))
            .Append(feedbackText.transform.DOScale(Vector3.one, 0.5f))
            .Insert(0, feedbackText.transform.DOMoveY(transform.position.y + 1, 2f))
            .Insert(1, feedbackText.DOFade(0, 1f))
            .OnComplete((() => { Destroy(feedback); }));
        feedbackSequence.Play();
    }

    protected override void ApplyEffect()
    {
        GameManager.Instance.OnTreasureObtained(quantityOfTreasure);
        GameManager.Instance.RemoveCurse(GetComponentInParent<IconManager>().revealedCurse);
        GameManager.Instance.OnGoldObtained(-GetComponentInParent<IconManager>().revealedCurse);
    }
}
