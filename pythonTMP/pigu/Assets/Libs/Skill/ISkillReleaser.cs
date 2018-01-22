using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillReleaser
{
    void Attack();
    void Skill(int index);
    void Play(string aniName);
    void PlayTrigger(string aniName);
}
