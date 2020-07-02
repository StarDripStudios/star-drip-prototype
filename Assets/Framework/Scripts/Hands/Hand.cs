using UnityEngine;

namespace Framework
{
    public class Hand : MonoBehaviour
    {
        public Handedness hand;

        private HandConstraints _constraints = HandConstraints.Free;

        public HandConstraints Constraints => _constraints;
    }
}
