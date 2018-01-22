using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectLightmapDataLoaderTest : MonoBehaviour
{
    public string prefabPath;
    void Start()
    {
        GameObjectLightmapDataLoader gameObjectLightmapDataLoader = gameObject.AddComponent<GameObjectLightmapDataLoader>();
        gameObjectLightmapDataLoader.prefabPath = prefabPath;
    }
}

