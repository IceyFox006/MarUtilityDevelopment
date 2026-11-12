/*
 * Marlow Greenan
 * Created: 11/12/2026
 * Last Updated: 11/12/2026
 */
using MarUtility.ExecutionManagement;
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace MarUtility.Notifications
{
    public class NotificationMaster : Manager
    {
        //NOTIFICATION
        [SerializeField, BoxGroup("Notification")]
        private GameObject _notificationPrefab;
        [SerializeField, BoxGroup("Notification")]
        private Transform _notificationHolder;
        [SerializeField, BoxGroup("Notification"), Tooltip("The max amount of notifications that can be spawned in at once.")]
        private int _maxNotifications = 20;
        [SerializeField, BoxGroup("Notification")]
        private Dictionary<string, NotificationData> _notificationLibrary;

        //DATA
        [SerializeField, ReadOnly]
        private List<Notification> notificationPool;

        //SIMULATION
        [SerializeField, BoxGroup("Simulation")]
        private string _simNotification;

        //Creates a new notification.
        public void Notify(string ndID)
        {
            if (!_notificationLibrary.ContainsKey(ndID))
            {
                DebugMessages.LibraryDoesNotContain("Notificationmaster", ndID);
                return;
            }
            Notify(_notificationLibrary[ndID]);
        }

        public void Notify(NotificationData nd)
        {
            Notification n;
            if (notificationPool.Count < _maxNotifications) //Spawns a new notification if the notification pool is not full;
                n = SpawnNotification();
            else //Updates the oldest notification with new information.
                n = notificationPool[notificationPool.Count - 1];

            n.CurNotification = nd;
        }

        //Spawns a new notification.
        public Notification SpawnNotification()
        {
            Notification n = Instantiate(_notificationPrefab, _notificationHolder).GetComponent<Notification>();
            notificationPool.Add(n);
            return n;
        }

        #region Inspector
        [Button]
        private void Notify()
            => Notify(_simNotification);
        #endregion
    }

    [Serializable]
    public class NotificationData
    {
        [SerializeField]
        private string _text;
        [SerializeField]
        private Color _textColor = new Color(0, 0, 0, 100);
        [SerializeField]
        private Sprite _icon;

        #region GS
        public string Text { get => _text; }
        public Color TextColor { get => _textColor; }
        public Sprite Icon { get => _icon; }
        #endregion
    }
}
