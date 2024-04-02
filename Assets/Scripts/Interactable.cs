using System;
using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Ingredient item;
    public Player player;
    public bool stillThere = true;

    //Dissolve Effect
    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;
    float timer = 0;
    Renderer _renderer;
    int shaderProperty;
    bool collected = false;

    private void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _renderer = GetComponent<Renderer>();
    }
    public void Collect(Ingredient item)
    {
        if(item.type == Itemtype.Health)
        {
            GetHealth();
            StartCoroutine(SpawnObject());
        }
    }

    private void GetHealth()
    {
        Debug.Log("Health Increased");
        player.currentHealth += 50;
        player.healthBar.SetHealth(player.currentHealth);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        collected = true;
        stillThere = false;
    }

    IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(2);

        gameObject.GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(30);

        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        stillThere = true;
        collected = false;
    }

    void Update()
{
  
        if (collected && (timer < spawnEffectTime + pause))
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        if(_renderer != null)
            _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));
    }
    
}
