using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class IconManager : MonoBehaviour
{
    public GameObject iconItem;
    public GameObject iconMedkit;
    public GameObject iconWeapon;
    public GameObject iconWeapon2;
    public GameObject iconWeapon3;
    public GameObject iconCompanion;
    public GameObject iconCompanion2;
    public GameObject iconCompanion3;

    private void Start()
    {
        // Panggil method ini saat awalnya
        MatikanSemuaIkon();
    }
    
    public void MatikanSemuaIkon()
    {
        // Matikan semua ikon
        iconItem.SetActive(false);
        iconMedkit.SetActive(false);
        iconWeapon.SetActive(false);
        iconWeapon2.SetActive(false);
        iconWeapon3.SetActive(false);
        iconCompanion.SetActive(false);
        iconCompanion2.SetActive(false);
        iconCompanion3.SetActive(false);
    }
}
