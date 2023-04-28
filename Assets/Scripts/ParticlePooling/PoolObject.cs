using UnityEngine;

[System.Serializable]
public class PoolObject
{
    public VFXType VFXType;
    public Transform Prefab;
    public Pool Pool;
    public int Amount;
    public float Duration;
    public void InitPool(Transform root)
    {
        Pool = new Pool(true, Prefab, Amount, root);
    }
}
