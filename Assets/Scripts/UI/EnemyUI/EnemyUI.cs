using Character.Character;
using Character.Stats;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyUI : CharacterUI
    {
        Slider resourceSlider;
        DefaultCharacter npc;

        override protected void Awake()
        {
            base.Awake();
            resourceSlider = GetComponentInChildren<Slider>();
        }

        private void Start()
        {
            npc = GetComponentInParent<DefaultCharacter>();
        }

        override protected void Update()
        {
            base.Update();
            UpdateResourceUnit();
        }

        private void UpdateResourceUnit()
        {
            resourceSlider.maxValue = npc.GetMaxValueOfResource(ResourceType.Health);
            resourceSlider.value = npc.GetCurrentResource(ResourceType.Health);
        }

    }
}