// �޸ĺ�� AddressableLoader.cs
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using NewLoader;
using UnityEngine.UI;

namespace NewLoader
{
    public class AddressableLoader : MonoBehaviour
    {
        public string goName;
        public Button loadBut;
        private void Start()
        {
            loadBut.onClick.AddListener(() => {
                //reload json file,and load assets
                LoadFile();
            });
        }
        void LoadFile()
        {
            string serverUrl = ServerConfigLoader.LoadServerUrl();
            Addressables.InitializeAsync().Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    // �����Զ���URL
                    var catalogPath = $"{serverUrl}/TestAdd/StandaloneWindows64/catalog.json";
                    LoadContentCatalog(catalogPath);
                }
            };
        }

        void LoadContentCatalog(string catalogPath)
        {
            Addressables.LoadContentCatalogAsync(catalogPath, true).Completed += (catalogOp) =>
            {
                if (catalogOp.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("Catalog loaded successfully");
                    // ���ڿ��Լ�����Դ��
                    
                    LoadAddressableAsset();
                }
                else
                {
                    Debug.LogError($"Failed to load catalog: {catalogOp.OperationException}");
                }
            };
        }

        void LoadAddressableAsset()
        {
            // ����Addressable��Դ��ʾ��
            Addressables.LoadAssetAsync<GameObject>(goName).Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject loadedObject = op.Result;
                   GameObject tempGo =  Instantiate(loadedObject);
                    tempGo.AddComponent<Rigidbody>();
                    Destroy(tempGo,1);
                }
                else
                {
                    Debug.LogError($"Failed to load asset: {op.OperationException}");
                }
            };
        }
    }
}