using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endings : MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string targetSceneName;

    [Tooltip("Time in seconds to wait before changing scenes")]
    public float delayTime = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.speed = 0f;
            }

            StartCoroutine(WaitAndLoadScene());
        }
    }

    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(targetSceneName);
    }
}
