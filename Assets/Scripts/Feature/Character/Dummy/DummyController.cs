using Character.Stat;
using Kryz.CharacterStats;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Character.Dummy
{
    public class DummyController : Character
    {
        [Header("References")]
        [SerializeField] private Material HitMaterial;
        private Material _material;

        [SerializeField] private TMP_Text Label;
        [SerializeField] private TMP_Text DPSLabel;

        [Header("Parameters")]
        public float DPSUpdateInterval = 0.5f;
        public float StatsUpdateInterval = 0.5f;
        public float ResetTimer = 5f;

        private float lastDamageDealt = 0f;
        private float elapsedTime = 0f;
        private float timer = 0f;

        private float damagePerSecond = 0f;
        private int hitCount = 0;

        public DummyController(StatsPreset preset) : base(preset) { Stats = preset.Stats; }

        private void Start()
        {
            SetLabel();
            SetDPSLabel();
            InvokeRepeating("SetLabel", 0, StatsUpdateInterval);
        }

        public override void TakeDamage(float totalAttack = 0, float multiplier = 1, StatModifier statMod = null)
        {
            base.TakeDamage(totalAttack, multiplier);
            CharacterDynamicStat healthStat = Stats.StatList[StatsEnum.Health] as CharacterDynamicStat;
            lastDamageDealt += (float)Math.Round(healthStat.CurrentValue - (totalAttack - Stats.StatList[StatsEnum.Defense].Value) * multiplier, 4);
            elapsedTime = 0f;
            hitCount++;
            SetLabel();
        }

        private void Update()
        {
            if (Mathf.Round(lastDamageDealt) == 0f) return;

            elapsedTime += Time.deltaTime;
            timer += Time.deltaTime;
            if(timer > DPSUpdateInterval)
            {
                timer = 0f;
                damagePerSecond = lastDamageDealt / elapsedTime;
                SetDPSLabel();
            }
            if (elapsedTime > ResetTimer)
            {
                lastDamageDealt = 0f;
                elapsedTime = 0f;
                hitCount = 0;
                damagePerSecond = 0f;
                SetDPSLabel();
            }
        }

        private void SetLabel()
        {
            Label.text = "";
            foreach(var keyValuePair in Stats.StatList)
            {
                if(keyValuePair.Value is CharacterDynamicStat)
                {
                    Label.text += keyValuePair.Key.ToString() + ": " + ((CharacterDynamicStat) keyValuePair.Value).CurrentValue.ToString("F2") + " / " + keyValuePair.Value.Value.ToString("F2") + "\n";
                    continue;
                }
                Label.text += keyValuePair.Key.ToString() + ": " + keyValuePair.Value.Value.ToString("F2") + "\n";
            }

        }

        private void SetDPSLabel()
        {
            DPSLabel.text = "";
            DPSLabel.text += "Hit Count: " + hitCount;
            DPSLabel.text += "\nDPS: " + damagePerSecond.ToString("F2");
        }
    }
}