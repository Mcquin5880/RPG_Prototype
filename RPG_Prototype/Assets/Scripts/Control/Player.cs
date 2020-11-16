using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using System;

namespace RPG.Control
{
    public class Player : MonoBehaviour
    {
        Health health;      

        [System.Serializable]
        struct CursorMapping
        {
            public MouseCursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractingWithUI()) return;
            if (!health.IsAlive())
            {
                SetMouseCursorType(MouseCursorType.None);
                return;
            }
            if (InteractWithRaycastable()) return;
            //if (CombatInteraction()) return;
            if (MovementInteraction()) return;
            SetMouseCursorType(MouseCursorType.None);
        }

        private bool InteractWithRaycastable()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRay());
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetMouseCursorType(raycastable.GetMouseCursorType());
                        return true;
                    }
                }
            }
            return false;
        }
        /*
        private bool CombatInteraction()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                SetMouseCursorType(MouseCursorType.Combat);
                return true;
            }
            return false;
        }
        */
        private bool MovementInteraction()
        {
            RaycastHit hit;

            if (Physics.Raycast(GetRay(), out hit, 100))
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<MovementHandler>().StartMoveAction(hit.point, 1f);
                }
                SetMouseCursorType(MouseCursorType.Movement);
                return true;
            }
            return false;
        }

        private bool InteractingWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private void SetMouseCursorType(MouseCursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(MouseCursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
