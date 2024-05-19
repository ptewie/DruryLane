using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardProperties : MonoBehaviour
{
    public HazardData hazardData;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (hazardData != null)
        {
            spriteRenderer.sprite = hazardData.hazardSprite;
            rb.velocity = Vector2.left * hazardData.speed;
        }
    }
}
