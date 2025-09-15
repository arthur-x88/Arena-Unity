using System.Collections.Generic;
using Core;
using JetBrains.Annotations;
using UnityEngine;

namespace Client
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField, UsedImplicitly] private ActionBarSettingsContainer container;
        [SerializeField, UsedImplicitly] private List<ButtonSlot> buttonSlots;
        [SerializeField, UsedImplicitly] private ActionBarSettings actionBarSettings;

        public void Initialize()
        {
            for (int i = 0; i < buttonSlots.Count; i++)
                buttonSlots[i].Initialize();
        }

        public void Denitialize()
        {
            buttonSlots.ForEach(buttonSlot => buttonSlot.Denitialize());
        }

        public void DoUpdate(float deltaTime)
        {
            foreach (var slot in buttonSlots)
                slot.DoUpdate();
        }

        public void ModifyContent(ClassType classType)
        {
            ActionBarSettings appliedSettings = container.SettingsByClassSlot.TryGetValue((classType, actionBarSettings.SlotId), out ActionBarSettings classSettings)
                ? classSettings
                : actionBarSettings;

            // Special handling for Warrior: only show action bar 1, hide 2-6
            bool hasContent = classType == ClassType.Warrior
                ? actionBarSettings.SlotId == 1 && appliedSettings.DefaultPresets.Count > 0
                : appliedSettings.DefaultPresets.Count > 0;

            // Show/hide the action bar based on whether it has content
            gameObject.SetActive(hasContent);

            if (hasContent)
            {
                for (int i = 0; i < buttonSlots.Count; i++)
                    buttonSlots[i].ButtonContent.UpdateContent(appliedSettings.ActiveButtonPresets[i]);
            }
        }
    }
}