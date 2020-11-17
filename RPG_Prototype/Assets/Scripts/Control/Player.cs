using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.Resources;
using System;
using UnityEngine.AI;

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
        [SerializeField] float maxNavMeshRayDistance = 1f;
        [SerializeField] float maxNavMeshPathLength = 40f;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            ProcessPlayerInteractions();
        }

        private void ProcessPlayerInteractions()
        {
            if (InteractingWithUI()) return;
            if (health.IsDead())
            {
                SetMouseCursorType(MouseCursorType.None);
                return;
            }
            if (InteractWithRaycastable()) return;
            if (MovementInteraction()) return;
            SetMouseCursorType(MouseCursorType.None);
        }

        private bool InteractingWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithRaycastable()
        {
            RaycastHit[] hits = SortRaycastHitsByDistance();
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

        private RaycastHit[] SortRaycastHitsByDistance()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRay());
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            return hits;
        }
     
        private bool MovementInteraction()
        {         
            Vector3 target;
            bool hasHit = RaycastToNavMesh(out target);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<MovementHandler>().StartMoveAction(target, 1f);
                }
                SetMouseCursorType(MouseCursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastToNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshRayDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            // calculating distance from player to raycast hit point on navmesh to handle paths that are too long
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath || path.status != NavMeshPathStatus.PathComplete) return false;
            if (CalculatePathLength(path) > maxNavMeshPathLength) return false;

            return true;
        }

        private float CalculatePathLength(NavMeshPath path)
        {
            float sum = 0;
            if (path.corners.Length < 2)
            {
                return sum;
            }
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                sum += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return sum;
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
