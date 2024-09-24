using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableCubeLoader : MonoBehaviour
{
    // 用于存储加载的cube实例
    private GameObject loadedCube;

    // Addressable的key，用于标识资源
    public string cubeAddress = "cube";

    // 开始时加载cube
    void Start()
    {
        LoadCube();
    }

    // 加载cube的函数
    public void LoadCube()
    {
        // 调用Addressables来异步加载资源
        Addressables.LoadAssetAsync<GameObject>(cubeAddress).Completed += OnCubeLoaded;
    }

    // 当cube加载成功时调用
    private void OnCubeLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            // 实例化cube
            loadedCube = Instantiate(obj.Result);
            Rigidbody rigidbody = loadedCube.AddComponent<Rigidbody>();
            Destroy(rigidbody.gameObject,2);
            Debug.Log("Cube加载成功！");
        }
        else
        {
            Debug.LogError("Cube加载失败！");
        }
    }

    // 卸载cube的函数
    public void UnloadCube()
    {
        if (loadedCube != null)
        {
            Destroy(loadedCube); // 销毁实例化的GameObject
            Addressables.ReleaseInstance(loadedCube); // 释放资源
            Debug.Log("Cube已卸载！");
        }
    }

    // 重加载cube
    public void ReloadCube()
    {
        // 先卸载当前加载的cube
        UnloadCube();

        // 然后重新加载
        LoadCube();
    }

    // 测试：按下R键时重载cube
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCube();
        }
    }
}
