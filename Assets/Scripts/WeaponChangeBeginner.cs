using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponChangeBeginner : MonoBehaviour
{
    public TwoBoneIKConstraint leftHand, rightHand;
    public RigBuilder rig;
    public Transform leftTargetWeapon1, leftTargetWeapon2, leftTargetWeapon3;
    public Transform rightTargetWeapon1, rightTargetWeapon2, rightTargetWeapon3;
    public GameObject weapon1, weapon2, weapon3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeWeaponKey();
    }

    void ChangeWeaponKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            ActiveRigWeapons(true,false,false, leftTargetWeapon1, rightTargetWeapon1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            ActiveRigWeapons(false, true, false, leftTargetWeapon2, rightTargetWeapon2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            ActiveRigWeapons(false, false, true, leftTargetWeapon3, rightTargetWeapon3);
        }

    }

    private void ActiveRigWeapons(bool w1,bool w2,bool w3,Transform leftTarget, Transform rightTarget)
    {
        weapon1.SetActive(w1);
        weapon2.SetActive(w2);
        weapon3.SetActive(w3);
        leftHand.data.target = leftTarget;
        rightHand.data.target = rightTarget;
        rig.Build();
    }
}
