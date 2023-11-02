using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace kfutils {

    public class SimpleOpener : MonoBehaviour, IDoorOpener {
        public enum Axes {
            X,
            Y,
            Z
        }
        public enum Side {
            left,
            right
        }

        [SerializeField] Transform hinge;
        [SerializeField] Axes axisOfRotation = Axes.Y;
        [SerializeField] Side side = Side.left;
        [SerializeField] float closedAngle = 0f;
        [SerializeField] float openAngle = -85f;
        [SerializeField] float timeToOpen = 1f;

        private bool moving;
        private bool open;
        private Quaternion closedQ;
        private Quaternion openQ;
        private float t, startT;


        // Start is called before the first frame update
        void Start() {
            Vector3 closedEuler = Vector3.zero;
            Vector3 openEuler = Vector3.back;
            if(side == Side.right) {
                openAngle = openAngle - ((openAngle - closedAngle) * 2);
            }
            switch (axisOfRotation) {
                case Axes.X:
                    closedEuler = new Vector3(closedAngle, 0, 0);
                    openEuler = new Vector3(openAngle, 0, 0);
                    break;
                case Axes.Y:
                    closedEuler = new Vector3(0, closedAngle, 0);
                    openEuler = new Vector3(0, openAngle, 0);
                    break;
                case Axes.Z:
                    closedEuler = new Vector3(0, 0, closedAngle);
                    openEuler = new Vector3(0, 0, openAngle);
                    break;
            }
            closedQ = Quaternion.Euler(closedEuler);
            openQ = Quaternion.Euler(openEuler);
        }


        public void Open() {
            open = true;
            moving = true;
            startT = Time.time;
            StartCoroutine(Opening());
        }


        public void Close() {
            open = false;
            moving = true;
            startT = Time.time;
            StartCoroutine(Closing());
        }


        public void Activate() {
            if (moving) return;
            else if (open) Close();
            else Open();
        }


        private IEnumerator Opening() {
            while (moving) {
                yield return new WaitForFixedUpdate();
                t = Mathf.Clamp((Time.fixedTime - startT) / timeToOpen, 0f, 1f);
                hinge.transform.localRotation = Quaternion.Slerp(closedQ, openQ, t);
                moving = (t < 1f);
            }
        }


        private IEnumerator Closing() {
            while (moving) {
                yield return new WaitForFixedUpdate();
                t = Mathf.Clamp((Time.fixedTime - startT) / timeToOpen, 0f, 1f);
                hinge.transform.localRotation = Quaternion.Slerp(openQ, closedQ, t);
                moving = (t < 1f);
            }
        }
    }

}
