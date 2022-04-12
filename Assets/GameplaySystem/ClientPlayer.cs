using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;


public class ClientPlayer : MonoBehaviour
{
    public int moveForce;
    public int interactionDistance;
    public List<string> GhostComponent = new List<string>();
    public float throwForce;
    public GameObject heldObject;
    public GameObject currentWeapon;
    public GameObject hand;
    public List<WeaponsData> weapons = new List<WeaponsData>();
    public int weaponsSelected = 0, objectrotation = 0;
    public float temp;
    public Canvas canvas;
    public TextMeshProUGUI UIweaponAmmo, UIweaponName;
    public Camera cam;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public Rigidbody rigibody;
    public Vector3 inputsPos = Vector3.zero;
    public Vector3 inputsRot = Vector3.zero;
    public bool _isGrounded = true;
    public Transform _groundChecker;
    public int sensSpeed;
    public int sensSpeedRun;
    public int SensMouse;
    public List<GameObject> SpawnmenuPrefab= new List<GameObject>();
    public GameObject ButtonPrefab;
    public GameObject content;
    public GameObject SpawnMenu;

    void Start()
    {
        SpawnmenuPrefab = Resources.LoadAll<GameObject>("BuildingPrefabs/").ToList();
        weapons.Add(new WeaponsData(WeaponType.Melee, "Hand", Resources.Load<Mesh>("Mesh/Axe"), Resources.Load<Material>("Materials/0"), Resources.Load<Mesh>("Mesh/Axe"),0,0,0,1,1,0,5,0,0,true,new Vector3(0.025f,-0.25f,-0.2f), new Vector3(-58,180,-92), PlayerStateTop.Fire));
        weapons.Add(new WeaponsData(WeaponType.Guns, "Pompe", Resources.Load<Mesh>("Mesh/Axe"), Resources.Load<Material>("Materials/1"), Resources.Load<Mesh>("Mesh/Axe"),20,20,2,10,1,1,5,0.1f,10,true,new Vector3(0.025f,-0.25f,-0.2f), new Vector3(-58,180,-92),PlayerStateTop.Fire));
        weapons.Add(new WeaponsData(WeaponType.Melee, "Hache", Resources.Load<Mesh>("Mesh/Axe"), Resources.Load<Material>("Materials/0"), Resources.Load<Mesh>("Mesh/Axe"),0,0,0,1,1,0,5,0,0,true,new Vector3(0.025f,-0.25f,-0.2f), new Vector3(-58,180,-92),PlayerStateTop.Fire));
        EquipWeapon(0);
        initSpawnMenu();
    }

    void Update()
    {
        updateController();
        UIUpdate();
    }

    void PickupObject(GameObject obj)
    {
        Rigidbody rb;
        if(!(rb = obj.GetComponent<Rigidbody>()))
            rb = obj.AddComponent<Rigidbody>();
        obj.layer = 2;
        rb.angularDrag = 7;
        heldObject = obj;
        CreateGhostObject();
    }

    void MoveObject(){
        if(Vector3.Distance(heldObject.transform.position, cam.transform.position)> 0.1f)
        {
            Vector3 moveDirection = (cam.transform.position - heldObject.transform.position + cam.transform.forward * temp);
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.angularDrag = 0;
        heldObject.layer = 0;
        heldObject = null;
    }

    void PlaceObject(RaycastHit hit)
    {
        Destroy(heldObject.GetComponent<Rigidbody>());
        heldObject.transform.position =  hit.transform.position - Vector3.down;
        heldObject.transform.localRotation =  Quaternion.Euler(0,(objectrotation*90)%360,0);
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
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, interactionDistance))
            {
                ghostObject.transform.localRotation =  Quaternion.Euler(0,(objectrotation*90)%360,0);
                if(hit.transform.gameObject.tag == "Grid"){
                    ghostObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/1");
                    ghostObject.transform.position = hit.transform.position - Vector3.down;
                }
                else{
                    ghostObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/0");
                    ghostObject.transform.position = hit.point - Vector3.down;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(ghostObject.gameObject);
    }

    void EquipWeapon(int i)
    {
        Clamp(weapons.Count, ref weaponsSelected, i);
        Destroy(currentWeapon);
        GameObject weapon = new GameObject("weapon",typeof(MeshFilter),typeof(MeshRenderer));
        currentWeapon = weapon;
        weapon.transform.parent = cam.transform;
        weapon.transform.localPosition = weapons[weaponsSelected].position;
        weapon.transform.localRotation = Quaternion.Euler(weapons[weaponsSelected].rotation);
        switch (weapons[weaponsSelected].weaponType)
        {
            case WeaponType.Guns : weapon.AddComponent<WeaponsGuns>().Init(weapons[weaponsSelected]); break;
            case WeaponType.Melee : weapon.AddComponent<WeaponsMelee>().Init(weapons[weaponsSelected]); break;
            default: Debug.Log("Error"); break;
        }
    }

    void DropWeapons()
    {
        if(weaponsSelected == 0)
            return;
        weapons.RemoveAt(weaponsSelected);
        currentWeapon.GetComponent<Weapons>().ToEntity();
        currentWeapon = hand;
        EquipWeapon(weaponsSelected = 0);
    }

    void InteractEntity(GameObject entity)//ici qu'on va faire en sorte de trier les item recuperable et quest ce qui recupere
    {
        var entityClass = entity.GetComponent<Entity>();
        if(entity != null){
            entityClass.OnInteraction(gameObject);
        }
    }

    void Clamp(int max, ref int value, int add){
        value = ((value+add)%max+max)%max;
    }

    private void UIUpdate() {
        UIweaponName.SetText(currentWeapon.GetComponent<Weapons>().weaponsData.name);
        UIweaponAmmo.SetText(currentWeapon.GetComponent<Weapons>().weaponsData.magazine +" / "+ currentWeapon.GetComponent<Weapons>().weaponsData.magazineSize);
        if(ActiveSpawnMenuActionKey())
        {
            SpawnMenu.SetActive(!SpawnMenu.activeSelf);
        }
    }

    private void initSpawnMenu() {
        foreach (var Prefab in SpawnmenuPrefab)
        {
            GameObject go = Instantiate(ButtonPrefab);
            var button = go.GetComponent<Button>();
            button.onClick.AddListener(() => Instantiate(Prefab, transform.position + transform.forward, transform.rotation));   
            button.GetComponentInChildren<Image>().sprite = Sprite.Create(AssetPreview.GetAssetPreview(Prefab), new Rect(0,0,128,128), new Vector2(0,0), 1f);
            button.GetComponentInChildren<TextMeshProUGUI>().text = Prefab.name;
            go.transform.SetParent(content.transform);
        }
        //GameObject Prop = Instantiate(Resources.Load<GameObject>(""),transform.position, transform.rotation);
    }


    void updateController()
    {

        // direction
        Vector3 direction = transform.TransformDirection(Vector3.forward);

        inputsPos = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * sensSpeed;
        inputsRot = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * SensMouse;

        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance);
        if(inputsPos == Vector3.zero)
        {
            //UpdatePlayerStateBottomServerRpc(PlayerStateBottom.Idle);
        }
        else
        {
            if (!ActiveRunningActionKey() &&  inputsPos.z > 0)
            {
                //UpdatePlayerStateBottomServerRpc(PlayerStateBottom.Walk);
            }
            else if (ActiveRunningActionKey() && inputsPos.z > 0)
            {
                inputsPos *= sensSpeedRun;
                //UpdatePlayerStateBottomServerRpc(PlayerStateBottom.Run);
            }
            else if (inputsPos.z < 0)
            {
                //UpdatePlayerStateBottomServerRpc(PlayerStateBottom.ReverseWalk);
            }
            rigibody.MovePosition(rigibody.position + inputsPos * Time.fixedDeltaTime);
        }
        if (ActiveJumpActionKey() && _isGrounded)
        {
            rigibody.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
        if (ActiveDashActionKey())
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rigibody.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rigibody.drag + 1)) / -Time.deltaTime)));
            rigibody.AddForce(dashVelocity, ForceMode.VelocityChange);
        }

        cam.transform.eulerAngles += new Vector3(inputsRot.x, 0, 0);
        transform.Rotate(new Vector3(0,inputsRot.y,0));

        if(ActiveScrollUpActionKey())
            EquipWeapon(1);
        else if(ActiveScrollDownActionKey())
            EquipWeapon(-1);

        // hit object
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, interactionDistance);
        if (heldObject == null)
        {
            if (ActiveInteractActionKey())
                if (raycastHit)
                    if(hit.transform.gameObject.tag == "EntityInteract")// objet prenable ? avec les tag triage
                        InteractEntity(hit.transform.gameObject);
                    else if(hit.transform.gameObject.tag == "EntityPickable")
                        PickupObject(hit.transform.gameObject);
        }
        else
        {
            if (ActiveInteractActionKey())
                if (hit.transform.tag == "Grid")
                    PlaceObject(hit);//ammeliorer pour rendre l'objet fixe et voir si oui ou non on met 1 objet sur chaque grid
                else
                    DropObject();
            else if(ActiveThrowActionKey())
                ThrowObject();
            else if(ActiveRotateActionKey())
                Debug.Log("test");//RotateObject();
            else
                MoveObject();
        }

        // change bottom motion states
        if (ActiveFireActionKey())
        {
            //UpdatePlayerStatesTopServerRpc(PlayerStateTop.Fire);
        }
        else if (ActiveDropWeaponActionKey())
            DropWeapons();
        else{
            //UpdatePlayerStatesTopServerRpc(PlayerStateTop.IdleTop);
        }
    }

    private static bool ActiveRunningActionKey()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private static bool ActiveFireActionKey()
    {
        return Input.GetKey(KeyCode.M);
    }

    private static bool ActiveInteractActionKey()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private static bool ActiveThrowActionKey()
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    private static bool ActiveRotateActionKey()
    {
        return Input.GetKeyDown(KeyCode.R);
    }
    private static bool ActiveJumpActionKey()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private static bool ActiveDropWeaponActionKey()
    {
        return Input.GetKeyDown(KeyCode.J);
    }

    private static bool ActiveScrollUpActionKey()
    {
        return Input.GetKeyDown(KeyCode.N);
    }

    private static bool ActiveScrollDownActionKey()
    {
        return Input.GetKeyDown(KeyCode.B);
    }

    private static bool ActiveDashActionKey()
    {
        return Input.GetKeyDown(KeyCode.D);
    }
    private static bool ActiveSpawnMenuActionKey()
    {
        return Input.GetKeyDown(KeyCode.W);
    }
}