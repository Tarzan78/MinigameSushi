using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredient Pack List")]
public class IngredientPackList : ScriptableObject
{
    //NOTES: Need to implement the ids auto.
    public List<IngredientPack> ingredientPackList;

}
