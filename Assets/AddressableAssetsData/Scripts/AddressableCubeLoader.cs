using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableCubeLoader : MonoBehaviour
{
    // ���ڴ洢���ص�cubeʵ��
    private GameObject loadedCube;

    // Addressable��key�����ڱ�ʶ��Դ
    public string cubeAddress = "cube";

    // ��ʼʱ����cube
    void Start()
    {
        LoadCube();
    }

    // ����cube�ĺ���
    public void LoadCube()
    {
        // ����Addressables���첽������Դ
        Addressables.LoadAssetAsync<GameObject>(cubeAddress).Completed += OnCubeLoaded;
    }

    // ��cube���سɹ�ʱ����
    private void OnCubeLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            // ʵ����cube
            loadedCube = Instantiate(obj.Result);
            Rigidbody rigidbody = loadedCube.AddComponent<Rigidbody>();
            Destroy(rigidbody.gameObject,2);
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
        LoadCube();
    }

    // ���ԣ�����R��ʱ����cube
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCube();
        }
    }
}
