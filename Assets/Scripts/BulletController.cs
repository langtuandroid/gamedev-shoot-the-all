using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private List<Vector3> _listOfPoints;
    private int i = 0;

    private void Update()
    {
        if (_listOfPoints != null && _listOfPoints.Count > 0)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _listOfPoints[i], 10 * Time.deltaTime);

            transform.LookAt(_listOfPoints[i]);
            if (transform.position == _listOfPoints[i])
            {
                if (i < _listOfPoints.Count - 1)
                {
                    i++;
                }
                else
                {
                    Destroy(gameObject, 0.1f);
                }
            }
        }
        else Destroy(gameObject);
    }

    public void SetPath(List<Vector3> listOfPoints)
    {
        i = 0;
        _listOfPoints = listOfPoints;
    }
}