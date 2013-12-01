using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public SpriteRenderer sprGood;
    public SpriteRenderer sprBad;
    public SpriteRenderer sprBack;

    public Color clrGood;
    public Color clrBad;

    public bool isGood { get; private set; }

    private bool canPickup = false;

    private void Start()
    {
        canPickup = false;

        if (Random.value < 0.5f)
            SetGood();
        else
            SetBad();

        StartCoroutine(Animate());

        sprBack.color = sprBack.color.GetTransparent(0.25f);
    }

    public void SetGood()
    {
        isGood = true;

        sprGood.color = clrGood;
        
        sprBad.enabled = false;
    }

    public void SetBad()
    {
        isGood = false;

        sprGood.color = clrBad;
        sprBad.color = clrBad;

        sprBad.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!canPickup)
            return;
            
        Player p = collider.GetComponent<Player>();

        if (p != null)
        {
            if (p.playerId % 2 == 0)
                Score.Instance.RedScore(isGood ? 1 : -1);
            else
                Score.Instance.BlueScore(isGood ? 1 : -1);
        }

        Kill();
    }

    private IEnumerator Animate()
    {
        Color endColor = isGood ? clrGood.GetTransparent() : clrBad.GetTransparent();

        sprGood.color = endColor;
        sprBad.color = endColor;

        GoTweenConfig config = new GoTweenConfig();

        config.scale(2f);
        config.setEaseType(GoEaseType.Punch);

        Go.to(transform, 0.3f, config);

        config.clearProperties();
        config.colorProp("color", endColor.GetOpaque());
        config.setEaseType(GoEaseType.QuadOut);

        Go.to(sprGood, 0.3f, config);
        Go.to(sprBad, 0.3f, config);

        yield return new WaitForSeconds(0.3f);

        canPickup = true;
        sprBack.color = sprBack.color.GetOpaque();

        yield return new WaitForSeconds(3f);

        canPickup = false;
        sprBack.color = sprBack.color.GetTransparent(0.25f);

        config.clearProperties();
        config.scale(0f);
        config.setEaseType(GoEaseType.BackIn);

        Go.to(transform, 0.3f, config);

        config.clearProperties();
        config.colorProp("color", endColor.GetTransparent());
        config.setEaseType(GoEaseType.QuadOut);

        Go.to(sprGood, 0.3f, config);
        Go.to(sprBad, 0.3f, config);

        yield return new WaitForSeconds(0.45f);

        Kill();
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}

