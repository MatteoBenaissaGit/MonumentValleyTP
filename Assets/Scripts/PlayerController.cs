using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Variables

    //tile
    public Tile CurrentTile;
    public List<Tile> TilePath;

    //walk
    [HideInInspector] public bool IsMoving;
    private Animator _animator;
    private readonly int _walkingParameter = Animator.StringToHash("walking");

    #endregion

    #region Methods

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void MoveToTile(List<Tile> tilePath)
    {
        //values setup
        TilePath = tilePath;
        IsMoving = true;
        CurrentTile = TilePath.LastOrDefault();

        //path
        const float moveTime = 0.5f;
        Sequence sequence = DOTween.Sequence();
        for (int i = 1; i < TilePath.Count; i++)
        {
            //move
            Vector3 endPosition = TilePath[i].transform.position + new Vector3(0, 0.5f + TilePath[i].OffsetY, 0);
            sequence.Append(transform.DOMove(endPosition, moveTime).SetEase(Ease.Linear));
        
            //rotate
            const float lookTime = 0.25f;
            Vector3 direction = (endPosition - TilePath[i-1].transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z);
            sequence.Join(transform.DORotateQuaternion(Quaternion.LookRotation(-direction, Vector3.up), lookTime));
        }

        sequence.AppendCallback(MoveEnd);
        sequence.PlayForward();

        //anim
        _animator.SetBool(_walkingParameter, true);
    }

    private void MoveEnd()
    {
        _animator.SetBool(_walkingParameter, false);
        IsMoving = false;
    }

    #endregion
}