using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitVFX : MonoBehaviour
{
    private Character.Character chara;
    public SkinnedMeshRenderer CharaRenderer;

    private Color baseColor;
    [SerializeField] private Color hitColor;
    private float emission;

    private void Start()
    {
        baseColor = CharaRenderer.material.GetColor("_EmissionColor");
        chara = GetComponent<Character.Character>();
        chara.OnDamaged += HitEffect;
    }

    private void Update()
    {
        // No need to update emission in Update if you're using it only for HitEffect
    }

    public void HitEffect()
    {
        StartCoroutine(BlinkEffect());
    }

    private IEnumerator BlinkEffect()
    {
        // Set emission to white
        CharaRenderer.material.SetColor("_EmissionColor", hitColor);

        // Wait for a short duration
        yield return new WaitForSeconds(0.2f);

        // Set emission back to the original color
        emission = Mathf.PingPong(Time.time, 0.5f);
        CharaRenderer.material.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(emission));
    }
}
