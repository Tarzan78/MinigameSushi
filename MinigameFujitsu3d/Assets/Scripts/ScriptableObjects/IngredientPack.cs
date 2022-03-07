using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredient Pack")]
public class IngredientPack : ScriptableObject
{
    public GameObject ingredientPrefab;
    public GameObject ingredientLeftSide;
    public GameObject ingredientRightSide;
    public int iD;
}
