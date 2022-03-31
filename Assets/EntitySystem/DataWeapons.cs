using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataWeapons : MonoBehaviour
{
    public WeaponsData weaponsData;
    public void Initialize(WeaponsData _weaponsData)
    {
        /*bulletsLeft = magazineSize;
        readyToShoot = true;*/
        weaponsData.weaponModel = _weaponsData.weaponModel;
        weaponsData.material = _weaponsData.material;
        weaponsData.cam = _weaponsData.cam;
        weaponsData.damage = _weaponsData.damage;
        weaponsData.magazineSize = _weaponsData.magazineSize;
        weaponsData.bulletsPerTap = _weaponsData.bulletsPerTap;
        weaponsData.bulletsLeft = _weaponsData.bulletsLeft;
        weaponsData.spread = _weaponsData.spread;
        weaponsData.range = _weaponsData.range;
        weaponsData.reloadTime = _weaponsData.reloadTime;
        weaponsData.timeBetweenShots = _weaponsData.timeBetweenShots;
        weaponsData.force = _weaponsData.force;
        weaponsData.melee = _weaponsData.melee;
        weaponsData.allowButtonHold = _weaponsData.allowButtonHold;
        weaponsData.shooting = _weaponsData.shooting;
        weaponsData.readyToShoot = _weaponsData.readyToShoot;
        weaponsData.reloading = _weaponsData.reloading;
        weaponsData.text = _weaponsData.text;
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = weaponsData.weaponModel;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = weaponsData.material;
    }
    void Interact(GameObject player)
    {
        //player.GetComponent<ClientPlayer>().weaponsDatas.Add(new WeaponsData(weaponsData.weaponModel,weaponsData.material,player.GetComponent<Camera>(),10,10,3,10,0.1f,10,1,1,10,false,true,false,true,false,text));
    }
}
