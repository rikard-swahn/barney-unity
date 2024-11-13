using Mgl;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.ui.menu.ingame;
using UnityEngine;

public class GameObjectLabelProvider : MonoBehaviour, LabelProvider {

    public string key;
    public bool empty;
    
    public string getLabel() {
        if (empty) {
            return null;
        }
        
        return I18n.Instance.__(!key.isNullOrEmpty() ? key : gameObject.name);        
    }

}