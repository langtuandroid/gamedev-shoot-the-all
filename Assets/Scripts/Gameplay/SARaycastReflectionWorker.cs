using UnityEngine;

namespace Gameplay
{
	[RequireComponent(typeof(LineRenderer))]
	public class SARaycastReflectionWorker : MonoBehaviour
	{
		private const int _maxReflections = 10;
		private const float _maxLength = 30f;
		private LineRenderer _thisLineRenderer;
		private Collider[] _colliders;
		private bool _isKillableOnShot;

		public bool IsKillableOnShot => _isKillableOnShot;

		private void Awake()
		{
			_thisLineRenderer = GetComponent<LineRenderer>();
			_colliders = new Collider[50];
		}

		private void Update()
		{
			var ray = new Ray(transform.position, transform.forward);
			_thisLineRenderer.positionCount = 1;
			_thisLineRenderer.SetPosition(0, transform.position);
			float remainingLength = _maxLength;

			var count = Physics.OverlapSphereNonAlloc(ray.origin, 0.11f, _colliders);
			for (int i = 0; i < count; i++)
			{
				if (_colliders[i].tag.Equals("Player") || _colliders[i].tag.Equals("Emeny"))
				{
					_thisLineRenderer.positionCount += 1;
					_thisLineRenderer.SetPosition(_thisLineRenderer.positionCount - 1, ray.origin + ray.direction * 0.3f);
					_isKillableOnShot = true;
					return;
				}
			}
			_isKillableOnShot = false;
			for (int i = 0; i < _maxReflections; i++)
			{
				if (Physics.Raycast(ray.origin, ray.direction, out var hit, remainingLength))
				{
					_thisLineRenderer.positionCount += 1;
					_thisLineRenderer.SetPosition(_thisLineRenderer.positionCount - 1, hit.point);
					remainingLength -= Vector3.Distance(ray.origin, hit.point);
					if (!hit.collider.CompareTag("Mirror")) break;
					ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
				}
				else
				{
					_thisLineRenderer.positionCount += 1;
					_thisLineRenderer.SetPosition(_thisLineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
				}
			}
		}
	}
}