using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DefultDemo : MonoBehaviour
{
    public string cubeAddress = "Cube";  // Addressable name of the Cube prefab

    void Start()
    {
        // Start the coroutine to load the cube prefab and instantiate it at intervals
        StartCoroutine(SpawnCubes());
    }

    IEnumerator SpawnCubes()
    {
        while (true)
        {
            // Load the cube prefab asynchronously
            var handle = Addressables.LoadAssetAsync<GameObject>(cubeAddress);
            yield return handle;

            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                // Instantiate the loaded cube prefab in the scene
                GameObject cube = Instantiate(handle.Result, Vector3.zero, Quaternion.identity);

                // Add a Rigidbody component to the cube
                Rigidbody rb = cube.AddComponent<Rigidbody>();

                // Start coroutine to destroy the cube after 0.8 seconds
                StartCoroutine(DestroyCubeAfterTime(cube, 0.8f));
            }
            else
            {
                Debug.LogError("Failed to load Cube prefab.");
            }

            // Wait for 1 second before spawning the next cube
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator DestroyCubeAfterTime(GameObject cube, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy the cube
        Destroy(cube);
    }
}
