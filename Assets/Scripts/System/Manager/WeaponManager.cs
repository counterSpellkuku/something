using System.Collections.Generic;
using System.Weapon;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance {get; private set;}
    public List<Weapon> weapons = new();
    void Awake()
    {
        Instance = this;
    }

    public static Weapon FindById(string id) {
        return Instance?.weapons.Find((v)=>v.Id == id);
    }
}
