using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject[] scenes;
    public float zSpawn = 0;
    public float sceneLength = 80;
    public Transform playerTransform;
    public Queue<Object> shownScenes = new Queue<Object>();
    public Queue<int> shownScenesIndex = new Queue<int>();
    private Object prevScene;

    // Update is called once per frame
    void Update()
    {
        if (prevScene == null &&  playerTransform.position.z > FindObjectOfType<GameManager>().lastWall.transform.position.z)
        {
            prevScene = shownScenes.Dequeue();
            shownScenesIndex.Dequeue();

            //Random.seed = System.DateTime.Now.Millisecond;
            Random.InitState(System.DateTime.Now.Millisecond);
            SpawnScene(Random.Range(3, scenes.Length));
        }

        if ((playerTransform.position.z - 20) > zSpawn - (3 * sceneLength))
        {
            Destroy(prevScene);
            prevScene = null;
        }
    }

    public Object GetCurrentScene()
    {
        return shownScenes.Peek();
    }

    public void SpawnScene(int index)
    {
        Object obj = Instantiate(scenes[index], transform.forward * zSpawn, transform.rotation);
        zSpawn += sceneLength;
        shownScenes.Enqueue(obj);
        shownScenesIndex.Enqueue(index);
    }

    public void RestartLevel()
    {
        zSpawn = 0;
        Destroy(shownScenes.Dequeue());
        Destroy(shownScenes.Dequeue());
        Destroy(shownScenes.Dequeue());
        Destroy(prevScene);
        prevScene = null;
        SpawnScene(shownScenesIndex.Dequeue());
        SpawnScene(shownScenesIndex.Dequeue());
        SpawnScene(shownScenesIndex.Dequeue());
    }
}
