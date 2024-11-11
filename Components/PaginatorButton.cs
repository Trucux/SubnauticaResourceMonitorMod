using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ResourceMonitor.Components
{
    /**
     * Component that will be added onto the paginator buttons. Gives the hover effect and clicking function.
     */
    public class PaginatorButton : OnScreenButton, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public int AmountToChangePageBy = 1;
        private Text text;

        public void Start()
        {
            text = GetComponent<Text>();
            HoverText = text.text;
        }

        public void OnEnable()
        {
            if (text != null)
            {
                text.color = Options.Current.PaginatorStartingColor;
            }
        }

        public override void OnDisable()
        {
            if (text != null)
            {
                text.color = Options.Current.PaginatorStartingColor;
            }
            base.OnDisable();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (IsHovered)
            {
                text.color = Options.Current.PaginatorHoverColor;
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            text.color = Options.Current.PaginatorStartingColor;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (IsHovered)
            {
                ResourceMonitorDisplay.ChangePageBy(AmountToChangePageBy);
            }
        }
    }
}
