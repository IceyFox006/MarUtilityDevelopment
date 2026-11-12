using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace MarUtility.Notifications
{
    public class Notification : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text _text;

        [SerializeField, Required]
        private Image _icon;

        [SerializeField, ReadOnly]
        private NotificationData curNotification;

        #region GS
        public NotificationData CurNotification 
        { 
            get => curNotification; 
            set
            {
                curNotification = value;
                _text.text = curNotification.Text;
                _text.color = curNotification.TextColor;
                _icon.sprite = curNotification.Icon;
            }
        }
        #endregion
    }
}
