using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace kfutils {

    /// <summary>
    /// This script opens doors and similar hinged objects by rotating them and then moving the door (etc.) such
    /// that the edge retains the same position as the hinge.  This is to deal with the fact that Unity rotates
    /// around the center of the geometry not its origin.
    ///
    /// For this to work properly both the hinge and the edge transform must have the same world position and a local
    /// position of (0, 0, 0).  As a result, the hinge area should be determined during mesh creation in the modelling
    /// software (Blender, Maya, etc.) rather than the Unity editor for the best results.  More specifically, it must be
    /// inline with the meshes object origin along the axis being rotated around.  Not setting this up properly will cause
    /// the rotated object to race off the screen into the realm where its position causes engine errors.  It must rotate 
    /// around the objects origin.
    /// </summary>
    public class OneHingeOpener : MonoBehaviour {
        public enum Axes {
            X,
            Y,
            Z
        }

        [SerializeField] Transform hinge; // Point to rotate around
        [SerializeField] Transform edge;  // Edge connected to hinge; should be child of the rotated game object
        [SerializeField] Axes axisOfRotation = Axes.Y;
        [SerializeField] float closedAngle = 0f;
        [SerializeField] float openAngle = -85f;
        [SerializeField] float timeToOpen = 1f;
        [SerializeField] bool relativeAngles;

        private bool moving;
        private bool open;
        private Quaternion closedQ;
        private Quaternion openQ;
        private float t, startT;


        // Start is called before the first frame update
        void Start() {
            Vector3 closedEuler = Vector3.zero;
            Vector3 openEuler = Vector3.back;
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
            if (relativeAngles) {
                closedEuler += transform.localEulerAngles;
                openEuler += transform.localEulerAngles;
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
                transform.localRotation = Quaternion.Slerp(closedQ, openQ, t);
                Vector3 displacement = edge.position - hinge.position;
                transform.position += displacement;
                moving = (t < 1f);
            }
        }


        private IEnumerator Closing() {
            while (moving) {
                yield return new WaitForFixedUpdate();
                t = Mathf.Clamp((Time.fixedTime - startT) / timeToOpen, 0f, 1f);
                transform.localRotation = Quaternion.Slerp(openQ, closedQ, t);
                Vector3 displacement = edge.position - hinge.position;
                transform.position += displacement;
                moving = (t < 1f);
            }
        }
    }

}
