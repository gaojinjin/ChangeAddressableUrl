using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
public class AddressableServerConfig : MonoBehaviour
{
    // �����ļ�·��
    public string configFilePath = "serverConfig.json";
    public string cubeAddress = "cube";
    // ���ڴ洢���ص�cubeʵ��
    private GameObject loadedCube;
    public Button createBut;

    void Start()
    {
        createBut.onClick.AddListener(() => {
            LoadServerConfig();
        });
        // ���ط���������
       
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
            // ��̬���÷�������ַ�������µ�Catalog
            catalogUrl = $"http://{config.ServerAddress}/TestAdd/StandaloneWindows64/catalog.json";
            LoadRemoteCatalog(catalogUrl);

            Debug.Log($"Addressables��������ַ������Ϊ: {config.ServerAddress}");
        }
        else
        {
            Debug.LogError("�޷��ҵ������������ļ�������Ĭ��Catalog��");
            // ����Ĭ�ϵ�Catalog
            LoadRemoteCatalog(catalogUrl);
        }
    }

    // ����Զ�̵�Catalog�ļ�
    void LoadRemoteCatalog(string remoteCatalogUrl)
    {
        Addressables.LoadContentCatalogAsync(remoteCatalogUrl).Completed += OnCatalogLoaded;
    }

    // Catalog������ɺ�Ļص�
    private void OnCatalogLoaded(AsyncOperationHandle<IResourceLocator> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Catalog���سɹ�����Դ���Կ�ʼ����");
            Debug.Log("������Դ����");
            Addressables.LoadAssetAsync<GameObject>(cubeAddress).Completed += OnCubeLoaded;
        }
        else
        {
            Debug.LogError("Catalog����ʧ��");
        }
    }

    // ��cube���سɹ�ʱ����
    private void OnCubeLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            // ʵ����cube
            loadedCube = Instantiate(obj.Result);
            loadedCube.AddComponent<Rigidbody>();
            Destroy(loadedCube, 1);
            Debug.Log("Cube���سɹ���");
        }
        else
        {
            Debug.LogError("Cube����ʧ�ܣ�");
        }
    }

    // ж��cube�ĺ���
    public void UnloadCube()
    {
        if (loadedCube != null)
        {
            Destroy(loadedCube); // ����ʵ������GameObject
            Addressables.ReleaseInstance(loadedCube); // �ͷ���Դ
            Debug.Log("Cube��ж�أ�");
        }
    }

    // �ؼ���cube
    public void ReloadCube()
    {
        // ��ж�ص�ǰ���ص�cube
        UnloadCube();

        // Ȼ�����¼���
    }
}

// ���ڴ洢��������ַ����
[System.Serializable]
public class ServerConfig
{
    public string ServerAddress;
}
