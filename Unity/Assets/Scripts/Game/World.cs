using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public Pickup prefabPickup;
    public Transform parentPickup;

    public GameObject parentPlayers34;

    private List<Pickup> activePickups;

    private void Start()
    {
        activePickups = new List<Pickup>();

        if (XboxCtrlrInput.XCI.GetNumPluggedCtrlrs() <= 1)
            parentPlayers34.SetActive(false);

        StartCoroutine(DropPickups());
    }

    private IEnumerator DropPickups()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.25f));

            if (Score.Instance.isGameOver)
                break;

            activePickups.RemoveAll(x => x == null);

            if (activePickups.Count < 3)
            {
                Vector2 pos;
                
                while (true)
                {
                    pos = new Vector2(Random.Range(-80, 80), Random.Range(-80, 80));

                    bool goodSpot = true;
                    for (int i = 0; i < activePickups.Count; ++i)
                    {
                        if (Vector3.Distance(pos, activePickups[i].transform.position) < 20f)
                        {
                            goodSpot = false;
                            break;
                        }
                    }

                    if (goodSpot)
                        break;

                    yield return null;
                }

                Pickup pickup = Instantiate(prefabPickup, pos, Quaternion.identity) as Pickup;
                pickup.transform.parent = parentPickup;

                activePickups.Add(pickup);
            }

            yield return null;
        }
    }

}
