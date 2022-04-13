/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Zenject;

namespace Sanctuary.Modules.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class Collider2DDrag : MonoBehaviour
    {
        // -------------

        float startPosX;
        float startPosY;
        bool isBeingHeld;

        [SerializeField] Transform toMove;

        void Update()
        {
            // -------------
            
            if (isBeingHeld)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                transform.localPosition = new Vector3(
                    mousePos.x - startPosX, 
                    mousePos.y - startPosY, 
                    0);
            }
            
            // -------------
        }

        // * A collider on the same gameobject is required for this event to fire
        void OnMouseDown()
        {
            // -------------

            // * This means drag will happen with the left mouse button
            // TODO Update this to be done using settings
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                startPosX = mousePos.x - transform.localPosition.x;
                startPosY = mousePos.y - transform.localPosition.y;

                isBeingHeld = true;
            }

            // -------------
        }

        // * A collider on the same gameobject is required for this event to fire
        void OnMouseUp()
        {
            // -------------
            
            isBeingHeld = false;
            
            // -------------
        }

        // -------------
    }
}
/* maria aurelia at 02 December 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */