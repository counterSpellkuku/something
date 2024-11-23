using Entity;
using UnityEngine;

public class ShadowCreator {
    public static Animator Generate(BaseEntity entity, Vector2 offset = default){
        if (entity.GetComponent<Animator>() == null) {
            return null;
        }

        GameObject obj = new GameObject("shadow");
        Animator animator = obj.AddComponent<Animator>();
        SpriteRenderer render = obj.AddComponent<SpriteRenderer>();

        obj.transform.SetParent(entity.transform);

        render.sprite = entity.render.sprite;

        render.color = new Color(0, 0, 0, 160f/256);

        obj.transform.localScale = new Vector2(1.2f, 0.6f);
        obj.transform.localPosition = offset;

        animator.runtimeAnimatorController = entity.animator.runtimeAnimatorController;

        return animator;
    }
}