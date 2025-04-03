using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    [System.Serializable]
    public class SpellData
    {
        public string spellName;
        public GameObject powerUpObject;  // Reference to the power-up GameObject
    }

    public List<SpellData> spellPowerUps = new List<SpellData>();  // List of all power-ups

    private Dictionary<string, bool> activeSpells = new Dictionary<string, bool>();
    private Dictionary<string, SphereCollider> spellColliders = new Dictionary<string, SphereCollider>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Store sphere colliders for each power-up
        foreach (SpellData spell in spellPowerUps)
        {
            if (spell.powerUpObject != null)
            {
                SphereCollider collider = spell.powerUpObject.GetComponent<SphereCollider>();
                if (collider != null)
                {
                    spellColliders[spell.spellName] = collider;
                }
                else
                {
                    Debug.LogError($"Power-up '{spell.spellName}' is missing a SphereCollider!");
                }
            }
        }
    }

    public void ActivateSpell(string spellName)
    {
        if (!activeSpells.ContainsKey(spellName))
            activeSpells.Add(spellName, true);
        else
            activeSpells[spellName] = true;

        Debug.Log($"Activated Spell: {spellName}");
    }

    public bool IsSpellActive(string spellName)
    {
        return activeSpells.ContainsKey(spellName) && activeSpells[spellName];
    }
}
