using UnityEngine;

namespace UnityVolumeRendering
{
    [ExecuteInEditMode]
    public class SlicingPlane : MonoBehaviour
    {
        public Vector3 Scale { get { return _scale; } set { _scale = value; } }
        [SerializeField] Vector3 _scale;
        [SerializeField] Transform _rotator;
        [SerializeField] Transform _parent;
        private MeshRenderer meshRenderer;

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            meshRenderer.sharedMaterial.SetMatrix("_AxisRotationMatrix", _rotator.localToWorldMatrix);
            meshRenderer.sharedMaterial.SetMatrix("_parentInverseMat", _parent.localToWorldMatrix);
            meshRenderer.sharedMaterial.SetMatrix("_planeMat", Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one)); // TODO: allow changing scale
            meshRenderer.sharedMaterial.SetVector("_Scale", new Vector3(1/_scale.x,1/_scale.y,1/_scale.z));
        }
    }
}
