
using System.Collections.Generic;
using Entity;
using Entity.ViewAngle;
using Unity.VisualScripting;
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
        public Transform parent;
        #endregion

        #region Private Variables
        SpriteRenderer render;
        BoxCollider2D col;
        Vector2 mouse;
        float angle;
        Transform rot;
        SectorCollider sector;
        #endregion

        #region Static Methods
        public static DamageArea Init(Transform parent, DamageAreaShape shape, float width, float length) {
            GameObject obj = new GameObject("DamageArea");
            GameObject rot = new GameObject("rot");

            rot.transform.SetParent(parent);
            rot.transform.localPosition = Vector3.zero;

            obj.transform.SetParent(rot.transform);
            obj.transform.localPosition = Vector3.zero;

            var render = obj.AddComponent<SpriteRenderer>();
            DamageArea da = obj.AddComponent<DamageArea>();

            da.width = width;
            da.length = length;

            da.parent = parent;

            da.rot = rot.transform;

            if (shape == DamageAreaShape.Square) {
                render.sprite = Resources.Load<Sprite>("dummy/white_box");

                var col = obj.AddComponent<BoxCollider2D>();
                col.isTrigger = true;   
                col.size = Vector2.one;
            } else if (shape == DamageAreaShape.FanShaped) {
                SectorCollider sector = obj.AddComponent<SectorCollider>();
                sector.targetLayer = LayerMask.NameToLayer("monster");
                sector.obstacleLayer = LayerMask.NameToLayer("wall");
                sector.radius = length;
                sector.angle = (width * 180f) / (Mathf.PI * length);
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

                mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                angle = Mathf.Atan2(mouse.y - parent.position.y, mouse.x - parent.position.x) * Mathf.Rad2Deg;
                rot.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
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
        public void BecomeRed(bool val) {
            Color @color = render.color;

            if (val) {
                Color col = Color.red;
                col.a = @color.a;

                render.color = col;
            } else {
                Color col = Color.white;
                col.a = @color.a;

                render.color = col;
            }
        }

        public void Hide() {
            isShowing = false;
        }
    }
}