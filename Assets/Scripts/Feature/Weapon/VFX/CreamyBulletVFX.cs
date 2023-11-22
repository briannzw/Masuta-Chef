using UnityEngine;

public class CreamyBulletVFX : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    private void OnEnable()
    {
        transform.localScale = Vector3.one * Random.Range(0.5f, 1f);
        transform.rotation = Random.rotation;
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
