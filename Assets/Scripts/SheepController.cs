using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    public bool canCooperation = false;

    public PlayerController playerController;
    Tweener _tweener, _tweenerIce;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        _tweener = this.transform.DOMove(transform.position, playerController.speedOnLand).SetAutoKill(false);
        //_tweenerIce = this.transform.DOMove(transform.position, speedOnIce).SetAutoKill(false);

        
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)-transform.position.y;

    }

    public void CooperateWithPlayer(Vector3 direction)
    {
        if (!canCooperation)
            return;

        _tweener.ChangeEndValue(transform.position + direction, true).Play();
    }

}
