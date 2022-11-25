using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CopSpawner : MonoBehaviour
{
    [HideInInspector] public float largestDistance;
    
    [SerializeField] private int numOfCopsToSpawn;
    [SerializeField] private Transform copPrefab;
    [SerializeField] private Transform thiefCharacter;

    private List<CopHandler> _copList = new();
    
    public void SpawnCops()
    {
        _copList.Clear();
        
        StartCoroutine(Spawn());
        
        IEnumerator Spawn()
        {
            //Spawn Cops
            var previousAngle = 0;
            for (int i = 0; i < numOfCopsToSpawn; i++)
            {
                var constAngle = 360 / numOfCopsToSpawn;
                previousAngle += constAngle;
                var currentCop = Instantiate(copPrefab, new Vector3(0, RandomOffset()), Quaternion.identity);
                currentCop.RotateAround(thiefCharacter.position, Vector3.forward, previousAngle + RandomAngle(constAngle));
                currentCop.LookAt(Vector3.forward,Vector3.Cross(Vector3.forward,thiefCharacter.position - currentCop.transform.position));
                currentCop.rotation = Quaternion.Euler(0, 0, currentCop.rotation.z);
                currentCop.SetParent(transform);
                var edgeCollider = currentCop.gameObject.AddComponent<EdgeCollider2D>();
                edgeCollider.isTrigger = true;
                _copList.Add(currentCop.GetComponent<CopHandler>());
            }
            
            yield return new WaitForSeconds(.15f);
            
            //Set Edge Collider Length and Distance
            for (int i = 0; i < _copList.Count; i++)
            {
                var a = _copList[i].transform;
                
                var b = _copList[i] == _copList[9] ? _copList[0].transform : _copList[i + 1].transform;
                
                var t = b.position - a.position;

                a.GetComponent<EdgeCollider2D>().points = new Vector2[]
                {
                    new(t.x, t.y),
                    new()
                };

                var d = Vector2.Distance(a.position, b.position);

                a.GetComponent<CopHandler>().distance = d;
            }

            yield return new WaitForSeconds(.15f);
            
            //Get Max Value of the edge collider distance
            var tempList = _copList.ToArray();
            var distanceList = new List<float>();
            foreach (var copHandler in tempList)
            {
                distanceList.Add(copHandler.distance);
            }

            yield return new WaitForSeconds(.1f);

            largestDistance = Mathf.Max(distanceList.ToArray());
        }
    }
    
    public void ResetCops()
    {
        thiefCharacter.position = Vector3.zero;
        foreach (var c in _copList)
        {
            Destroy(c.gameObject);
        }
    }

    private int RandomAngle(int angle)
    {
        var randomMinAngle = Random.Range(-angle/4, -angle/2);
        var randomMaxAngle = Random.Range(angle/4, angle/2);

        bool minOrMax = Random.value > .5f;

        return minOrMax ? randomMaxAngle : randomMinAngle;
    }

    private float RandomOffset()
    {
        return Random.Range(3.5f, 4.5f);
    }
}