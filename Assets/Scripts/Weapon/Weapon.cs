using Entity.Player;
using UnityEngine;

namespace System.Weapon {
    public abstract class Weapon : MonoBehaviour
    {
        public SpriteRenderer render;
        public PlayerController attatcher;
        public Collider2D col;
        public ContactFilter2D filter;

        Collider2D[] results;
        
        
        void Awake() {
            render = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();
        }
        void Update() {
            OnUpdateTick();

            if (attatcher == null) {
                FindingPlayer();
            }
        }

        void FindingPlayer() {
            results = new Collider2D[10];

            int count = Physics2D.OverlapCollider(col, filter, results);

            if (count > 0) {
                for (int i = 0; i < count; i++) {
                    Collider2D col_ = results[i];
                    
                    PlayerController pl = col_.GetComponent<PlayerController>();
                    if (pl != null) {
                        Attatch(pl);

                        break;
                    }
                }
            }
        }
        public void Attatch(PlayerController player) {
            transform.SetParent(player.transform);

            attatcher = player;
            player.heldWeapon = this;

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