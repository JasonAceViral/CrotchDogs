using UnityEngine;
using System.Collections;

namespace AceViral {

    public delegate void EventNearestObject(Transform trans,int index);
    public delegate void EventObjectSelected(Transform trans,int index);
    public class ScrollPage : MonoBehaviour
    {
        public Transform[] selectableObjects;
        public BoxCollider draggableRegion;

        public bool EnableScroll = true;

        public float swipeResistance = 50f;
        public float objSpacing = 2.8f;

        private bool hasStarted = false;
        private float distanceToScroll = 0.1f;
        private float timeToScroll = 1f;
        private bool touchActive, touchAccepted, touchHasScrolled;
        private float previousLayerXPos;
        private Vector3 previousTouchPosition;
        private Vector3 touchOrigin;
        private float targetXPosition;
    	private float animationOriginXPosition;
        private double animationTime = 0.5f;
        private double animateTimePlayed;
        private int previousClosestObj;
        private double touchTimeBegun;

        private bool autoScrollActive = false;
        private float lastSwipeSpeed = 0;
        private float speedBeforeSnap = 0.02f;
        private float maxOverScroll = 2.0f;
        private float minManualScrollPoint, maxManualScrollPoint;
        private float minScrollPoint, maxScrollPoint;
    	private float onEnableTime;


        public event EventNearestObject nearestObjectChanged;

        protected virtual void NearestObjectChanged(Transform trans, int index)
        {
            if (nearestObjectChanged != null)
            {
                nearestObjectChanged(trans, index);
            }
        }

        public event EventObjectSelected objectSelected;

        protected virtual void ObjectSelected(Transform trans, int index)
        {
            if (objectSelected != null)
            {
                objectSelected(trans, index);
            }
        }

        public void MoveToIndex(int index, bool animated)
        {
            if (!hasStarted)
                Start();

            autoScrollActive = false;
            touchAccepted = false;
            targetXPosition = -selectableObjects[index].localPosition.x;

            if (animated)
            {
                animationOriginXPosition = transform.localPosition.x;
                animateTimePlayed = 0;
            }
            else
            {
                animationOriginXPosition = targetXPosition;
                animateTimePlayed = animationTime + 1f; // set it higher than animation time to prevent auto animation kicking in
                transform.localPosition = new Vector3(-selectableObjects[index].localPosition.x, transform.localPosition.y, transform.localPosition.z);
            }
        }

    	void OnEnable()
    	{
    		onEnableTime = Time.time;
    	}

        // Use this for initialization
        void Start()
        {
            if (hasStarted)
                return;

            hasStarted = true;

            for (int i = 0; i < selectableObjects.Length; i++)
            {
                selectableObjects[i].localPosition = new Vector3(objSpacing * i, 0, 0);
            }

            touchActive = false;
            previousLayerXPos = 0f;
            previousClosestObj = 0;
            minManualScrollPoint = -selectableObjects[0].localPosition.x;
            maxManualScrollPoint = -selectableObjects[selectableObjects.Length-1].localPosition.x;
            minScrollPoint = minManualScrollPoint + maxOverScroll;
            maxScrollPoint = maxManualScrollPoint - maxOverScroll;
        }

        // Update is called once per frame
        void Update()
        {
            if (!EnableScroll)
                return;

    		if (Time.time - onEnableTime < 0.2f)
    			return;

    		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_METRO
            if (Input.GetMouseButton(0))
            {
                TouchUpdateAtPosition(Input.mousePosition);
            }
            else if (touchActive)
            {
                TouchUp();
            }
            #else
            if( Input.touchCount > 0 ) 
            {
                Touch t = Input.GetTouch (0);
                TouchUpdateAtPosition(t.position);
    		}
            else if(touchActive)
            {
                TouchUp();
            }
            #endif

            if (autoScrollActive)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + lastSwipeSpeed, transform.localPosition.y, transform.localPosition.z);

                if (lastSwipeSpeed < 0)
                {
                    lastSwipeSpeed += swipeResistance * Time.deltaTime;
                    if (lastSwipeSpeed > 0)
                        lastSwipeSpeed = 0;
                }
                else if(lastSwipeSpeed > 0)
                {
                    lastSwipeSpeed -= swipeResistance * Time.deltaTime;
                    if (lastSwipeSpeed < 0)
                        lastSwipeSpeed = 0;
                }

                if (Mathf.Abs(lastSwipeSpeed) <= speedBeforeSnap)
                {
                    EndAutoScroll();
                }
                else if (lastSwipeSpeed < 0)
                {
                    if (transform.localPosition.x < maxScrollPoint)
                    {
                        EndAutoScroll();
                    }
                }
                else if (transform.localPosition.x > minScrollPoint)
                {
                    EndAutoScroll();
                }
            }

            // Update auto scrolling
            else if (!touchActive && animateTimePlayed <= animationTime)
            {
                animateTimePlayed += Time.deltaTime;

                float percProg = (float)(animateTimePlayed / animationTime);

                if (percProg <= 1f)
                {
                    float sinVal = Mathf.Sin((percProg / 2f) * Mathf.PI);
                    float newPos = animationOriginXPosition + (targetXPosition - animationOriginXPosition) * sinVal;
                    transform.localPosition = new Vector3(newPos, transform.localPosition.y, transform.localPosition.z);
                }
                else transform.localPosition = new Vector3(targetXPosition, transform.localPosition.y, transform.localPosition.z);
            }

            int closest = FindClosestObjectIndex();
            if (closest != previousClosestObj)
            {
                previousClosestObj = closest;
                NearestObjectChanged(selectableObjects[closest], closest);
            }
        }

        void EndAutoScroll()
        {
            autoScrollActive = false;
            touchAccepted = false;

            animationOriginXPosition = transform.localPosition.x;
            animateTimePlayed = 0f;
            targetXPosition = -selectableObjects[FindClosestObjectIndex()].localPosition.x;
        }

        void TouchUpdateAtPosition(Vector3 pos)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);

            if (!touchActive)
            {
                touchActive = true;
                touchAccepted = false;
                touchHasScrolled = false;
                if (!CheckTouchIsWithinBounds(pos))
                    return;

                previousLayerXPos = gameObject.transform.localPosition.x;
                touchOrigin = worldPos;
                touchTimeBegun = Time.time;
                touchAccepted = true;
            }

            if (!touchAccepted)
                return;

            lastSwipeSpeed = (worldPos.x - previousTouchPosition.x);
            lastSwipeSpeed = Mathf.Clamp(lastSwipeSpeed, -0.65f, 0.65f);
            previousTouchPosition = worldPos;
            float moveDistance = worldPos.x - touchOrigin.x;

            if (!touchHasScrolled)
            {
                if (Mathf.Abs(moveDistance) >= distanceToScroll)
                    touchHasScrolled = true;
                else if (Time.time - touchTimeBegun >= timeToScroll)
                    touchHasScrolled = true;
            }

            if (touchHasScrolled)
            {
                float toSetXPos = previousLayerXPos + moveDistance;

                float overHang = 0;

                if (toSetXPos < maxManualScrollPoint)
                {
                    overHang = (toSetXPos - maxManualScrollPoint) * 0.5f;
                }
                else if (toSetXPos > minManualScrollPoint)
                {
                    overHang = (toSetXPos - minManualScrollPoint) * 0.5f;
                }

                gameObject.transform.localPosition = new Vector3(toSetXPos - overHang, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
            }
        }

        void TouchUp()
        {
            touchActive = false;
            if (touchAccepted)
            {
                if (!touchHasScrolled)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(previousTouchPosition));
                    RaycastHit[] hits = Physics.RaycastAll(ray, 100);

                    for (int i = 0; i < hits.Length; i++)
                    {
                        int hitId = hits[i].transform.gameObject.GetInstanceID();

                        for (int j = 0; j < selectableObjects.Length; j++)
                        {
                            int instanceId = selectableObjects[j].gameObject.GetInstanceID();

                            if (instanceId == hitId)
                            {
                                ObjectSelected(selectableObjects[j], j);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    autoScrollActive = true;
                }
            }
        }

        bool CheckTouchIsWithinBounds(Vector3 pos)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);

            RaycastHit[] hits = Physics.RaycastAll(ray, 100);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.GetInstanceID() == draggableRegion.gameObject.GetInstanceID())
                {
                    return true;
                }
            }
            return false;
        }

        int FindClosestObjectIndex()
        {
            float posOfLayer = -transform.localPosition.x;
            float closestDistance = 999999f;
            int indexOfClosest = 0;

            for (int i = 0; i < selectableObjects.Length; i++)
            {
                float distance = Mathf.Abs(selectableObjects[i].localPosition.x - posOfLayer);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    indexOfClosest = i;
                }
            }
            return indexOfClosest;
        }
    }
}
