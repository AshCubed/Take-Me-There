using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    #region Singleton

    public static PlayerInventory instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField] private List<Item> playerItems;
    private List<GameObject> playerItemImages;

    private string pickUpItemTag;

    [SerializeField] private TextMeshProUGUI txtPickUpBtn;
    [SerializeField] private TextMeshProUGUI txtItemName;
    [SerializeField] private TextMeshProUGUI txtItemDesc;

    [Header("Item Canavs Items")]
    [SerializeField] private GameObject txtItemObtained;
    [SerializeField] private GameObject itemsImgParent;
    [SerializeField] private GameObject imageToSpawn;
    bool canSpawnItemImg;

    // Start is called before the first frame update
    void Start()
    {
        canSpawnItemImg = true;
        playerItems = new List<Item>();
        playerItemImages = new List<GameObject>();
        pickUpItemTag = MainManager.instance.GetPickUpItemTag();

        txtPickUpBtn.text = "Use E to pick up Item";
        SetText(null, false);

        txtItemObtained.SetActive(false);
    }

    public void PickUpItem(Item item)
    {
        if (canSpawnItemImg)
        {
            if (playerItems.Count > 0)
            {
                for (int i = 0; i < playerItems.Count; i++)
                {
                    if (playerItems[i] == item)
                    {
                        playerItems.RemoveAt(i);
                        Destroy(playerItemImages[i]);
                        playerItemImages.RemoveAt(i);
                    }
                }
                playerItems.Add(item);
            }
            else
            {
                playerItems.Add(item);
            }

            GameObject image = Instantiate(imageToSpawn, itemsImgParent.transform);
            image.GetComponent<Image>().sprite = item.icon;
            image.SetActive(true);
            canSpawnItemImg = false;
            playerItemImages.Add(image);

            txtItemObtained.SetActive(true);
            Color c = txtItemObtained.GetComponent<TextMeshProUGUI>().color;

            LeanTween.value(txtItemObtained.gameObject, c.a, 1, .8f)
                .setOnUpdate((float val) => {
                    c.a = val;
                    txtItemObtained.GetComponent<TextMeshProUGUI>().color = c;
                }).setOnComplete(() => {
                    LeanTween.value(txtItemObtained.gameObject, c.a, 0, .7f)
                    .setOnUpdate((float val) => {
                        c.a = val;
                        txtItemObtained.GetComponent<TextMeshProUGUI>().color = c;
                    }).setOnComplete(() => {
                        txtItemObtained.SetActive(false);
                        canSpawnItemImg = true;
                    }).setDelay(.5f);
                });
        }
    }

    public void SetText(Item item, bool x)
    {
        SetActiveTxt(x);
        /*if (item != null)
        {
            txtItemName.text = item.name;
            txtItemDesc.text = item.description;
        }
        else
        {
            txtItemName.text = "";
            txtItemDesc.text = "";
        }*/
        
        void SetActiveTxt(bool x)
        {
            txtPickUpBtn.gameObject.SetActive(x);
            //txtItemName.gameObject.SetActive(x);
            //txtItemDesc.gameObject.SetActive(x);
        }
    }

    public bool SearchInventory(Item item)
    {
       return playerItems.Find(x => x == item);
    }
}
