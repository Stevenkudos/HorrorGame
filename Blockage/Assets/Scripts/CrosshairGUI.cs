using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CrosshairGUI : MonoBehaviour
{
	public Texture2D mCrosshairTexture;
	public Texture2D mUseTexture;
	public float rayLength = 3f;

	public bool mDefaultReticle = true;
	public bool mUseReticle;

	private bool isCrosshairVisible = false;
	private Rect mCrosshairRect;
	private Camera playerCam;
	public LayerMask usable;
	public Text hint;

	void FixedUpdate()
	{
		playerCam = Camera.main;
		if (playerCam != null)
		{
			Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			RaycastHit hit;

			if (Physics.Raycast(playerAim, out hit, rayLength, usable))
			{
				switch (hit.collider.gameObject.tag)
				{
					case "Name":
						mDefaultReticle = false;
						mUseReticle = true;
						hint.text = hit.collider.name;
						break;
					case "Unlock":
						mDefaultReticle = false;
						mUseReticle = true;
						hint.text = "Unlock[E]";
						break;
				}
			}
			else
			{
				hint.text = "";
				mDefaultReticle = true;
				mUseReticle = false;
			}
		}
	}

	void Awake()
	{
		if (mDefaultReticle)
		{
			mCrosshairRect = new Rect((Screen.width - mCrosshairTexture.width) *.5f,
								  (Screen.height - mCrosshairTexture.height)*.5f,
								  mCrosshairTexture.width,
								  mCrosshairTexture.height);
		}

		if (mUseReticle)
		{
			mCrosshairRect = new Rect((Screen.width - mUseTexture.width)*.5f,
								  (Screen.height - mUseTexture.height)*.5f,
								  mUseTexture.width*0.3f,
								  mUseTexture.height*0.3f);
		}
	}

	void OnGUI()
	{
		if (isCrosshairVisible && mDefaultReticle)
			GUI.DrawTexture(mCrosshairRect, mCrosshairTexture);

		if (mUseReticle)
			GUI.DrawTexture(mCrosshairRect, mUseTexture);
	}
}
