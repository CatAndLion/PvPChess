using Assets.Scripts.ChessBoardElement;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// Скрипт поля доски
    /// </summary>
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class BoardSpaceScript : MonoBehaviour
    {
        private RectTransform rectTransform;

        /// <summary>
        /// Картинка
        /// </summary>
        private Image image;

        /// <summary>
        /// Позиция на доске
        /// </summary>
        public BoardCoord Coordiantes;

        /// <summary>
        /// Видимость
        /// </summary>
        public bool IsVisible
        {
            get { return image.enabled; }
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }

        /// <summary>
        /// Установить позицию на доске
        /// </summary>
        /// <param name="coordinates"></param>
        public void SetCoordinates(BoardCoord coordinates)
        {
            Coordiantes = coordinates;
        }

        /// <summary>
        /// Установить размер
        /// </summary>
        /// <param name="size"></param>
        public void SetSize(float size)
        {
            rectTransform.sizeDelta = new Vector2(size, size);
        }

        /// <summary>
        /// Установить цвет картинки
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            image.color = color;
        }

        /// <summary>
        /// Установить видимость
        /// </summary>
        /// <param name="visible">видимость</param>
        public void SetVisibility(bool visible)
        {
            image.enabled = visible;
        }
    }
}
