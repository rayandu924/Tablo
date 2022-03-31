using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientPlayer : MonoBehaviour
{
    public int moveForce;
    public int pickUpRange;
    public List<string> GhostComponent = new List<string>();
    public float throwForce;
    public Mesh mesh;
    GameObject heldObject;
    public GameObject currentWeapon;
    public TextMeshProUGUI text;
    public List<WeaponsData> weaponsDatas = new List<WeaponsData>();
    public int weaponsSelected;
    public float temp;
    
    private void Start() {
        weaponsDatas.Add(new WeaponsData(mesh,Resources.Load<Material>("Materials/0"),gameObject.GetComponent<Camera>(),10,10,3,10,0.1f,10,1,1,10,false,true,false,true,false,text));
        weaponsDatas.Add(new WeaponsData(mesh,Resources.Load<Material>("Materials/0"),gameObject.GetComponent<Camera>(),10,10,3,10,0.1f,10,1,1,10,false,true,false,true,false,text));
        weaponsDatas.Add(new WeaponsData(mesh,Resources.Load<Material>("Materials/0"),gameObject.GetComponent<Camera>(),10,10,3,10,0.1f,10,1,1,10,false,true,false,true,false,text));
    }
    private void Update() {
        ObjectUpdate();
        WeaponsSelectorUpdate();
    }

    void ObjectUpdate()
    {
        if (heldObject == null)
        {
            if (Input.GetKeyDown(KeyCode. E))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    if(hit.transform.gameObject.GetComponent<DataWeapons>())// objet prenable ?
                    {
                        
                    }
                    else if(hit.transform.gameObject.GetComponent<Rigidbody>())
                    {
                        PickupObject(hit.transform.gameObject);
                        CreateGhostObject();
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode. E))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange) && hit.transform.gameObject.tag == "Grid")
                {
                    PlaceObject(hit);//ammeliorer pour rendre l'objet fixe et voir si oui ou non on met 1 objet sur chaque grid
                }
                else
                {
                    DropObject();
                }
            }
            else if(Input.GetKeyDown(KeyCode. W))
            {
                ThrowObject();
            }
            else
            {
                MoveObject();
            }
        }

        if (Input.GetKeyDown(KeyCode. G))
        {
            DropItem();
        }
    }

    void PickupObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.useGravity = false;
        obj.layer = 2;
        heldObject = obj;
    }

    void MoveObject(){
        if(Vector3.Distance(heldObject.transform.position, transform.parent.position)> 0.1f)
        {
            Vector3 moveDirection = (transform.parent.position - heldObject.transform.position + transform.forward * temp);
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        heldObject.layer = 0;
        heldObject = null;
    }

    void DropItem()
    {
        GameObject entity = new GameObject();
        entity.AddComponent<DataWeapons>().Initialize(weaponsDatas[weaponsSelected]);
        weaponsDatas.RemoveAt(weaponsSelected);
    }

    void GetItem(GameObject entity)//ici qu'on va faire en sorte de trier les item recuperable et quest ce qui recupere
    {
        WeaponsData dataWeapons = entity.GetComponent<DataWeapons>().weaponsData;
        weaponsDatas.Add(new WeaponsData(dataWeapons.weaponModel,dataWeapons.material,dataWeapons.cam,10,10,3,10,0.1f,10,1,1,10,false,true,false,true,false,text));
        Destroy(entity);
    }

    void PlaceObject(RaycastHit hit)
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.transform.localPosition = hit.point;
        heldObject.layer = 0;
        heldObject = null;
    }

    void CreateGhostObject()
    {
        GameObject ghostObject = Instantiate(heldObject, heldObject.transform.position, heldObject.transform.rotation);
        foreach (var component in ghostObject.GetComponents<Component>())
            if(!GhostComponent.Contains(component.GetType().Name))
                Destroy(component);
        StartCoroutine("UpdateGhostObject", ghostObject);
    }

    void ThrowObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        heldObject.layer = 0;
        heldObject = null;
        rb.AddForce(transform.forward * throwForce,ForceMode.Impulse);
    }
    IEnumerator UpdateGhostObject(GameObject ghostObject)
    {     
        while(heldObject != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
            {
                ghostObject.transform.position = hit.point;
            }
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(ghostObject);
    }

    void CreateWeapon(int i)
    {
        Destroy(currentWeapon);
        GameObject weapon = new GameObject();
        weapon.AddComponent<Weapons>().Initialize(weaponsDatas[i].weaponModel, weaponsDatas[i].material, weaponsDatas[i].cam, weaponsDatas[i].damage, weaponsDatas[i].magazineSize, weaponsDatas[i].bulletsPerTap, weaponsDatas[i].bulletsLeft, weaponsDatas[i].spread, weaponsDatas[i].range, weaponsDatas[i].reloadTime, weaponsDatas[i].timeBetweenShots, weaponsDatas[i].force, weaponsDatas[i].melee, weaponsDatas[i].allowButtonHold, weaponsDatas[i].shooting, weaponsDatas[i].readyToShoot, weaponsDatas[i].reloading, weaponsDatas[i].text);
        currentWeapon = weapon;
        weapon.transform.parent = gameObject.transform;
    }

    void WeaponsSelectorUpdate()
    {
        if(Input.GetKeyDown(KeyCode. P))
            CreateWeapon(weaponsSelected = ((weaponsSelected+1)%weaponsDatas.Count+weaponsDatas.Count)%weaponsDatas.Count);
        else if(Input.GetKeyDown(KeyCode. M))
            CreateWeapon(weaponsSelected = ((weaponsSelected-1)%weaponsDatas.Count+weaponsDatas.Count)%weaponsDatas.Count);
    }
}
public class WeaponsData{
	public Mesh weaponModel, bulletModel;
    public Material material;
    public Camera cam;
    public TextMeshProUGUI text;
    public int damage, magazineSize, bulletsPerTap, bulletsLeft;
    public float spread, range, reloadTime, timeBetweenShots, force;
    public bool melee, allowButtonHold, shooting, readyToShoot, reloading;

    public WeaponsData(Mesh weaponModel,Material material,Camera cam,int damage,int magazineSize,int bulletsPerTap,int bulletsLeft, float spread,float range,float reloadTime,float timeBetweenShots,float force,bool melee,bool allowButtonHold,bool shooting,bool readyToShoot,bool reloading, TextMeshProUGUI text)
    {
        /*bulletsLeft = magazineSize;
        readyToShoot = true;*/
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
};
