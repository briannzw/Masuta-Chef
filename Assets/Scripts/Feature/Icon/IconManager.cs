using UnityEngine;

public class IconManager : MonoBehaviour
{
    public GameObject iconItem;
    public GameObject iconWeapon;
    public GameObject iconCompanion;
    public GameObject iconResep;

    private CombineCrate combineCrateScript;

    private void Start()
    {
        MatikanSemuaIkon();
        combineCrateScript = GetComponent<CombineCrate>();

        if (combineCrateScript == null)
        {
            Debug.LogError("CombineCrate script is not found on the player object.");
        }
    }

    private void Update()
    {
        if (IsPlayerInAreaCollider())
        {
            // Check if the player is in the area collider
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E key pressed");

                if (combineCrateScript != null)
                {
                    CombineCrate.CubeState? currentState = CheckCrateStateInAreaCollider();

                    if (currentState != null)
                    {
                        Debug.Log("CombineCrate script is not null. Current state: " + currentState.Value);

                        // Display icons based on the current state
                        TampilkanIkonBerdasarkanCurrentState(currentState.Value);
                    }
                    else
                    {
                        Debug.LogError("CombineCrate script is null or no 'crate' object found.");
                    }
                }
                else
                {
                    Debug.LogError("CombineCrate script is null. Ensure it is assigned in the inspector.");
                }
            }
            else { }
        }
        else
        {
            MatikanSemuaIkon();
        }
    }

    public void TampilkanIkonBerdasarkanCurrentState(CombineCrate.CubeState currentState)
    {
        // Tampilkan ikon berdasarkan currentState
        switch (currentState)
        {
            case CombineCrate.CubeState.Gray:
                iconItem.SetActive(true);
                break;
            case CombineCrate.CubeState.Red:
                iconWeapon.SetActive(true);
                break;
            case CombineCrate.CubeState.Blue:
                iconCompanion.SetActive(true);
                break;
            case CombineCrate.CubeState.Yellow:
                iconResep.SetActive(true);
                break;
            default:
                Debug.LogError("Current state tidak valid");
                break;
        }
    }

    private CombineCrate.CubeState? CheckCrateStateInAreaCollider()
    {
        // Cari semua objek dengan tag "crate" di dalam AreaCollider
        GameObject areaColliderObject = GameObject.Find("AreaCollider");

        if (areaColliderObject == null)
        {
            Debug.LogError("AreaCollider object with the specified name not found.");
            return null;
        }

        Collider[] crateColliders = Physics.OverlapBox(
            areaColliderObject.transform.position,
            areaColliderObject.GetComponent<BoxCollider>().size / 2, // Mengurangi ukuran area collider untuk memastikan objek 'crate' berada di dalamnya
            Quaternion.identity,
            LayerMask.GetMask("Crate") // Pastikan layer "Crate" adalah layer yang digunakan pada objek 'crate'
        );

        if (crateColliders.Length > 0)
        {
            // Ambil komponen CombineCrate dari objek pertama yang ditemukan
            CombineCrate crateScript = crateColliders[0].GetComponent<CombineCrate>();

            if (crateScript != null)
            {
                return crateScript.GetCurrentState();
            }
            else
            {
                Debug.LogError("CombineCrate script not found on crate object.");
                return null;
            }
        }
        else
        {
            Debug.Log("No 'crate' object found in the AreaCollider.");
            return null;
        }
    }

    private bool IsPlayerInAreaCollider()
    {
        // Cari objek AreaCollider berdasarkan namanya
        GameObject areaColliderObject = GameObject.Find("AreaCollider");

        if (areaColliderObject == null)
        {
            Debug.LogError("AreaCollider object with the specified name not found.");
            return false;
        }

        Collider[] colliders = Physics.OverlapBox(
            areaColliderObject.transform.position,
            areaColliderObject.GetComponent<BoxCollider>().size,
            Quaternion.identity,
            LayerMask.GetMask("Player")
        );

        return colliders.Length > 0;
    }

    private void MatikanSemuaIkon()
    {
        // Matikan semua ikon
        iconItem.SetActive(false);
        iconWeapon.SetActive(false);
        iconCompanion.SetActive(false);
        iconResep.SetActive(false);
    }
}
