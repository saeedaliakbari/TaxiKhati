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
        transform.position = new Vector2(startTarget.position.x + 0.3f, startTarget.position.y - 0.3f);
    }
    void Update()
    {
        if (!Arrived())
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), MoveTarget.transform.position, 5 * Time.deltaTime);
            Debug.Log(transform.position + "/ " + MoveTarget.transform.position);
        }
        else
        {
            Debug.Log("arrived");
            StartCoroutine(IEwait());
        }
    }
    public bool Arrived()
    {
        if (MoveTarget.transform.position.x == this.transform.position.x && MoveTarget.transform.position.y == this.transform.position.y)
            return true;
        return false;
    }
    IEnumerator IEwait()
    {
        yield return new WaitForSeconds(0.7f);
        transform.position = new Vector2(startTarget.position.x + 0.3f, startTarget.position.y - 0.3f);
        Debug.Log("Done Wait :" + transform.position);
    }

}
