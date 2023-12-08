using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private CinemachineBasicMultiChannelPerlin perlinNoise;
    private bool isShaking = false;

    [SerializeField] private Character.Character chara;

    [Header("Properties")]
    [SerializeField] private DynamicStatsEnum DynamicEnum = DynamicStatsEnum.Health;

    [Header("Effect When Damaged")]
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;
    [SerializeField] private Animator hurtEffect;
    void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        chara.OnDamaged += Shake;
        
    }

    private void Start()
    {
        ResetIntensity();
    }

    private void Update()
    {
        hurtEffect.SetBool("IsDying", chara.Stats.DynamicStatList[DynamicEnum].CurrentValue < (0.2f * chara.Stats.DynamicStatList[DynamicEnum].Value));
    }

    private void Shake()
    {
        hurtEffect.SetTrigger("Hurt");
        if (!isShaking)
        {
            ShakeCamera(shakeIntensity, shakeTime);
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        perlinNoise.m_AmplitudeGain = intensity;
        isShaking = true;
        StartCoroutine(WaitTime(time));
    }

    IEnumerator WaitTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ResetIntensity();
    }

    private void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0;
        isShaking = false;
    }
}
