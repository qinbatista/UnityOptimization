using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchAndSetPassTestManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject cube;
    [SerializeField] GameObject cubeAnimation;
    [SerializeField] GameObject cubeBatchStatic;
    List<GameObject> GameObjectList = new List<GameObject>();
    void Start()
    {
        StartCoroutine(CreateInstance());
    }
    int angle;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit"))
        {
            foreach (var item in GameObjectList)
            {
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y + Mathf.Sin(angle++), item.transform.position.z);
            }
        }
    }
    IEnumerator CreateInstance()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        //random cube with random size
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                GameObject cubeInstance = Instantiate(cube, new Vector3(Random.Range(10f, -10f), Random.Range(10f, -10f), Random.Range(10f, -10f)), Quaternion.identity);
                cubeInstance.transform.localScale = new Vector3(Random.Range(1.5f, 0), Random.Range(1.5f, 0), Random.Range(1.5f, 0));
                GameObjectList.Add(cubeInstance);
                yield return null;
            }
        }
        Debug.Log("Create cube done");
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        foreach (var item in GameObjectList)
        {
            Destroy(item);
        }

        //random cube with random size with animation
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                GameObject cubeInstance = Instantiate(cubeAnimation, new Vector3(Random.Range(10f, -10f), Random.Range(10f, -10f), Random.Range(10f, -10f)), Quaternion.identity);
                cubeInstance.transform.localScale = new Vector3(Random.Range(1.5f, 0), Random.Range(1.5f, 0), Random.Range(1.5f, 0));
                GameObjectList.Add(cubeInstance);
                yield return null;
            }
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        foreach (var item in GameObjectList)
        {
            Destroy(item);
        }
        Debug.Log("Create cube with animation done");
        //random static cube with random size
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                GameObject cubeInstance = Instantiate(cubeBatchStatic, new Vector3(Random.Range(10f, -10f), Random.Range(10f, -10f), Random.Range(10f, -10f)), Quaternion.identity);
                cubeInstance.transform.localScale = new Vector3(Random.Range(1.5f, 0), Random.Range(1.5f, 0), Random.Range(1.5f, 0));
                GameObjectList.Add(cubeInstance);
                yield return new WaitForSeconds(0.001f);
            }
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        foreach (var item in GameObjectList)
        {
            Destroy(item);
        }
        //random cube with same size
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                GameObject cubeInstance = Instantiate(cube, new Vector3(Random.Range(10f, -10f), Random.Range(10f, -10f), Random.Range(10f, -10f)), Quaternion.identity);
                cubeInstance.transform.localScale = new Vector3(1, 1, 1);
                GameObjectList.Add(cubeInstance);
                yield return new WaitForSeconds(0.001f);
            }
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        foreach (var item in GameObjectList)
        {
            Destroy(item);
        }

        //random static cube with same size
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                GameObject cubeInstance = Instantiate(cubeBatchStatic, new Vector3(Random.Range(10f, -10f), Random.Range(10f, -10f), Random.Range(10f, -10f)), Quaternion.identity);
                cubeInstance.transform.localScale = new Vector3(1, 1, 1);
                GameObjectList.Add(cubeInstance);
                yield return new WaitForSeconds(0.001f);
            }
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        foreach (var item in GameObjectList)
        {
            Destroy(item);
        }
    }
}
