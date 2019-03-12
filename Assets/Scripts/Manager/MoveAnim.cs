using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnim : MonoBehaviour
{
    [HideInInspector]
    public Transform startTarget;
    public GameObject MoveTarget;
    private void OnEnable()
    {
        transform.position = new Vector2(startTarget.position.x+0.3f ,startTarget.position.y-0.3f);
    }
    void Update()
    {
        if (!Arrived())
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), MoveTarget.transform.position, 5 * Time.deltaTime);
        }
        else
        {
            StartCoroutine(IEwait());
        }
    }
    public bool Arrived()
    {
        if (MoveTarget.transform.localPosition == this.transform.localPosition)
            return true;
        return false;
    }
    IEnumerator IEwait()
    {
        yield return new WaitForSeconds(0.7f);
        transform.position = new Vector2(startTarget.position.x + 0.3f, startTarget.position.y - 0.3f);
    }

}
