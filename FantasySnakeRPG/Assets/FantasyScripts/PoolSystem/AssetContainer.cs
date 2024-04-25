using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetsContainerScriptableObject", menuName = "ScriptableObjects/AssetsContainerScriptableObject", order = 1)]
public class AssetContainer : ScriptableObject
{
   public GameObject GroundPrefab;
   public GameObject MonsterPrefab;
   public GameObject HeroPrefab;
}
