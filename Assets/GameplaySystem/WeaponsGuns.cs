using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponsGuns : Weapons
{
    private void Update()
    {
        InputUpdate();
    }
    private void InputUpdate()
    {
        if (weaponsData.allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && weaponsData.magazine < weaponsData.magazineSize && !reloading) Reload();

        //Shoot
        if (readyToUse && shooting && !reloading && weaponsData.magazine > 0){
            base.Use();
        }
    }
    protected override void Fire()
    {
        for (int i = 0; i < weaponsData.bulletsPerTap; i++)
        {   
            GameObject bullet = new GameObject();
            BoxCollider boxCollider = bullet.AddComponent<BoxCollider>();
            Rigidbody rigidbody = bullet.AddComponent<Rigidbody>();
            MeshFilter meshFilter = bullet.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = bullet.AddComponent<MeshRenderer>();
            meshFilter.mesh = weaponsData.bulletModel;
            meshRenderer.material = weaponsData.material;
            bullet.transform.position = transform.parent.position + transform.parent.transform.forward;
            bullet.transform.rotation = transform.parent.rotation;
            rigidbody.AddForce((transform.parent.transform.forward + new Vector3(Random.Range(-weaponsData.spread, weaponsData.spread), Random.Range(-weaponsData.spread, weaponsData.spread), 0)) * weaponsData.speed, ForceMode.Impulse);
            Object.Destroy(bullet, 3.0f);
        }
        weaponsData.magazine--;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", weaponsData.reloadTime);
    }
    private void ReloadFinished()
    {
        weaponsData.magazine = weaponsData.magazineSize;
        reloading = false;
    }
}