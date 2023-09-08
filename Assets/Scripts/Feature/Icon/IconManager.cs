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
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");

            // Check if the player is in the area collider
            if (IsPlayerInAreaCollider())
            {
                Debug.Log("Player is in the AreaCollider");

                if (combineCrateScript != null)
                {
                    CombineCrate.CubeState currentState = combineCrateScript.GetCurrentState();
                    Debug.Log("CombineCrate script is not null. Current state: " + currentState);

                    // Display icons based on the current state
                    TampilkanIkonBerdasarkanCurrentState(currentState);
                }
                else
                {
                    Debug.LogError("CombineCrate script is null. Ensure it is assigned in the inspector.");
                }
            }
            else
            {
                Debug.Log("Player is NOT in the AreaCollider");
            }
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
            areaColliderObject.GetComponent<BoxCollider>().size / 2f,
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
