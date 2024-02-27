using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    
    public bool isActive;
    public string title;
    public Sprite monster1;
    public Sprite monster2;
    public Sprite dish;
    public bool ingredientsComplete = false;
    public bool complete = false;
    public int deaths;
    public Dialogues dialogue;
    public Ingredientsamount[] Ingredients;   
  
}

[System.Serializable]
 public class Ingredientsamount
 {
    public Itemtype type;
    public Sprite icon;
    public int amount;
    public int currentAmount = 0;
    
 }
 