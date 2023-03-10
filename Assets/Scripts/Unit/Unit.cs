using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

// Unit base class
public class Unit : MonoBehaviour
{

    public Vector2 index;
    public List<GroundTile> path;
    public float AttackPower;
    public float HealthPoint = 10f;
    public Build CreatorBuild;
    protected float speed = 0.1f;
    protected Sprite splash;
    protected string unitName;
    protected Unit targetUnit;
    protected Build targetBuild;
    protected virtual void Start()
    {
        path = new List<GroundTile>();
    }

    // Mouse clicks
    protected void OnMouseOver()
    {
        //leftclick 
        // select unit
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.sourceUnit = transform;
           
        }
        // rightclick
        else if (Input.GetMouseButtonDown(1))
        {
            // move unit to unit
            GameManager.instance.destinationIndex = index;
           
            GameManager.instance.HandleMovement();


        }
    }
    protected void OnMouseEnter()
    {
        // highlight unit when mouse is over
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
    }
    protected void OnMouseExit()
    {
        // reset the highlight
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }

    //Movement handlers
    public void MoveOnNewPath(List<GroundTile> newPath)
    {
        // when there is a new path stop the previous to start the new
        if (newPath != path || newPath != null)
        {
            path = newPath;
            StopCoroutine("MoveOnPath");
            if (targetBuild != null)
                targetBuild.StopCoroutine("GetDamage");
            if (targetUnit != null)
                targetUnit.StopCoroutine("GetDamage");
            StartCoroutine("MoveOnPath");
        }

    }
    // move along the path
    IEnumerator MoveOnPath()
    {
        int currentIndex = 0;
        Vector2 currentPosition = path[currentIndex].transform.position;
        // free the first tile
        TileManager.instance.tilemap[(int)index.x, (int)index.y].GetComponent<GroundTile>().isOccupied = false;
        TileManager.instance.tilemap[(int)index.x, (int)index.y].GetComponent<GroundTile>().hasUnit = false;
        transform.SetParent(null);
        // move
        while (true)
        {
            if ((Vector2)transform.position == currentPosition)
            {
                // reset the flags after passing the tile
                path[currentIndex].isOccupied = false;
                path[currentIndex].hasUnit = false;
                transform.SetParent(null);
                currentIndex++;
                if (path[path.Count - 1].transform.childCount > 0)
                {
                    if (path[currentIndex].transform.childCount > 0)
                    {
                        targetUnit = path[currentIndex].transform.GetChild(0).GetComponent<Unit>();
                        
                        if (targetUnit != null)
                        {
                            targetBuild = targetUnit.CreatorBuild;
                            if (targetBuild != null)
                            {
                                StartCoroutine(AttackTargetBuilding(targetBuild));

                                targetUnit.transform.parent.GetComponent<GroundTile>().isOccupied = false;

                                targetUnit.transform.parent.GetComponent<GroundTile>().hasUnit = false;
                                Destroy(targetUnit);
                                StopCoroutine("MoveOnPath");
                            }
                            else
                            {
                                StartCoroutine(AttackTargetUnit(targetUnit));
                                StopCoroutine("MoveOnPath");
                            }
                        }
                    }
                }
                // path end
                if (currentIndex >= path.Count)
                {
                    

                    path[currentIndex - 1].isOccupied = true;
                    path[currentIndex - 1].hasUnit = true;
                    transform.SetParent(path[currentIndex - 1].transform);
                    yield break;
                }
                // set the tile flags when on the tile
                currentPosition = path[currentIndex].transform.position;

                path[currentIndex].isOccupied = true;
                path[currentIndex].hasUnit = true;
                transform.SetParent(path[currentIndex].transform);

                index = path[currentIndex].index;
            }
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, speed);

            yield return null;
        }
    }
    IEnumerator AttackTargetUnit(Unit TargetUnit)
    {
        while(TargetUnit.HealthPoint > 0)
        {
            StartCoroutine(TargetUnit.GetDamage(AttackPower));
            if (HealthPoint <=0 || TargetUnit.HealthPoint <=0)
            {
                StopCoroutine("AttackTargetUnit");
            }
            
            yield return new WaitForSeconds(0.3f);
        }

    }
    IEnumerator AttackTargetBuilding(Build TargetBuild)
    {
        while (TargetBuild.HealthPoint > 0)
        {
            StartCoroutine(TargetBuild.GetDamage(AttackPower));
            if (HealthPoint <= 0 || TargetBuild.HealthPoint <= 0)
            {
                StopCoroutine("AttackTargetBuilding");
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    public IEnumerator GetDamage(float damage)
    {
        HealthPoint -= damage;
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        transform.DOShakePosition(0.1f,0.5f,5,45);
        yield return new WaitForSeconds(0.15f);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        if (HealthPoint <= 0)
        {
            Destroy(gameObject);
        }
    }

}
