using System.Collections.Generic;
using System.Linq;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using UnityEngine;

public class ItemSprites : MonoBehaviour {

	private Dictionary<ItemType, Sprite> itemSpriteMap;

	private void Awake () {
		itemSpriteMap = GetComponents<InventoryItemSprite>().ToDictionary(s => s.type, s => s.sprite);
	}

	public Sprite getSprite(ItemType itemType) {
		return itemSpriteMap [itemType];
	}
	
}
