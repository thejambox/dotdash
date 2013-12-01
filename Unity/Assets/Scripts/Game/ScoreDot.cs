using UnityEngine;
using System.Collections;

public class ScoreDot : MonoBehaviour
{
    public Transform tCover;

    private bool showing = false;

    private void Start()
    {
        tCover.localScale = Vector2.one;
    }

    public void Show()
    {
        if (showing)
            return;

        GoTweenConfig config = new GoTweenConfig();

        config.scale(0.5f);
        config.setEaseType(GoEaseType.BackOut);

        Go.to(tCover, 0.3f, config);

        showing = true;
    }

    public void Hide()
    {
        if (!showing)
            return;

        GoTweenConfig config = new GoTweenConfig();

        config.scale(1f);
        config.setEaseType(GoEaseType.QuadOut);

        Go.to(tCover, 0.3f, config);

        showing = false;
    }
}
