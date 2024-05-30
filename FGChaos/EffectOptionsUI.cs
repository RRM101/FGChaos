﻿using BepInEx;
using FGChaos.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;
using UniverseLib.UI.Widgets;

namespace FGChaos
{
    public class EffectOptionsUI : PanelBase
    {
        public EffectOptionsUI(UIBase owner) : base(owner)
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public static EffectOptionsUI instance;

        public override string Name => "Effect Options";
        public override int MinWidth => 300;
        public override int MinHeight => 500;
        public override Vector2 DefaultAnchorMin => new(0.25f, 0.25f);
        public override Vector2 DefaultAnchorMax => new(0.75f, 0.75f);
        HashSet<string> EffectIDs = new HashSet<string>();
        Dictionary<string, Toggle> effectToggles = new Dictionary<string, Toggle>();
        string[] disabledEffectIDs = new string[] {};

        protected override void ConstructPanelContent()
        {
            Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MinWidth);
            Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MinHeight);
            Dragger.OnEndResize();

            if (File.Exists($"{Paths.PluginPath}/FGChaos/disabledeffects.txt"))
            {
                disabledEffectIDs = File.ReadAllLines($"{Paths.PluginPath}/FGChaos/disabledeffects.txt");
            }

            UIFactory.CreateScrollView(ContentRoot, "Effect ID Scroll View", out GameObject content, out AutoSliderScrollbar autoScrollbar, new Color(0.07f, 0.07f, 0.07f, 1));
            UIFactory.SetLayoutElement(content);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(content, spacing: 2, padLeft: 5);

            GameObject buttonRow = UIFactory.CreateHorizontalGroup(ContentRoot, "Buttons", true, false, true, true, 4, bgColor: new Color(0.07f, 0.07f, 0.07f, 1));
            ButtonRef saveButton = UIFactory.CreateButton(buttonRow, "Save Button", "Save");
            UIFactory.SetLayoutElement(saveButton.Component.gameObject, minHeight: 25, minWidth: 300, flexibleWidth: 0, flexibleHeight: 0);
            saveButton.OnClick += Save;

            ChaosPluginBehaviour.instance.RunCoroutine(WorkAroundForInvisibleSlider(autoScrollbar.Slider.GetComponent<Mask>()));

            foreach (Effect effect in EffectList.effects)
            {
                EffectIDs.Add(effect.ID);
            }

            foreach (string effectID in EffectIDs)
            {
                GameObject effectRow = UIFactory.CreateHorizontalGroup(content, "effect row", false, false, true, true, 2, bgColor: new Color(0.07f, 0.07f, 0.07f, 1));
                GameObject effectToggle = UIFactory.CreateToggle(effectRow, "effect toggle", out Toggle toggle, out _);
                Text effectText = UIFactory.CreateLabel(effectRow, "effect", effectID);
                UIFactory.SetLayoutElement(effectText.gameObject, minHeight: 20);

                if (disabledEffectIDs.Contains(effectID))
                {
                    toggle.isOn = false;
                }

                effectToggles.Add(effectText.text, toggle);
            }
        }

        protected override void OnClosePanelClicked()
        {
            Owner.Enabled = false;
            CursorManager.Instance.OnApplicationFocus(true);
        }

        IEnumerator WorkAroundForInvisibleSlider(Mask mask)
        {
            yield return new WaitForEndOfFrame();
            mask.OnEnable();
        }

        void Save()
        {
            List<string> disabledEffectIDs = new();

            foreach (string effectID in effectToggles.Keys)
            {
                if (!effectToggles[effectID].isOn)
                {
                    disabledEffectIDs.Add(effectID);
                }
            }

            File.WriteAllLines($"{Paths.PluginPath}/FGChaos/disabledeffects.txt", disabledEffectIDs.ToArray());

            ChaosPluginBehaviour.DisableEffects();
        }
    }
}
