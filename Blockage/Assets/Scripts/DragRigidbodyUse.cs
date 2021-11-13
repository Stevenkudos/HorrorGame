using System.Linq;
using UnityEngine;

[System.Serializable]
public class GrabObjectClass
{
	//public bool mFreezeRotation;
	public float mPickupRange = 3f;
	public float mDistance = 3f;
	public float mMAXDistanceGrab = 4f;
}

[System.Serializable]
public class DoorGrabClass
{
	public float mDoorPickupRange = 2f;
	public float mDoorDistance = 2f;
	public float mDoorMaxGrab = 3f;
}

[System.Serializable]
public class TagsClass
{
	public string mInteractTag = "Object";
	public string mDoorsTag = "Door";
}

public class DragRigidbodyUse : MonoBehaviour
{

	public GameObject playerCam;

	public string grabButton = "Grab";
	public GrabObjectClass objectGrab = new GrabObjectClass();
	public DoorGrabClass doorGrab = new DoorGrabClass();
	public TagsClass tags = new TagsClass();

	private float pickupRange = 3f;
	private float distance = 3f;
	private float maxDistanceGrab = 4f;
	
	private GameObject objectHeld;
	private bool isObjectHeld;
	private bool tryPickupObject;
	private Rigidbody rb;

	void Start()
	{
		isObjectHeld = false;
		tryPickupObject = false;
		objectHeld = null;
	}

	void FixedUpdate()
	{
		if (Input.GetButton(grabButton))
		{
			if (!isObjectHeld)
			{
				TryPickObject();
				tryPickupObject = true;
			}
			else
			{
				HoldObject();
			}
		}
		else if (isObjectHeld) 
			DropObject();
	}

	private void TryPickObject()
	{
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

		if (Physics.Raycast(playerAim, out var hit, pickupRange))
		{
			string[] interactableTags = {"Interact", "Door", "Usable"};
			objectHeld = hit.collider.gameObject;
			rb = objectHeld.GetComponent<Rigidbody>();
			if (!tryPickupObject || !interactableTags.Contains(hit.collider.tag)) return;
			if (hit.collider.CompareTag(tags.mInteractTag))
			{
				isObjectHeld = true;
				rb.useGravity = false;
				//objectGrab.mFreezeRotation = rb.freezeRotation;
				pickupRange = objectGrab.mPickupRange;
				distance = objectGrab.mDistance;
				maxDistanceGrab = objectGrab.mMAXDistanceGrab;
			}

			if (hit.collider.CompareTag(tags.mDoorsTag))
			{
				isObjectHeld = true;
				rb.useGravity = true;
				rb.freezeRotation = false;
				pickupRange = doorGrab.mDoorPickupRange;
				distance = doorGrab.mDoorDistance;
				maxDistanceGrab = doorGrab.mDoorMaxGrab;
			}
		}
	}

	private void HoldObject()
	{
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

		if (objectHeld != null)
		{
			Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
			Vector3 currPos = objectHeld.transform.position;

			rb.velocity = (nextPos - currPos) * 10;

			if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
			{
				DropObject();
			}
		}
	}

	private void DropObject()
	{
		isObjectHeld = false;
		tryPickupObject = false;
		rb.useGravity = true;
		rb.freezeRotation = false;
		objectHeld = null;
	}
}
