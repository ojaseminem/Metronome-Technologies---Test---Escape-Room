using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThiefController : MonoBehaviour
{
    [HideInInspector] public bool canMove = false;

    [SerializeField] private Camera cam;
    [SerializeField] private CopSpawner copSpawner;

    private Tweener _moveTween;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            var pos = cam.ScreenToWorldPoint(Input.mousePosition);
            SelectEndPoint(new Vector3(pos.x, pos.y, 0));
        }
    }

    private void SelectEndPoint(Vector3 endPoint)
    {
        _moveTween = transform.DOMove(endPoint * 5, 3f);
        canMove = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cop"))
        {
            var c = other.GetComponent<CopHandler>().distance;
            var randomC = Random.Range(-0.5f, 0.5f);
            
            if (c + randomC > copSpawner.largestDistance || copSpawner.largestDistance < c + randomC)
            {
                GameManager.instance.Remark(Math.Abs(copSpawner.largestDistance - c) < .1f
                    ? "Best Direction !!!"
                    : "Almost !", new Color(0.15f, 1f, 0.53f));
            }
            else
            {
                GameManager.instance.Remark("Incorrect !", new Color(1f, 0.17f, 0.01f));
            }
        }

        if (other.CompareTag("Finish")) _moveTween.Kill();
    }
}