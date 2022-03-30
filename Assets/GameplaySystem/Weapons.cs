using UnityEngine;
using TMPro;


public class Weapons : MonoBehaviour
{
    //Gun stats
    public Mesh weaponModel, bulletModel;
    public Material material;
    public Camera cam;
    public TextMeshProUGUI text;
    public int damage, magazineSize, bulletsPerTap, bulletsLeft;
    public float spread, range, reloadTime, timeBetweenShots, force;
    public bool melee, allowButtonHold, shooting, readyToShoot, reloading;
    public void Initialize(Mesh weaponModel,Material material,Camera cam,int damage,int magazineSize,int bulletsPerTap,int bulletsLeft, float spread,float range,float reloadTime,float timeBetweenShots,float force,bool melee,bool allowButtonHold,bool shooting,bool readyToShoot,bool reloading, TextMeshProUGUI text)
    {
        /*bulletsLeft = magazineSize;
        readyToShoot = true;*/
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = weaponModel;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        this.weaponModel = weaponModel;
        this.material = material;
        this.cam = cam;
        this.damage = damage;
        this.magazineSize = magazineSize;
        this.bulletsPerTap = bulletsPerTap;
        this.bulletsLeft = bulletsLeft;
        this.spread = spread;
        this.range = range;
        this.reloadTime = reloadTime;
        this.timeBetweenShots = timeBetweenShots;
        this.force = force;
        this.melee = melee;
        this.allowButtonHold = allowButtonHold;
        this.shooting = shooting;
        this.readyToShoot = readyToShoot;
        this.reloading = reloading;
        this.text = text;
    }

    private void Update()
    {
        MyInput();
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0){
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;
        if(melee)
        {
            //raycast
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
            {
                Debug.Log(hit.collider.name);

                //if (rayHit.collider.CompareTag("Enemy"))
                    // degat
            }
        }
        else
        {
            //Spawn bullets
            for (int i = 0; i < bulletsPerTap; i++)
            {   
                GameObject bullet = new GameObject();
                bullet.transform.position = cam.transform.position + cam.transform.forward;
                bullet.transform.rotation = cam.transform.rotation;
                Rigidbody rigidbody = bullet.AddComponent<Rigidbody>();
                rigidbody.AddForce((cam.transform.forward + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0)) * force, ForceMode.Impulse);
                MeshFilter meshFilter = bullet.AddComponent<MeshFilter>();
                meshFilter.mesh = weaponModel;
                MeshRenderer meshRenderer = bullet.AddComponent<MeshRenderer>();
                meshRenderer.material = material;
                BoxCollider boxCollider = bullet.AddComponent<BoxCollider>();
                //component online
                Object.Destroy(bullet, 3.0f);
            }
            bulletsLeft--;
        }
        Invoke("ResetShot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
