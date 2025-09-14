using System.Collections.Generic;
using UnityEngine;

public enum MonsterColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    White,
    Cyan
}

public class Monster : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 2f;
    public List<Vector3> pathPoints; // 路线点
    private int currentPointIndex = 0;
    private Animator animator;

    public MonsterColor monsterColor = MonsterColor.Red; // 怪物颜色属性

    void Start()
    {
        animator = GetComponent<Animator>();
        if (pathPoints != null && pathPoints.Count > 0)
        {
            transform.position = pathPoints[0];
        }
        PlayIdle();
    }

    void Update()
    {
        if (health <= 0)
        {
            PlayDeath();
            return;
        }

        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (pathPoints == null || currentPointIndex >= pathPoints.Count) return;

        PlayMove();

        Vector3 target = pathPoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= pathPoints.Count)
            {
                PlayIdle();
            }
        }
    }

    void PlayIdle()
    {
        if (animator != null)
            animator.Play("Idle");
    }

    void PlayMove()
    {
        if (animator != null)
            animator.Play("Move");
    }

    void PlayDeath()
    {
        if (animator != null)
            animator.Play("Death");
        // 可以在这里销毁怪物或触发其他逻辑
    }

    // 怪物的特殊能力函数（留空，子类可重写）
    public virtual void SpecialAbility()
    {
        // 实现特殊能力
    }

    // 只有攻击颜色与怪物颜色一致时才会受伤
    public void TakeDamage(float amount, MonsterColor attackColor)
    {
        if (attackColor == monsterColor && health > 0)
        {
            health -= amount;
            if (health <= 0)
            {
                PlayDeath();
            }
        }
    }
}