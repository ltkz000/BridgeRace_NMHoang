using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize;
    [SerializeField] private bool expanable;
    private List<GameObject> freeList;
    private List<GameObject> usedList;

    private void Awake() 
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            GenerateNewObject();
        }
    }

    public GameObject GetObject(Vector3 position)
    {
        int totalFree = freeList.Count;
        if(totalFree == 0 && !expanable) return null;
        else if(totalFree == 0) GenerateNewObject();

        GameObject g = freeList[freeList.Count - 1];
        g.transform.position = position;
        g.SetActive(true);

        freeList.RemoveAt(freeList.Count - 1);
        usedList.Add(g);
        return g;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        usedList.Remove(obj);
        freeList.Add(obj);
    }

    private void GenerateNewObject()
    {
        GameObject g = Instantiate(prefab);
        g.transform.parent = transform;
        g.SetActive(false);
        freeList.Add(g);
    }


}
