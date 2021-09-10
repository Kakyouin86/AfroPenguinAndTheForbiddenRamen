using UnityEngine;
using DG.Tweening;

public class GhostTrail : MonoBehaviour
{
    public Transform ghostsParent;
    public Color trailColor;
    public Color fadeColor;
    public float ghostInterval;
    public float fadeTime;

    private void Start()
    {

    }

    public void ShowGhost()
    {
        Sequence s = DOTween.Sequence();

        for (int i = 0; i < ghostsParent.childCount; i++)
        {
            Transform currentGhost = ghostsParent.GetChild(i);
            s.AppendCallback(()=> currentGhost.position = PlayerController.instance.transform.position);
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = PlayerController.instance.sr.flipX);
            s.AppendCallback(()=> currentGhost.GetComponent<SpriteRenderer>().sprite = PlayerController.instance.sr.sprite);
            s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0));
            s.AppendCallback(() => FadeSprite(currentGhost));
            s.AppendInterval(ghostInterval);
        }
    }

    public void FadeSprite(Transform current)
    {
        current.GetComponent<SpriteRenderer>().material.DOKill();
        current.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }

}
