using UnityEngine;
using TMPro;


public abstract class Weapons : MonoBehaviour
{
    public WeaponsData weaponsData;
    public Camera cam;
    public bool readyToUse, shooting, reloading;
    public void Init(WeaponsData weaponsData){
        this.weaponsData = weaponsData;
        cam = Object.FindObjectOfType<Camera>();
        GetComponent<MeshFilter>().mesh = weaponsData.model;
        GetComponent<MeshRenderer>().material = weaponsData.material;
        readyToUse = true;
        shooting = false;
        reloading = false;
    }

    protected void Use()
    {
        readyToUse = false;
        Fire();
        Invoke("ResetUse", weaponsData.timeBetweenShots);
    }

    protected abstract void Fire();
    protected void ResetUse()
    {
        readyToUse = true;
    }

    public void ToEntity()
    {
        GameObject Weapon = new GameObject();
        EntityWeapons entityWeapons = Weapon.AddComponent<EntityWeapons>();
        entityWeapons.weaponsData = weaponsData;
        Weapon.transform.position = gameObject.transform.position;
        Weapon.transform.parent = null;
        Destroy(gameObject);
    }
}