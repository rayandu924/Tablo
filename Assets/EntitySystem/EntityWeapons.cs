using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityWeapons : Entity
{
    public WeaponsData weaponsData;
    public override void Init()
    {
        GetComponent<MeshFilter>().mesh = weaponsData.weaponModel;
        GetComponent<MeshRenderer>().material = weaponsData.material;
    }
    public override void OnInteraction(GameObject player)
    {
        player.GetComponent<ClientPlayer>().WeaponsList.Add(weaponsData);
        Destroy(gameObject);
    }
}
