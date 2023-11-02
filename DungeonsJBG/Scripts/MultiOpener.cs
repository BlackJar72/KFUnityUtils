using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace kfutils {

    public class MultiOpener : MonoBehaviour, IDoorOpener {
        [SerializeField] SimpleOpener[] doors;


        public void Activate() {
            foreach(SimpleOpener door in doors)
                door.Activate();
        }


        public void Close() {
            foreach(SimpleOpener door in doors)
                door.Close();
        }


        public void Open() {
            foreach(SimpleOpener door in doors)
                door.Open();
        }

    }

}
