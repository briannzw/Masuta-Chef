using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class IconManager : MonoBehaviour
{
    public GameObject iconItem;
    public GameObject iconMedkit;
    public GameObject iconWeapon;
    public GameObject iconCompanion;

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
        iconCompanion.SetActive(false);
    }
}
