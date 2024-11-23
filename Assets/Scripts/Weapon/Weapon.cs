using Entity.Player;
using UnityEngine;

namespace System.Weapon {
    public abstract class Weapon : MonoBehaviour
    {
        public SpriteRenderer render;
        public PlayerController attatcher;
        
        void Awake() {
            render = GetComponent<SpriteRenderer>();
        }
        void Update() {
            OnUpdateTick();
        }
        public void Attatch(PlayerController player) {
            transform.SetParent(player.transform);

            attatcher = player;

            transform.localPosition = Vector2.zero;
        }
        public abstract void OnAttatch();
        public abstract void OnMouseLeftDown();
        public abstract void OnMouseLeftkUp();
        public abstract void OnMouseRightDown();
        public abstract void OnMouseRightkUp();

        public virtual void OnUpdateTick(){}
    }
}