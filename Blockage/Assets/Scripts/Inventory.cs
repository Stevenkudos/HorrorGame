using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[Serializable]
public class KeyItems
{
    public string keyName;
    public Sprite keyIcon;
    public GameObject keyPrefab;
}

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public List<GameObject> slotSpawnPos;

    public List<KeyItems> keys;
    public Camera mainCam;

    private GameObject hitKeyObject;
    int keyIndex;
    int selectedKeyIndex;
    private int chosenIndex;
    public LayerMask usableItem;
    public List<GameObject> storedKey = new List<GameObject>();
    public GameObject player;

    private bool haveSmallKey;
    private bool haveBigKey;
    
    private void Awake()
    {
        keys.Add(keys[0]);
        keys.Add(keys[1]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedKeyIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedKeyIndex = 1;
        }

        if (storedKey.Count > 0 && slotSpawnPos[selectedKeyIndex].GetComponent<Image>().sprite != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropKey();
            }
        }
    }

    private void FixedUpdate()
    {
        Ray playerAim = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(playerAim, out var hit, 5f, usableItem))
        {
            keyIndex = hit.collider.gameObject.name switch
            {
                "SmallKey" => 0,
                "BigKey" => 1,
                "SmallKey(Clone)" => 0,
                "BigKey(Clone)" => 1,
                _ => keyIndex
            };
            if (hit.collider.gameObject != null)
                hitKeyObject = hit.collider.gameObject;
            if (Input.GetButton("Use"))
            {
                OnPickUp();
            }
        }

        if (Physics.Raycast(playerAim,out var hit2, 5f))
        {
            switch(hit2.collider.gameObject.name)
            {
                case "Lock":
                    if (haveSmallKey && Input.GetButton("Use"))
                    {
                        hit2.collider.GetComponent<OpenableObject>().Unlock();
                        RemoveKey("SmallKey");
                    }
                    break;
                case "UVLight":
                    {
                        if (Input.GetButton("Use"))
                        {
                            player.GetComponent<FirstPersonController>().haveUVLight = true;
                            Destroy(hit2.collider.gameObject);
                        }
                    }
                    break;
            }
        }

        slotSpawnPos[selectedKeyIndex].GetComponent<Image>().color = Color.Lerp(
            slotSpawnPos[selectedKeyIndex].GetComponent<Image>().color, new Color(1, 1, 1, 0.9f), Time.deltaTime * 7);
        for (int i = 0; i < slotSpawnPos.Count; i++)
        {
            if (i != selectedKeyIndex && slotSpawnPos[i]!= null)
            {
                slotSpawnPos[i].GetComponent<Image>().color = Color.Lerp(
                    slotSpawnPos[i].GetComponent<Image>().color, new Color(1, 1, 1, 0.1f), Time.deltaTime * 7);
            }
        }
        CurrentSlot();
    }

    void OnPickUp()
    {
        if (hitKeyObject == null) return;
        slotSpawnPos[storedKey.Count].GetComponent<Image>().sprite = keys[keyIndex].keyIcon;
        storedKey.Add(keys[keyIndex].keyPrefab);
        Destroy(hitKeyObject);

        switch (keyIndex)
        {
            case 0:
                haveSmallKey = true;
                break;
            case 1:
                haveBigKey = true;
                break;
        }
    }

    void DropKey()
    {      
        var playerPos = player.transform.position;
        var playerFacingDir = player.transform.forward;
        var playerRot = player.transform.rotation;
        var spondPos = playerPos + playerFacingDir * 2;
        Instantiate(storedKey[selectedKeyIndex], spondPos, playerRot);
        RemoveKey("null");
    }

    void RemoveKey(string keyName)
    {
        if (keyName == "null")
        {
            switch (storedKey[selectedKeyIndex].name)
            {
                case "SmallKey":
                    haveSmallKey = false;
                    break;
                case "BigKey":
                    haveBigKey = false;
                    break;
                case "SmallKey(Clone)":
                    haveSmallKey = false;
                    break;
                case "BigKey(Clone)":
                    haveBigKey = false;
                    break;
            }
            storedKey.RemoveAt(selectedKeyIndex);
            slotSpawnPos[selectedKeyIndex].GetComponent<Image>().sprite = null;
            if (slotSpawnPos[selectedKeyIndex + 1] == null) return;
            if (slotSpawnPos[selectedKeyIndex + 1].GetComponent<Image>().sprite != null)
            {
                slotSpawnPos[selectedKeyIndex].GetComponent<Image>().sprite = slotSpawnPos[selectedKeyIndex + 1].GetComponent<Image>().sprite;
                slotSpawnPos[selectedKeyIndex + 1].GetComponent<Image>().sprite = null;
            }
        }
        else
        {
            chosenIndex = storedKey.IndexOf(storedKey.Find(x => x.name == keyName));
            switch (storedKey[chosenIndex].name)
            {
                case "SmallKey":
                    haveSmallKey = false;
                    break;
                case "BigKey":
                    haveBigKey = false;
                    break;
                case "SmallKey(Clone)":
                    haveSmallKey = false;
                    break;
                case "BigKey(Clone)":
                    haveBigKey = false;
                    break;
            }
            storedKey.Remove(storedKey.Find(x=> x.name == keyName));
            print(chosenIndex);
            slotSpawnPos[chosenIndex].GetComponent<Image>().sprite = null;
            if (slotSpawnPos[chosenIndex + 1] == null) return;
            if (slotSpawnPos[chosenIndex + 1].GetComponent<Image>().sprite != null)
            {
                slotSpawnPos[chosenIndex].GetComponent<Image>().sprite = slotSpawnPos[chosenIndex + 1].GetComponent<Image>().sprite;
                slotSpawnPos[chosenIndex + 1].GetComponent<Image>().sprite = null;
            }
        }
    }

    void CurrentSlot()
    {
        if (storedKey.Count == 0) return;
        for (var i = 0; i < storedKey.Count; i++)
        {
            if (storedKey[i] == null && storedKey[i + 1] != null)
            {
                storedKey.Insert(i, storedKey[i + 1]);
                storedKey.RemoveAt(i + 1);
            }
        }
    }
}    
