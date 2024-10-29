using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorButton : MonoBehaviour
{
    private Interactor parentInteractor;
    private SpriteRenderer spriteRenderer;

    public void Start ()
    {
        parentInteractor = GetComponentInParent<Interactor>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update ()
    {
        spriteRenderer.enabled = parentInteractor.Visible;
    }

}