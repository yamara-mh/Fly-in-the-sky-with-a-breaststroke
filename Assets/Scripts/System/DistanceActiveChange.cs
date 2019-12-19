using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceActiveChange : MonoBehaviour
{
    [SerializeField]
    private bool nearTrue;
    [SerializeField]
    private float distance;
    [SerializeField]
    private GameObject _gameObject;

    void Start()
    {
        StartCoroutine("distanseCheck");
    }

    private IEnumerator distanseCheck()
    {
        yield return new WaitForSeconds(0.25f);

        while (true)
        {
            float dis = Vector3.Distance(transform.position, Manager.playerPos);
            _gameObject.SetActive(dis <= distance ? nearTrue : !nearTrue);
            yield return new WaitForSeconds(Mathf.Abs((dis - distance) * 0.01f));
        }
    }
}
