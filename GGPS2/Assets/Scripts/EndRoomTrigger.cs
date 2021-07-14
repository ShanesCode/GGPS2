using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomTrigger : MonoBehaviour
{
    [SerializeField] private int roomNumber;
    public event EventHandler<OnTriggerEventArgs> OnTrigger;
    public class OnTriggerEventArgs : EventArgs
    {
        public int roomNumber;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnTriggerEventArgs e = new OnTriggerEventArgs
            {
                roomNumber = roomNumber
            };

            OnTrigger?.Invoke(this, e);
        }
    }
}
