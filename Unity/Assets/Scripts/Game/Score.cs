using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
    public ScoreDot[] redDots;
    public ScoreDot[] blueDots;

    public TextMesh winText;

    private int scoreRed;
    private int scoreBlue;

    private bool animateEnd = false;

    public static Score Instance = null;

    public bool isGameOver
    {
        get { return scoreRed >= 5 || scoreBlue >= 5; }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        scoreRed = 0;
        scoreBlue = 0;
    }

    private void Update()
    {
        if (isGameOver && !animateEnd)
        {
            StartCoroutine(RestartGame());

            animateEnd = true;
        }
    }

    public void RedScore(int delta)
    {
        if (isGameOver)
            return;

        if (delta < 0)
        {
            if (scoreRed > 0)
                redDots[scoreRed - 1].Hide();
        }
        else
        {
            if (redDots.Length > scoreRed)
                redDots[scoreRed].Show();
        }

        scoreRed = Mathf.Max(0, scoreRed + delta);
    }

    public void BlueScore(int delta)
    {
        if (isGameOver)
            return;

        if (delta < 0)
        {
            if (scoreBlue > 0)
                blueDots[scoreBlue - 1].Hide();
        }
        else
        {
            if (blueDots.Length > scoreBlue)
                blueDots[scoreBlue].Show();
        }

        scoreBlue = Mathf.Max(0, scoreBlue + delta);
    }

    private IEnumerator RestartGame()
    {
        float startTime = Time.time;

        winText.text = string.Format("{0} WINS.", scoreRed >= 5 ? "<color=#ff0000ff>RED</color>" : "<color=#0000ffff>BLUE</color>");
        
        GoTweenConfig config = new GoTweenConfig();
        config.localPosition(Vector2.zero);
        config.setEaseType(GoEaseType.BackOut);

        Go.to(winText.transform, 0.6f, config);

        while (true)
        {
            if (Time.time - startTime > 3f)
                break;

            if (XboxCtrlrInput.XCI.GetButton(XboxCtrlrInput.XboxButton.Back) || XboxCtrlrInput.XCI.GetButton(XboxCtrlrInput.XboxButton.Start))
                break;

            yield return null;
        }

        config.clearProperties();
        config.localPosition(Vector2.right * 175f);
        config.setEaseType(GoEaseType.BackIn);

        Go.to(winText.transform, 0.3f, config);

        yield return new WaitForSeconds(1f);

        Application.LoadLevel(Scenes.game);
    }
}
