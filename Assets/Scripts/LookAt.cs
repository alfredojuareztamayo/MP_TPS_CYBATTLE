using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAt : MonoBehaviour
{
    private Vector3 worldPosition, screenPosition;
    public GameObject crosshair;
    public Text nickNameText;


    private void Start()
    {
        Cursor.visible = false;
        nickNameText.text = PhotonNetwork.LocalPlayer.NickName;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = 3f;

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = worldPosition;

        crosshair.transform.position = Input.mousePosition;
    }
}
