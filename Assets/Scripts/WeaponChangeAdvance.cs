using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponChangeAdvance : MonoBehaviour
{
    public TwoBoneIKConstraint leftHand, rightHand, leftThumb;
    public RigBuilder rig;
    public Transform[] leftTarget, rightTarget, thumbTarget;
    public GameObject[] weapons;
    private int weaponNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            weaponNumber++;
            if (weaponNumber > weapons.Length - 1) weaponNumber = 0;
            for(int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(false);
            }
            weapons[weaponNumber].SetActive(true);
            leftHand.data.target = leftTarget[weaponNumber];
            rightHand.data.target = rightTarget[weaponNumber];
            leftThumb.data.target = thumbTarget[weaponNumber];
            rig.Build();
        }
    }
}
