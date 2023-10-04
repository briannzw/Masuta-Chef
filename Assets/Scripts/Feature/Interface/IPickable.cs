using UnityEngine;

public interface IPickable
{
    Transform Holder { get; } 

    float ThrowForce { get; }

    void StartPickup(GameObject picker);

    void ExitPickup();

    void Throw();
}
