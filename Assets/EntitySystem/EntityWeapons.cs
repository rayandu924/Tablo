using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityWeapons : Entity
{
    public WeaponsData weaponsData;
    public override void Init()
    {
        GetComponent<MeshFilter>().mesh = weaponsData.model;
        GetComponent<MeshRenderer>().material = weaponsData.material;
    }
    public override void OnInteraction(GameObject player)
    {
        player.GetComponent<ClientPlayer>().weapons.Add(weaponsData);
        Destroy(gameObject);
    }
}
