using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float destroyTimer = 2f;
    private Vector3 offset = new Vector3(0f, 3f, 0f);

    private void Start()
    {
        Destroy(gameObject, destroyTimer);
        transform.position += offset;
    }
}
