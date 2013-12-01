using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class Player : MonoBehaviour
{
    public Transform otherPlayer;
    public Transform tArrow;
    public Transform tEnergy;
    public SpriteRenderer sprArrow;
    public SpriteRenderer sprBase;
    public SpriteRenderer sprStop;

    public int playerId = 0;

    private Rigidbody2D _rigidbody;

    private float movePower = 1000f;

    private float energy;

    private float ENERGY_MAX = 2f;

    private bool exhausted = false;

    private Color clrBase;

    private void Start()
    {
        clrBase = sprBase.color;
        energy = ENERGY_MAX;
        _rigidbody = rigidbody2D;

        sprStop.color = sprStop.color.GetTransparent();
    }

    private void Update()
    {
        if (Score.Instance.isGameOver)
        {
            sprArrow.color = sprArrow.color.GetTransparent();
            enabled = false;
        }

        tEnergy.localScale = Vector3.one * Mathf.Clamp01(energy / ENERGY_MAX);
    }

    private void FixedUpdate()
    {
        Vector2 power = Vector2.zero;
        bool gas = false;
        bool brake = false;

        if (playerId == 0)
        {
            power = new Vector2(XCI.GetAxis(XboxAxis.LeftStickX, 1), XCI.GetAxis(XboxAxis.LeftStickY, 1));
            gas = XCI.GetAxis(XboxAxis.LeftTrigger, 1) > 0.2f;
            brake = XCI.GetButton(XboxButton.LeftBumper, 1);
        }
        else if (playerId == 1)
        {
            power = new Vector2(XCI.GetAxis(XboxAxis.RightStickX, 1), XCI.GetAxis(XboxAxis.RightStickY, 1));
            gas = XCI.GetAxis(XboxAxis.RightTrigger, 1) > 0.2f;
            brake = XCI.GetButton(XboxButton.RightBumper, 1);
        }
        else if (playerId == 2)
        {
            power = new Vector2(XCI.GetAxis(XboxAxis.LeftStickX, 2), XCI.GetAxis(XboxAxis.LeftStickY, 2));
            gas = XCI.GetAxis(XboxAxis.LeftTrigger, 2) > 0.2f;
            brake = XCI.GetButton(XboxButton.LeftBumper, 2);
        }
        else if (playerId == 3)
        {
            power = new Vector2(XCI.GetAxis(XboxAxis.RightStickX, 2), XCI.GetAxis(XboxAxis.RightStickY, 2));
            gas = XCI.GetAxis(XboxAxis.RightTrigger, 2) > 0.2f;
            brake = XCI.GetButton(XboxButton.RightBumper, 2);
        }

        // only allow if we actually have energy
        gas &= energy > 0f && !exhausted;
        brake &= energy > 0f && !exhausted;

        _rigidbody.isKinematic = brake;

        if (brake)
        {
            sprArrow.color = sprArrow.color.GetTransparent();
            sprStop.color = sprStop.color.GetTransparent(0.8f);
        }
        else
        {
            sprStop.color = sprStop.color.GetTransparent();

            if (power.sqrMagnitude == 0f)
            {
                sprArrow.color = sprArrow.color.GetTransparent();
            }
            else
            {
                if (gas)
                {
                    power *= 50f;
                }

                tArrow.right = power;
                sprArrow.color = sprArrow.color.GetTransparent(Mathf.Clamp01(power.sqrMagnitude));
            }

            _rigidbody.AddForce(power * movePower);
        }

        if (exhausted)
            _rigidbody.drag = 0f;
        else
            _rigidbody.drag = power.sqrMagnitude == 0 ? 10f : 0.25f;


        if (gas || brake)
        {
            energy = Mathf.Clamp(energy - Time.deltaTime, 0, ENERGY_MAX);

            if (energy == 0f)
            {
                exhausted = true;
                sprBase.color = Color.gray;
            }
        }
        else
        {
            energy = Mathf.Clamp(energy + Time.deltaTime, 0, ENERGY_MAX);

            if (exhausted && energy == ENERGY_MAX)
            {
                exhausted = false;
                sprBase.color = clrBase;
            }
        }
    }
}
