
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialOfferObj : MonoBehaviour
{
    [HideInInspector]
    public SpecialOffer specialOffer;
    [HideInInspector]
    public bool giftBox;
    private int i;
    private float speed = 4.343149f;
    public void DiverARound()
    {
        i++;
        Debug.Log("i>>" + i);
        if (i < 5)
        {
            Hashtable hash = iTween.Hash("path", iTweenPath.GetPath("Road"), "orienttopath", true, "speed", speed, "easetype", iTween.EaseType.linear, "oncomplete", "CompleteCycle");
            iTween.MoveTo(gameObject, hash);
        }
        else
        {
            specialOffer.OfferNext();
        }
    }
    private void CompleteCycle()//وقتی کامل شد یک دور حرکت
    {
        DiverARound();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Special"))
                {
                    specialOffer.btnGem.interactable = true;
                    specialOffer.btnVideo.interactable = true;
                    specialOffer.panelOffer.SetActive(true);
                }
            }
        }
    }
}
