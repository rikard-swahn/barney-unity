using net.gubbi.goofy.item;
using UnityEngine;

namespace net.gubbi.goofy.input {
    public interface MouseHandler {

        void handleLeftClick (Vector3 mousePos, ItemType selectedItem);
        void handleMouseOver (ItemType selectedItem);

    }
}