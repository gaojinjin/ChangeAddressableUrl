using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
public class AddressableServerConfig : MonoBehaviour
{
    // 配置文件路径
    public string configFilePath = "serverConfig.json";
    public string cubeAddress = "cube";
    // 用于存储加载的cube实例
    private GameObject loadedCube;
    public Button createBut;

    void Start()
    {
        createBut.onClick.AddListener(() => {
            LoadServerConfig();
        });
        // 加载服务器配置
       
    }

    void LoadServerConfig()
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, configFilePath);
        string catalogUrl=string.Empty;
        Debug.Log(fullPath);    
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            ServerConfig config = JsonUtility.FromJson<ServerConfig>(json);
            Debug.Log(config.ServerAddress);
            // 动态设置服务器地址并加载新的Catalog
            catalogUrl = $"http://{config.ServerAddress}/TestAdd/StandaloneWindows64/catalog.json";
            LoadRemoteCatalog(catalogUrl);

            Debug.Log($"Addressables服务器地址已设置为: {config.ServerAddress}");
        }
        else
        {
            Debug.LogError("无法找到服务器配置文件，加载默认Catalog。");
            // 加载默认的Catalog
            LoadRemoteCatalog(catalogUrl);
        }
    }

    // 加载远程的Catalog文件
    void LoadRemoteCatalog(string remoteCatalogUrl)
    {
        Addressables.LoadContentCatalogAsync(remoteCatalogUrl).Completed += OnCatalogLoaded;
    }

    // Catalog加载完成后的回调
    private void OnCatalogLoaded(AsyncOperationHandle<IResourceLocator> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Catalog加载成功，资源可以开始加载");
            Debug.Log("开启资源加载");
            Addressables.LoadAssetAsync<GameObject>(cubeAddress).Completed += OnCubeLoaded;
        }
        else
        {
            Debug.LogError("Catalog加载失败");
        }
    }

    // 当cube加载成功时调用
    private void OnCubeLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            // 实例化cube
            loadedCube = Instantiate(obj.Result);
            loadedCube.AddComponent<Rigidbody>();
            Destroy(loadedCube, 1);
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
    }
}

// 用于存储服务器地址的类
[System.Serializable]
public class ServerConfig
{
    public string ServerAddress;
}
