using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnim:MonoBehaviour
{
    [SerializeField] private Image _circule1;
    [SerializeField] private Image _circule2;
    [SerializeField] private Image _circule3;
    [Space] [SerializeField] private float duration;
    [SerializeField] private Color transparent;

    private void Start()
    {
        ShowAnim();
    }

    private void ShowAnim()
    {
        var Seq = DOTween.Sequence();
        Seq.Append(_circule1.DOColor(Color.white, duration))
            .Append(_circule2.DOColor(Color.white, duration))
            .Append(_circule3.DOColor(Color.white, duration))
            .Append(_circule1.DOColor(transparent, duration))
            .Append(_circule2.DOColor(transparent, duration))
            .Append(_circule3.DOColor(transparent, duration))
            .OnComplete(ShowAnim);
    }
}
