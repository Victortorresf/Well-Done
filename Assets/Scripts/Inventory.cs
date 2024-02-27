using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("More then one instance of inventory found");
            return;
        }
        
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 12;

    public List<Ingredient> items = new List<Ingredient>();

    public bool Add (Ingredient item)
    {
        if (!item.isDefaultItem)
        {
            if(items.Count >= space)
            {
                Debug.Log("Not enough room in Inventory");
                return false;
            }

            //Debug.Log("Adicionou no inventario = " + item.name);
            items.Add(item);

            if(onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        return true;
    }

    public void Clear()
    {
        items.Clear();
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
