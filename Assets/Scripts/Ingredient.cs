using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    new public string name = "New Ingredient";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public Itemtype type;
}

public enum Itemtype
{
    Health,
    Worms,
    Eyeballs,
    BloodVial,
    Eggs,
    Berries,
    Wheat,
    Honey,
    Ribs,
    Potatoes,
    Beetles,
    Saliva,
    Mushrooms,
    Hooves,
    Tentacles,
    Coral,
    FishEggs,
    Nuri
}
