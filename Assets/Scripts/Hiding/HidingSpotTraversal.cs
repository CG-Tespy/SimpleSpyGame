using CGT;
using CGT.Utils;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleSpyGame
{
    public class HidingSpotTraversal : MonoBehaviour
    {
        [SerializeField] protected Vector3 _rotationMultipliers = Vector3.one;
        [SerializeField] protected GameObject _hasVisuals;
        [SerializeField] protected GameObject _teleportationVfxPrefab;

        [Tooltip("How long the vfx sticks around for each poof")]
        [SerializeField] protected float _vfxDuration = 2f;

        [SerializeField] protected VisualFader3D _visualFader;

        protected virtual void Awake()
        {
            _charaController = GetComponentInParent<CharacterController>();
            _agent = GetComponentInParent<NavMeshAgent>();

            _telepStartVfx = Instantiate(_teleportationVfxPrefab).transform;
            _telepEndVfx = Instantiate(_teleportationVfxPrefab).transform;

            _telepStartVfx.position = _telepEndVfx.position = _charaController.transform.position;

            _telepStartVfx.gameObject.SetActive(false);
            _telepEndVfx.gameObject.SetActive(false);

            _vfxWait = new WaitForSeconds(_vfxDuration);

            // To help with the vanishing and reappearing acts
            IList<Renderer> renderers = _hasVisuals.GetComponentsInChildren<Renderer>();
            foreach (var rendererEl in renderers)
            {
                _modelMaterials.AddRange(rendererEl.materials);
            }

            foreach (var matEl in _modelMaterials)
            {
                _matOrigColors.Add(matEl, matEl.color);
            }
        }

        protected CharacterController _charaController;
        protected NavMeshAgent _agent;
        protected Transform _telepStartVfx, _telepEndVfx;
        protected WaitForSeconds _vfxWait;
        protected IList<Material> _modelMaterials = new List<Material>();

        protected IDictionary<Material, Color> _matOrigColors = new Dictionary<Material, Color>();

        public virtual void TraverseTo(Transform hidingSpot, bool poofRightAway = false)
        {
            if (IsTraversing)
            {
                return;
            }

            StartCoroutine(TraversalCoroutine(hidingSpot, poofRightAway));
        }

        protected virtual IEnumerator TraversalCoroutine(Transform hidingSpot, bool teleportToSpot)
        {
            IsTraversing = true;
            _charaController.enabled = false; // <- So physics doesn't get in the way
            
            Vector3 endRot = GetEndRotation(hidingSpot);

            if (teleportToSpot)
            {
                // To avoid displacement issues, we want the agent disabled during
                // the teleportation process
                _agent.enabled = false;
                yield return Vanish();
                Hide(_telepStartVfx);
                yield return ReappearAt(hidingSpot, endRot);
                Hide(_telepEndVfx);
                _agent.enabled = true;
            }
            else
            {
                // We'll want smooth movement on the nav mesh to the hiding spot
                _agent.enabled = true;
                NavMeshPath path = new NavMeshPath();
                
                NavMesh.CalculatePath(_agent.transform.position, hidingSpot.position, NavMesh.AllAreas, path);
                _agent.isStopped = false;
                _agent.SetPath(path);

                Tween rotationTween = _agent.transform.DOLocalRotate(endRot, 0.1f);

                while (_agent.remainingDistance > 0.1f)
                {
                    yield return null;
                }

                yield return rotationTween.WaitForCompletion();
                rotationTween = _agent.transform.DOLocalRotate(endRot, 0.1f);
                //_agent.transform.forward = hidingSpot.forward;
                // ^ Since both DoTween and DoFaceTowards don't get the right result by themselves

                Debug.Log($"Agent done going to hiding spot {hidingSpot.name}");
            }

            Debug.Log($"Arrived at target hiding spot: {hidingSpot.name}");

            IsTraversing = false;
            _agent.isStopped = true;
            _charaController.enabled = true;
        }

        public virtual bool IsTraversing { get; protected set; }

        protected Vector3 GetEndRotation(Transform hidingSpot)
        {
            Vector3 endRotEuler = hidingSpot.localEulerAngles;
            endRotEuler.x *= _rotationMultipliers.x;
            endRotEuler.y *= _rotationMultipliers.y;
            endRotEuler.z *= _rotationMultipliers.z;
            return endRotEuler;
        }

        protected virtual IEnumerator Vanish()
        {
            ShowAtAgentPos(_telepStartVfx);
            _visualFader.FadeOut(_vfxDuration);
            yield return _vfxWait;
        }

        protected virtual void ShowAtAgentPos(Transform vfx)
        {
            vfx.position = _agent.transform.position;
            vfx.gameObject.SetActive(true);
        }

        protected virtual IEnumerator ReappearAt(Transform hidingSpot, Vector3 endRot)
        {
            _charaController.transform.position = hidingSpot.position;
            //_agent.transform.eulerAngles = endRot;
            _agent.transform.forward = hidingSpot.forward;
            ShowAtAgentPos(_telepEndVfx);
            _visualFader.FadeIn(_vfxDuration);
            yield return _vfxWait;
        }

        protected virtual void Hide(Transform vfx)
        {
            vfx.gameObject.SetActive(false);
        }


    }
}