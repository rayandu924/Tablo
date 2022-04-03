using UnityEngine;
using TMPro;


public abstract class Weapons : MonoBehaviour
{
    public WeaponsData weaponsData;
    public bool readyToUse, shooting, reloading;
    public void Init(WeaponsData weaponsData){
        this.weaponsData = weaponsData;
        transform.position = weaponsData.Player.transform.position;
        transform.parent = weaponsData.Player.transform;
        GetComponent<MeshFilter>().mesh = weaponsData.weaponModel;
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
        Weapon.transform.position = weaponsData.Player.transform.position;
        Destroy(gameObject);
    }
}
public class WeaponsData
{
    public GameObject Player;
    public string Name;
    public Mesh weaponModel, bulletModel;
    public Material material;
    public int typeOfWeapons,magazineSize, magazine, bulletsPerTap;
    public float damage, range, timeBetweenShots, spread, reloadTime, speed;
    public bool allowButtonHold;
    public WeaponsData(GameObject Player, string Name,Mesh weaponModel,Mesh bulletModel,Material material,int typeOfWeapons, int magazineSize,int magazine,int bulletsPerTap, float damage, float range, float timeBetweenShots, float spread, float reloadTime, float speed, bool allowButtonHold)
    {
        this.Player = Player;
        this.Name = Name;
        this.weaponModel = weaponModel;
        this.bulletModel = bulletModel;
        this.material = material;
        this.typeOfWeapons = typeOfWeapons;
        this.magazineSize = magazineSize;
        this.magazine = magazine;
        this.bulletsPerTap = bulletsPerTap;
        this.damage = damage;
        this.range = range;
        this.timeBetweenShots = timeBetweenShots;
        this.spread = spread;
        this.reloadTime = reloadTime;
        this.speed = speed;
        this.allowButtonHold = allowButtonHold;
    }
}