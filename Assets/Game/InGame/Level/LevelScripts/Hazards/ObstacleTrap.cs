using System.Collections;
using UnityEngine;

public class ObstacleTrap : MonoBehaviour
{
    [SerializeField]
    private float deathAnimationLength = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Guardian"))
        {
            StartCoroutine(WaitDeathAnimationFinish(other.gameObject));
        }
    }

    IEnumerator WaitDeathAnimationFinish(GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(deathAnimationLength);
        Destroy(objectToDestroy);
    }
}
