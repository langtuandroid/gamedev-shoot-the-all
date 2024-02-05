using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastReflection : MonoBehaviour
{
	public int reflections;
	private const float maxLength = 30f;

	private LineRenderer lineRenderer;
	private Ray ray;
	private RaycastHit hit;
	private Vector3 direction;
	private Collider[] colliders;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		colliders = new Collider[50];
	}

	private void Update()
	{
		ray = new Ray(transform.position, transform.forward);
		lineRenderer.positionCount = 1;
		lineRenderer.SetPosition(0, transform.position);
		float remainingLength = maxLength;

		var count = Physics.OverlapSphereNonAlloc(ray.origin, 0.1f, colliders);
		for (int i = 0; i < count; i++)
		{
			if (colliders[i].tag.Equals("Player")) return;
		}
		for (int i = 0; i < reflections; i++)
		{
			if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
			{
				lineRenderer.positionCount += 1;
				lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
				remainingLength -= Vector3.Distance(ray.origin, hit.point);
				ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
				if (!hit.collider.CompareTag("Mirror")) break;
			}
			else
			{
				lineRenderer.positionCount += 1;
				lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
			}
		}
	}
}