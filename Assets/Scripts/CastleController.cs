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

    //public GameObject card;
    //private GameObject[] allCards;
    //public bool isCarringCard = false;
    
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

        //allCards = GameObject.FindGameObjectsWithTag("Card");
        //card = null;
    }

    private void Update()
    {
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!isCarringCard)
        {
            Physics.Raycast(ray, out hit, 100.0f);
            if (hit.collider.gameObject.CompareTag("Card") && Input.GetMouseButtonUp(0)) isCarringCard = !isCarringCard;
            card = hit.collider.gameObject;
        }
        else
        {
            Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("GameBoard"));
            Vector3 minimum;
            int position, layer;
            card.transform.position = (GetPosition(hit.point, out minimum, out layer, out position) + .3f * Vector3.up);
            card.transform.LookAt(transform.position);
            if (Input.GetMouseButtonUp(0))
            {
                CardController carriedCardControler = card.GetComponent<CardController>();
                if (carriedCardControler.availableLayers.Contains(layer))
                {
                    Debug.Log(layer.ToString());
                    isCarringCard = false;
                    card.transform.position = (minimum + .3f * Vector3.up);
                    card.transform.LookAt(transform.position);
                    carriedCardControler.layer = layer;
                    carriedCardControler.position = position;
                }
                
                GameObject toAttack = findAttackedCard(card, 2.0f);
                if (!toAttack.Equals(null))
                {
                    CardController otherController = toAttack.GetComponent<CardController>();
                    
                    carriedCardControler.Attack(otherController);
                    
                }
                card = null;
            }
        }*/
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

    Vector3 GetPosition(Vector3 linearPosition, out Vector3 minimum, out int layer, out int position)
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

    /*GameObject findAttackedCard(GameObject attacker, float distanceThreshold)
    {
        GameObject closestCard = null;

        for (int i = 0; i < allCards.Length; i++)
        {
            if (((allCards[i].transform.position - attacker.transform.position).magnitude < distanceThreshold)
                && !allCards[i].Equals(attacker))
            {
                closestCard = allCards[i];
                break;
            }
        }
        
        return closestCard;
    }*/
}
