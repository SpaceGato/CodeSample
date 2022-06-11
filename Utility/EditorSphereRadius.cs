using UnityEngine;

public class EditorSphereRadius: MonoBehaviour
{
    public Transform firePoint;
    public float fpRadius;

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null)
            Gizmos.DrawWireSphere(firePoint.position, fpRadius);
    }
}
