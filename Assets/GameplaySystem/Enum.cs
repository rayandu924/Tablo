using UnityEngine;

public enum PlayerStateBottom
{
    Idle,
    Walk,
    Run,
    ReverseWalk,
}
public enum PlayerStateTop
{
    IdleTop,
    Fire,
    Reload,
    Drop
}

public enum WeaponType
{
    Melee,
    Guns,
}

public struct WeaponsData
{
    public WeaponType weaponType;
    public string name;
    public Mesh model, bulletModel;
    public Material material;
    public int magazine,magazineSize,bulletsPerTap;
    public float damage, timeBetweenShots, range, spread, reloadTime, speed;
    public bool allowButtonHold;
    public Vector3 position, rotation;
    public PlayerStateTop playerStateTop;
    public WeaponsData(WeaponType weaponType, string name,Mesh model,Material material,Mesh bulletModel,int magazine,int magazineSize,int bulletsPerTap,float damage,float timeBetweenShots,float reloadTime,float range,float spread,float speed,bool allowButtonHold, Vector3 position, Vector3 rotation,PlayerStateTop playerStateTop)
    {
        this.weaponType = weaponType;
        this.name = name;
        this.model = model;
        this.bulletModel = bulletModel;
        this.material = material;
        this.magazine = magazine;
        this.magazineSize = magazineSize;
        this.bulletsPerTap = bulletsPerTap;
        this.damage = damage;
        this.timeBetweenShots = timeBetweenShots;
        this.reloadTime = reloadTime;
        this.range = range;
        this.spread = spread;
        this.speed = speed;
        this.allowButtonHold = allowButtonHold;
        this.position = position;
        this.rotation = rotation;
        this.playerStateTop = playerStateTop;
    }
}