using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CastleController : MonoBehaviour
{
    public int nrLayers = 3;
    public int HP = 20;
    public int[] layerLimits = {5, 5, 7};

    public float radialOffset = 1;
    public float radialCoefficient = 1;

    public List<List<GameObject>> layers = new List<List<GameObject>>();
    public GameObject updateModule1;
    public GameObject updateModule2;
    
    public int production = 3;
    private int level = 0;

    private PlayerController _playerController;
    private Collider[] castleElements;
    
    void Start()
    {
        for(int i = 0; i < nrLayers; i++)
        {
            //layerLimits[i] = 2 * (i+1) + 1;
            layers.Add(new List<GameObject>());
            for(int j = 0; j < layerLimits[i]; j++)
            {
                layers[i].Add(CreateRadialGO(i, j));
            }
        }

        castleElements = GetComponentsInChildren<Collider>();
        _playerController = GetComponent<PlayerController>();
        updateModule1.SetActive(false);
        updateModule2.SetActive(false);
    }

    GameObject CreateRadialGO(int layer, int position)
    {
        float angularPosition = (position + 1) / (float)(layerLimits[layer] + 1) * Mathf.PI - Mathf.Deg2Rad * transform.eulerAngles.y;
        Vector3 goTransform = radialCoefficient * new Vector3((layer + radialOffset) * Mathf.Cos(angularPosition), 0.0f, (radialOffset + layer) * Mathf.Sin(angularPosition)) + transform.position;
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.position = goTransform;
        cylinder.transform.localScale = new Vector3(.7f, .1f, .7f);
        return cylinder;
    }

    public Vector3 GetPosition(Vector3 linearPosition, out Vector3 minimum, out int layer, out int position)
    {
        float radious = (linearPosition - transform.position).magnitude;
        layer = Math.Min((int)((radious - radialOffset) / radialCoefficient), nrLayers-1);
        Vector3 min = layers[layer][0].transform.position;
        position = 0; // formally -1
        for (int i = 0; i < layers[layer].Count(); i++)
        {
            if ((linearPosition - layers[layer][i].transform.position).sqrMagnitude <
                (linearPosition - min).sqrMagnitude)
            {
                min = layers[layer][i].transform.position;
                position = i;
            }
        }

        minimum = min;
        return min + .3f * (linearPosition - min);
    }

    public void RoundlyCost()
    {
        _playerController.ReduceGold(-production);
    }

    public void UpdateCastle()
    {
        if (level == 0)
        {
            level = 1;
            updateModule1.SetActive(true);
            production *= 2;
        }
        else if (level == 1) 
        {
            updateModule2.SetActive(true);
            production *= 2;
            level = 2;
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if(HP <= 0) GameObject.Destroy(gameObject);
    }

    public bool hitCastle(Transform hit)
    {
        for (int i = 0; i < castleElements.Length; i++)
        {
            if (hit == castleElements[i].transform) return true;
        }

        return false;
    } 
}
