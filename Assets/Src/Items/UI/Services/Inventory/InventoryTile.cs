using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Src.Items.UI.Services.Inventory
{
    [RequireComponent(typeof(Image))]
    public class InventoryTile : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI counterText;
        [SerializeField] private Image tileImage;

        public void SetSpriteAndColor(Sprite sprite, Color color)
        {
            tileImage.sprite = sprite;

            if (sprite == null)
            {
                tileImage.color = Color.white;
                return;
            }
            
            tileImage.color = new Color(color.r, color.g, color.b, 1);
        }

        public void SetStackCount(int count)
        {
            if (count < 2)
            {
                counterText.gameObject.SetActive(false);
                return;
            }

            counterText.gameObject.SetActive(true);
            counterText.text = count.ToString();
        }
    }
}