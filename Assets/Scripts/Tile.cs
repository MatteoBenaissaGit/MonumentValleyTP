using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public float OffsetY;
    public List<Tile> Neighbours = new List<Tile>();
    private List<Tile> _tilesDone = new List<Tile>();
    private List<Tile> _tilesPath = new List<Tile>();

    public void DetectNeighbours()
    {
        Neighbours.Clear();
        RaycastHit[] raycastHitsX = Physics.BoxCastAll(transform.position, new Vector3(1f,1f,0.1f), Vector3.down, Quaternion.identity);
        RaycastHit[] raycastHitsY = Physics.BoxCastAll(transform.position, new Vector3(0.1f,1f,1f), Vector3.down, Quaternion.identity);
        foreach (RaycastHit ray in raycastHitsX)
        {
            Tile tile = ray.collider.gameObject.GetComponent<Tile>();
            if (tile != null && tile != this && Neighbours.Contains(tile) == false)
            {
                Neighbours.Add(ray.collider.gameObject.GetComponent<Tile>());
            }
        }
        foreach (RaycastHit ray in raycastHitsY)
        {
            Tile tile = ray.collider.gameObject.GetComponent<Tile>();
            if (tile != null && tile != this && Neighbours.Contains(tile) == false)
            {
                Neighbours.Add(ray.collider.gameObject.GetComponent<Tile>());
            }
        }
    }

    private void OnMouseDown()
    {
        //list
        _tilesDone.Clear();
        _tilesDone.Add(GameCore.Instance.Player.CurrentTile);
        _tilesPath.Clear();
        _tilesPath.Add(GameCore.Instance.Player.CurrentTile);

        if (FindTile(GameCore.Instance.Player.CurrentTile, this) && GameCore.Instance.Player.IsMoving == false)
        {
            GameCore.Instance.Player.MoveToTile(_tilesPath);
            transform.DOMoveY(transform.localPosition.y - 0.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
        }
        
        //Debug.Log($"from {GameCore.Instance.Player.CurrentTile.name} to {name}");
    }

    #region FindTile
    
    private bool FindTile(Tile startTile, Tile endTile)
    {
        foreach (Tile neighbour in startTile.Neighbours)
        {
            if (_tilesDone.Contains(neighbour) == false)
            {
                //Debug.Log($"  {startTile.name} -> {neighbour.name} == {neighbour == endTile}");
                
                _tilesDone.Add(neighbour);
                _tilesPath.Add(neighbour);
                if (neighbour == endTile || FindTile(neighbour, endTile))
                {
                    return true;
                }
                _tilesPath.Remove(neighbour);
            }
        }
        return false;
    }

    #endregion
    
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        foreach (Tile tile in Neighbours)
        {
            Gizmos.color = Color.white;
            
            if (tile == null)
            {
                return;
            }

            Vector3 offset = new Vector3(0, 0.55f, 0);
            
            if (tile.Neighbours.Contains(this) == false)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(tile.transform.position + offset, 0.2f);
            }
            
            Gizmos.DrawLine(transform.position + offset, tile.transform.position + offset);
        }
    }

#endif
}
