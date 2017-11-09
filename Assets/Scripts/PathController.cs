using UnityEngine;
using System.Collections;

public class PathController : MonoBehaviour
{
    public GameObject[] Paths;

    private int currPath = 0;

    // Use this for initialization
    void Start()
    {
        foreach (var p in Paths) {
            p.SetActive(false);
        }
    }

    public void SetPath(int idx)
    {
        UIManager.instance.HideAll();
        Paths[currPath].SetActive(false);
        Paths[idx].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
