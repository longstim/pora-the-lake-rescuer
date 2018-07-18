using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WearableVirtualItem : MonoBehaviour {

    [System.Serializable]
    public enum ItemType
    { 
        Headwear,
        Bodywear,
        Gun
    }

    public bool isDefault = false;
    public string Name;

    [System.Serializable]
    public class Item
    { 
        public string Alias;
        public ItemType Type;
    }

    public Item[] Items;

    public Text TextName;
    public PoraCustomization poraCustomization;

    public GameObject Equiped;

	// Use this for initialization
	void Start () {
        new UserItemStock().Load();
        gameObject.SetActive(UserItemStock.Items.Contains(Items[0].Alias) || isDefault);
        TextName.text = Name;
	}

    void FixedUpdate()
    {
        bool status = false;
        for (int i = 0; i < Items.Length; i++)
        {
            status = status || Items[i].Alias == poraCustomization.CurrentHeadwear || Items[i].Alias == poraCustomization.CurrentBodywear || Items[i].Alias == poraCustomization.CurrentGunwear;
            if(status)
            {
                break;
            }
        }
        Equiped.SetActive(status);
    }
	

    public void OnClick()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].Type == ItemType.Headwear)
            {
                poraCustomization.UseItemHeadwear(Items[i].Alias);
            }
            else if (Items[i].Type == ItemType.Bodywear)
            {
                poraCustomization.UseItemBodywear(Items[i].Alias);
            }
            else if (Items[i].Type == ItemType.Gun)
            {
                poraCustomization.UseItemGunwear(Items[i].Alias);
            }
        }
    }
}
