using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class AddressablesManager : MonoBehaviour
{
    public void InstantiateAsset(AssetReference assetReference, Action<AsyncOperationHandle<GameObject>> action)
    {
        assetReference.InstantiateAsync().Completed += action;
    }
    
    public void ReleaseInstance(AssetReference assetReference, GameObject gameObject)
    {
        assetReference.ReleaseInstance(gameObject);
    }
    
    public void LoadAsset<T>(AssetReferenceT<T> assetReference, Action<AsyncOperationHandle<T>> action) where T : Object
    {
        assetReference.LoadAssetAsync<T>().Completed += action;
    }

    public void ReleaseAsset<T>(AssetReferenceT<T> assetReference) where T : Object
    {
        assetReference.ReleaseAsset();
    }
}