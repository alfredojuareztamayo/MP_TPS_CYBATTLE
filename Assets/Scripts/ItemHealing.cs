using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//using System.Collections;

public class ItemHealing : MonoBehaviourPunCallbacks
{
    private Collider itemCollider;
    private Renderer itemRenderer;

    public float healAmount = 1f; // Cantidad de salud que el ítem restaura
    // Start is called before the first frame update
    void Start()
    {
        itemCollider = GetComponent<Collider>();
        itemRenderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PhotonView photonView = other.GetComponent<PhotonView>();
           // string name = GetComponent<PhotonView>().name;
            if (photonView != null && photonView.IsMine)
            {
                // Llama al método RPC para curar al jugador que recogió el ítem
                other.GetComponent<PhotonView>().RPC("HealPlayer", RpcTarget.AllBuffered, healAmount, other.GetComponent<PhotonView>().Owner.NickName);
                Debug.Log(other.GetComponent<PhotonView>().Owner.NickName);
                // Desactivar el ítem temporalmente
                this.GetComponent<PhotonView>().RPC("DeactivateItem", RpcTarget.All);
            }
        }
    }

    [PunRPC]
     void DeactivateItem()
    {
        itemCollider.enabled = false;
        itemRenderer.enabled = false;
        StartCoroutine(ReactivateItem());
    }


    private IEnumerator ReactivateItem()
    {
        yield return new WaitForSeconds(10f); // Tiempo que el ítem permanece desactivado
        this.GetComponent<PhotonView>().RPC("TurnOnHealth", RpcTarget.All);
    }
    [PunRPC]
    void TurnOnHealth()
    {
        itemCollider.enabled = true;
        itemRenderer.enabled = true;
    }
}
