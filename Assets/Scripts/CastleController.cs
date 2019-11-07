using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    public int nrLayers = 3;
    public int[] layerLimits;

    public float radialOffset = 1;
    public float radialCoefficient = 1;

    public List<List<GameObject>> layers = new List<List<GameObject>>();
    public GameObject updateModule;
    
    public int production = 3;
    private int level = 0;

    private PlayerController _playerController;
    
    void Start()
    {
        layerLimits = new int[nrLayers];
        for(int i = 0; i < nrLayers; i++)
        {
            layerLimits[i] = 2 * (i+1) + 1;
            layers.Add(new List<GameObject>());
            for(int j = 0; j < layerLimits[i]; j++)
            {
                layers[i].Add(CreateRadialGO(i, j));
            }
        }

        _playerController = GetComponent<PlayerController>();
        updateModule.SetActive(false);
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
        position = -1;
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
            updateModule.SetActive(true);
            production *= 2;
        }
    }
}
