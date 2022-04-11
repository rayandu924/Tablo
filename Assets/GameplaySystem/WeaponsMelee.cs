using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsMelee : Weapons
{

    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        if (weaponsData.allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Shoot
        if (readyToUse && shooting){
            base.Use();
        }
    }
    protected override void Fire()
    {
            //raycast
  //          RaycastHit hit;
//            if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, weaponsData.range))
            {
    //            Debug.Log(hit.collider.name);

                //if (rayHit.collider.CompareTag("Enemy"))
                    // degat
            }
    }
}