using Nautilus.Json;
using Nautilus.Options.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceMonitor
{
    [Menu("Resource Monitor")]
    public class Options : ConfigFile
    {
        public static Options Current => Plugin.Options;


        [Toggle("AllowSelectingItemsFromMonitor", Tooltip = "Allows the player to click an item and automatically retreive it from storage.")]
        public Boolean AllowSelectingItemsFromMonitor = true;

        [Choice("PaginatorStartingColor", Tooltip = "Color for the page selection.")]
        public Color PaginatorStartingColor = Color.white;

        [ColorPicker("PaginatorHoverColor", Tooltip = "Color for the page selection hover.")]
        public Color PaginatorHoverColor = new Color(0.07f, 0.38f, 0.7f, 1f);

        [Slider("MaxInteractionDistance", Min = 1f, Max = 20f, DefaultValue = 2.5f, Tooltip = "Max Interaction Distance.")]
        public float MaxInteractionDistance = 2.5f;

        [Slider("MaxInteractionIdlePageDistance", Min = 1f, Max = 20f, DefaultValue = 5f, Tooltip = "Applies for idle mode.")]
        public float MaxInteractionIdlePageDistance = 5f;

        [Toggle("EnableIdle", Tooltip = "If disabled, resource monitor may require manual overriding to display accurate numbers.")]
        public Boolean EnableIdle = true;

        [Slider("IdleTime", Min = 1f, Max = 60f, DefaultValue = 20f, Tooltip = "How many seconds beyond idle page distance until the idle screen appears.")]
        public float IdleTime = 20f;

        [Slider("IdleTimeRandomnessLowBound", Min = 0f, Max = 20f, DefaultValue = 1f, Tooltip = "Low bounduary for idle time randomness.")]
        public float IdleTimeRandomnessLowBound = 1f;

        [Slider("IdleTimeRandomnessHighBound", Min = 0f, Max = 20f, DefaultValue = 10f, Tooltip = "High bounduary for idle time randomness.")]
        public float IdleTimeRandomnessHighBound = 10f;

        [Slider("IdleScreenColorTransitionTime", Min = 1f, Max = 20f, DefaultValue = 2f, Tooltip = "Idle screen color transition time")]
        public float IdleScreenColorTransitionTime = 2f;

        [Slider("IdleScreenColorTransitionRandomnessHighBound", Min = 0f, Max = 20f, DefaultValue = 2f, Tooltip = "High bounduary for color transition randomness")]
        public float IdleScreenColorTransitionRandomnessHighBound = 0f;

        [Slider("IdleScreenColorTransitionRandomnessLowBound", Min = 0f, Max = 20f, DefaultValue = 0f, Tooltip = "Low bounduary for color transition randomness")]
        public float IdleScreenColorTransitionRandomnessLowBound = 0f;

        [ColorPicker("ItemButtonBackgroundColor", Tooltip = "Background colors for item buttons")]
        public Color ItemButtonBackgroundColor = new Color(0.07843138f, 0.3843137f, 0.7058824f);

        [ColorPicker("ItemButtonHoverColor", Tooltip = "Background colors for item buttons - hover")]
        public Color ItemButtonHoverColor = new Color(0.07843137f, 0.1459579f, 0.7058824f);

    }
}