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
    public GameObject hand;
    public List<WeaponsData> WeaponsList = new List<WeaponsData>();
    public int weaponsSelected, objectrotation;
    public float temp;
    public Canvas canvas;
    GameObject UIWeapon;
    TextMeshProUGUI UIammo, UIname;
    private void Start() {
        
        WeaponsList.Add(new WeaponsData(gameObject,"Hand",mesh,mesh,Resources.Load<Material>("Materials/0"),1, 20, 20, 2, 10, 10, 0.8f, 0.1f, 0.2f, 10f, true));
        WeaponsList.Add(new WeaponsData(gameObject,"Pompe",mesh,mesh,Resources.Load<Material>("Materials/0"),0, 20, 20, 2, 10, 10, 0.8f, 0.1f, 0.2f, 10f, true));
        UIWeapon = Instantiate(Resources.Load<GameObject>("UI/WeaponUI"));
        UIWeapon.transform.SetParent(gameObject.GetComponentInChildren<Canvas>().transform, false);
        UIname = UIWeapon.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        UIammo = UIWeapon.transform.Find("Ammo").GetComponent<TextMeshProUGUI>();
        EquipWeapon(0);
    }
    private void Update() {
        ObjectUpdate();
        UIUpdate();
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
                    if(hit.transform.gameObject.GetComponent<Entity>())// objet prenable ? avec les tag triage
                    {
                        InteractEntity(hit.transform.gameObject);
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
            else if(Input.GetKeyDown(KeyCode. K))
            {
                objectrotation = (objectrotation+90)%360;            
            }
            else
            {
                MoveObject();
            }
        }

        if (Input.GetKeyDown(KeyCode. G))
        {
            DropWeapons();
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

    void PlaceObject(RaycastHit hit)
    {
        Destroy(heldObject.GetComponent<Rigidbody>());
        heldObject.transform.position =  hit.transform.position;
        heldObject.transform.rotation =  Quaternion.Euler(0,objectrotation,0);;
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
        rb.AddForce(transform.forward * throwForce,ForceMode.Impulse);
        heldObject.layer = 0;
        heldObject = null;
    }
    IEnumerator UpdateGhostObject(GameObject ghostObject)
    {     
        while(heldObject != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
            {
                ghostObject.transform.position = hit.point;
                ghostObject.transform.localRotation =  Quaternion.Euler(0,objectrotation,0);
                if(hit.transform.gameObject.tag == "Grid"){
                    ghostObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/1");
                    ghostObject.transform.position =  hit.transform.position;
                }
                else
                    ghostObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/0");
            }
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(ghostObject);
    }

    void EquipWeapon(int i)
    {
        Destroy(currentWeapon);
        GameObject weapon = new GameObject("weapon",typeof(MeshFilter),typeof(MeshRenderer));
        currentWeapon = weapon;
        switch (WeaponsList[i].typeOfWeapons)
        {
            case 0 : weapon.AddComponent<WeaponsGuns>().Init(WeaponsList[i]); break;
            case 1 : weapon.AddComponent<WeaponsMelee>().Init(WeaponsList[i]); break;
            default: Debug.Log("Error"); break;
        }
    }

    void DropWeapons()
    {
        if(weaponsSelected == 0)
            return;
        WeaponsList.RemoveAt(weaponsSelected);
        currentWeapon.GetComponent<Weapons>().ToEntity();
        currentWeapon = hand;
        EquipWeapon(weaponsSelected = 0);
    }

    void InteractEntity(GameObject entity)//ici qu'on va faire en sorte de trier les item recuperable et quest ce qui recupere
    {
        EntityWeapons entityWeapons = entity.GetComponent<EntityWeapons>();
        if(entity != null){
            entityWeapons.OnInteraction(gameObject);
        }
    }

    void WeaponsSelectorUpdate()
    {
        if(Input.GetKeyDown(KeyCode. P))
            EquipWeapon(weaponsSelected = Clamp(WeaponsList.Count, weaponsSelected, 1));
        else if(Input.GetKeyDown(KeyCode. M))
            EquipWeapon(weaponsSelected = Clamp(WeaponsList.Count, weaponsSelected, -1));
    }

    int Clamp(int max, int value, int add){
        return ((value+add)%max+max)%max;
    }

    private void UIUpdate() {
        if(currentWeapon != hand)
        {
            UIname.SetText(currentWeapon.GetComponent<Weapons>().weaponsData.Name);
            UIammo.SetText(currentWeapon.GetComponent<Weapons>().weaponsData.magazine +" / "+ currentWeapon.GetComponent<Weapons>().weaponsData.magazineSize);
        }
    }
}