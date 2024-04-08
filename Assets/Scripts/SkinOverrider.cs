using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinOverrider : MonoBehaviour
{
    [SerializeField] private string idSkin;
    // Start is called before the first frame update

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Animator animator = GetComponent<Animator>();
            AnimatorOverrideController aoc = Resources.Load(idSkin + "/aoc", typeof(AnimatorOverrideController)) as AnimatorOverrideController;
            animator.runtimeAnimatorController = aoc;
        }
    }
}
