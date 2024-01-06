using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Attached to any object that should be detected during eye-tracking
/// </summary>
public class FocusItem : MonoBehaviour
{
    [SerializeField] private string itemLabel;

    /// <summary>
    ///     Defaults the label for this item as the item name given in
    ///     the unity editor
    /// </summary>
    public void Reset()
    {
        itemLabel = this.transform.name;
    }

    /// <summary>
    /// Used to obtain the name of the object currently being looked at
    /// </summary>
    /// <returns>Name of the object being looked at</returns>
    public string GetItemLabel()
    {
        return itemLabel;
    }
}
