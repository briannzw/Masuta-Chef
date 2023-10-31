using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Stat;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private GameObject meleeAttackPos;
    [SerializeField] private GameObject meleeAttackArea;
    public float AttackInterval = 2f;
    private Character.Character chara;
    private void Start()
    {
        chara = GetComponent<Character.Character>();
    }

    public void Attack()
    {
        Vector3 spawnPosition = meleeAttackPos.transform.position; // Set your desired spawn position here
        Quaternion spawnRotation = Quaternion.identity; // Set your desired spawn rotation here

        GameObject instantiatedObject = Instantiate(meleeAttackArea, spawnPosition, spawnRotation);
        if (instantiatedObject != null)
        {
            instantiatedObject.GetComponent<Character.Hit.HitController>().Value.CharacterAttack = chara.Stats.StatList[StatsEnum.Attack].Value;
        }
        
    }
}
