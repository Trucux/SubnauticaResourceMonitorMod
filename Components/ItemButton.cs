using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ResourceMonitor.Components
{
    /**
     * Component that will be added onto the item button.
     */
    public class ItemButton : OnScreenButton, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public int Amount { set; get; }
        private TechType type = TechType.None;
        private RawImage rawImage;

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
            rawImage.color = Options.Current.ItemButtonBackgroundColor;
        }

        public TechType Type
        {
            set
            {
                if (Options.Current.AllowSelectingItemsFromMonitor)
                    HoverText = "Take " + Language.main.Get(value);
                else
                    HoverText = Language.main.Get(value);
                
                type = value;
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (Options.Current.AllowSelectingItemsFromMonitor && IsHovered && ResourceMonitorDisplay != null && ResourceMonitorDisplay.ResourceMonitorLogic != null && type != TechType.None)
            {
                ResourceMonitorDisplay.ResourceMonitorLogic.AttemptToTakeItem(type);
            }
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (IsHovered)
            {
                rawImage.color = Options.Current.ItemButtonHoverColor;
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            rawImage.color = Options.Current.ItemButtonBackgroundColor;
        }
    }
}
