using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public Recipe recipe;
    public Player player;


    //UI Windows
    public GameObject givenRecipeWindow;
    public GameObject ratingWindow;


    //UI Recipe
    public Image chef;
    public Text titleObject;
    public Text chefsDialogue;
    public Text ingredientsText;
    public Image monster1;
    public Image monster2;
    public Image dish;
    public GameObject star2;
    public GameObject star3;
    string amount;

    private void Start()
    {
        for (int i = 0; i < recipe.Ingredients.Length; i++)
        {
            recipe.Ingredients[i].currentAmount = 0;
            recipe.complete = false;
            recipe.ingredientsComplete = false;
            recipe.isActive = false;
            recipe.deaths = 0;
        }
    }

    public void InteractWithPlayer()
    {
        //Begin dialogs
        if (player.quest == null)
        {
            OpenSelectedRecipe();
        }

        if (player.quest != null && recipe.ingredientsComplete)
        {
            RecipeCompleted();
        }
    }


    public void OpenSelectedRecipe()
    {
        Time.timeScale = 0;
        givenRecipeWindow.SetActive(true);
        titleObject.text = recipe.title;
        chef.sprite = recipe.dialogue.chefsImage;
        monster1.sprite = recipe.monster1;
        monster2.sprite = recipe.monster2;
        if (recipe.monster2 == null)
            monster2.gameObject.SetActive(false);
        chefsDialogue.text = "";
        chefsDialogue.text = recipe.dialogue.initialDialogue;
        ingredientsText.text = "";
        for (int i = 0; i < recipe.Ingredients.Length; i++)
        {
            amount = recipe.Ingredients[i].amount.ToString();
            ingredientsText.text += amount + " " + recipe.Ingredients[i].type + "\n";
        }
    }
    
    public void AcceptRecipe()
    {
        Time.timeScale = 1f;
        givenRecipeWindow.SetActive(false);
        recipe.isActive = true;
        player.quest = recipe;
        player.DisplayRecipeStatus();
    }

    public void RecipeCompleted()
    {
        player.recipeStatusWindow.SetActive(false);
        ratingWindow.SetActive(true);
        dish.sprite = recipe.dish;
        if (recipe.deaths >= 1)
            star3.SetActive(false);
        if (recipe.deaths >= 2)
            star2.SetActive(false);

        Inventory.instance.Clear();
        recipe.isActive = false;
        recipe.complete = true;
        player.quest = null;
        player.level++;
    }
}
