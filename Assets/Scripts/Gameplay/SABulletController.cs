using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class SABulletController : MonoBehaviour
    {
        private List<Vector3> _listOfPoints;
        private int _pathPointIndex = 0;

        private void Update()
        {
            if (_listOfPoints != null && _listOfPoints.Count > 0)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _listOfPoints[_pathPointIndex], 10 * Time.deltaTime);

                transform.LookAt(_listOfPoints[_pathPointIndex]);
                if (transform.position == _listOfPoints[_pathPointIndex])
                {
                    if (_pathPointIndex < _listOfPoints.Count - 1)
                    {
                        _pathPointIndex++;
                    }
                    else
                    {
                        Destroy(gameObject, 0.15f);
                    }
                }
            }
            else Destroy(gameObject);
        }

        public void SetPath(List<Vector3> listOfPoints)
        {
            _pathPointIndex = 0;
            _listOfPoints = listOfPoints;
        }
    }
}