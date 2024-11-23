
using System.Collections.Generic;
using Entity;
using UnityEngine;

public enum DamageAreaShape {
    Square,
    FanShaped,
    Circle
}
namespace Hunger_of_war.Util {
    
    public class DamageArea : MonoBehaviour {
        #region Public Variables
        public float width, length;
        public bool isShowing;
        public List<BaseEntity> casted = new();
        public Vector3 offset;
        #endregion

        #region Private Variables
        SpriteRenderer render;
        BoxCollider2D col;
        #endregion

        #region Static Methods
        public static DamageArea Init(Transform parent, DamageAreaShape shape, float width, float length) {
            GameObject obj = new GameObject("DamageArea");

            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;

            var render = obj.AddComponent<SpriteRenderer>();
            DamageArea da = obj.AddComponent<DamageArea>();

            da.width = width;
            da.length = length;

            if (shape == DamageAreaShape.Square) {
                render.sprite = Resources.Load<Sprite>("Texture2D/DamageArea");

                var col = obj.AddComponent<BoxCollider2D>();
                col.isTrigger = true;   
                col.size = Vector2.one;
            }

            render.sortingLayerName = "damageArea";

            return da;
        }
        #endregion
        
        void Start() {
            render = GetComponent<SpriteRenderer>();
            col = GetComponent<BoxCollider2D>();

            transform.localScale = new Vector3(width, length);
            transform.localPosition = new Vector3(0, length / 2) + offset;
        }

        void Update() {
            Color @color = render.color;
            if (isShowing) {
                transform.localScale = new Vector3(width, length);
                transform.localPosition = new Vector3(0, length / 2) + offset;

                @color.a = 0.3f;
            } else {
                @color.a = 0f;
            }

            render.color = @color;
        }

        void OnTriggerEnter2D(Collider2D col) {
            var dam = col.GetComponent<BaseEntity>();
            if (dam != null) {
                casted.Add(dam);
            }
        }

        void OnTriggerExit2D(Collider2D col) {
            var dam = col.GetComponent<BaseEntity>();
            if (dam != null) {
                casted.Remove(dam);
            }
        }

        public void Show() {
            isShowing = true;
        }

        public void Hide() {
            isShowing = false;
        }
    }
}