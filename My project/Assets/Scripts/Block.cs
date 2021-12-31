using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]Rigidbody blockRigidbody;
    [SerializeField]Renderer blockRenderer;
    [SerializeField]Color selected;
    [SerializeField]Color normal;
    // Start is called before the first frame update
    void Awake()
    {
        blockRenderer = GetComponent<Renderer>();
        blockRigidbody = GetComponent<Rigidbody>();
        blockRigidbody.mass = Random.Range(0.5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        blockRenderer.material.SetColor("_Color", selected);
    }

    void OnMouseExit()
    {
        blockRenderer.material.SetColor("_Color", normal);
    }
}
