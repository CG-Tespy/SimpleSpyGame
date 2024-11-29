using CGT.Utils;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CGT
{
    public class VisualFader3D : MonoBehaviour
    {
        [SerializeField] protected GameObject _hasVisuals;
        [SerializeField] protected bool _retainShadows = true;

        protected virtual void Awake()
        {
            _renderers = _hasVisuals.GetComponentsInChildren<Renderer>();
            foreach (var rendererEl in _renderers)
            {
                _materials.AddRange(rendererEl.materials);
            }

            foreach (var matEl in _materials)
            {
                _materialOrigColors.Add(matEl, matEl.color);
            }
        }

        protected IList<Renderer> _renderers;
        protected IList<Material> _materials = new List<Material>();
        protected IDictionary<Material, Color> _materialOrigColors = new Dictionary<Material, Color>();

        public virtual void FadeOut(float duration)
        {
            FadeOut(duration, out _fadeOutProcess);
        }

        protected IEnumerator _fadeOutProcess;

        public virtual void FadeOut(float duration, out IEnumerator process)
        {
            CancelRunningProcesses();
            _fadeOutProcess = process = FadeOutCoroutine(duration);
            StartCoroutine(_fadeOutProcess);
        }

        protected virtual void CancelRunningProcesses()
        {
            if (_fadeOutProcess != null)
            {
                StopCoroutine(_fadeOutProcess);
            }

            if (_fadeInProcess != null)
            {
                StopCoroutine(_fadeInProcess);
            }
        }

        protected IEnumerator _fadeInProcess;

        protected virtual IEnumerator FadeOutCoroutine(float duration)
        {
            foreach (var matEl in _materials)
            {
                MakeTransparencyWorkFor(matEl);

                if (matEl.HasProperty("_Color"))
                {
                    matEl.DOColor(Color.clear, duration);
                }
            }

            yield return new WaitForSeconds(duration);
            _fadeOutProcess = null;
        }

        protected virtual void MakeTransparencyWorkFor(Material mat)
        {
            mat.SetInt("_SrcBlend", _fadeOutSrcBlend);
            mat.SetInt("_DstBlend", _fadeOutDstBlend);
            mat.SetInt("_ZWrite", _fadeOutZWrite);
            mat.SetInt("_Surface", 1);
            mat.renderQueue = _fadeOutRenderQueue;

            mat.SetShaderPassEnabled("DepthOnly", _fadeOutDepthOnly);
            mat.SetShaderPassEnabled("SHADOWCASTER", _retainShadows);
            mat.SetOverrideTag("RenderType", "Transparent");
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        protected static int _fadeOutSrcBlend = (int)BlendMode.SrcAlpha,
            _fadeOutDstBlend = (int)BlendMode.OneMinusSrcAlpha,
            _fadeOutZWrite = 0,
            _fadeOutSurface = 1,
            _fadeOutRenderQueue = (int)RenderQueue.Transparent;

        protected static bool _fadeOutDepthOnly = false;

        public virtual void FadeIn(float duration)
        {
            FadeIn(duration, out _fadeInProcess);
        }

        public virtual void FadeIn(float duration, out IEnumerator process)
        {
            CancelRunningProcesses();
            _fadeInProcess = process = FadeInCoroutine(duration);
            StartCoroutine(_fadeInProcess);
        }

        protected virtual IEnumerator FadeInCoroutine(float duration)
        {
            foreach (var matEl in _materials)
            {
                if (matEl.HasProperty("_Color"))
                {
                    Color origCol = _materialOrigColors[matEl];
                    matEl.DOColor(origCol, duration).
                        OnComplete(() => SetToBeProperlyOpaque(matEl));
                    
                }
            }

            yield return new WaitForSeconds(duration);
        }

        protected virtual void SetToBeProperlyOpaque(Material mat)
        {
            mat.SetInt("_SrcBlend", _fadeOutSrcBlend);
            mat.SetInt("_DstBlend", _fadeOutDstBlend);
            mat.SetInt("_ZWrite", _fadeInZWrite);
            mat.SetInt("_Surface", _fadeInSurface);
            mat.renderQueue = _fadeInRenderQueue;

            mat.SetShaderPassEnabled("DepthOnly", _fadeInDepthOnly);
            mat.SetShaderPassEnabled("SHADOWCASTER", _fadeOutRetainShadows);
            mat.SetOverrideTag("RenderType", "Opaque");

            // DISable instead of ENable 
            mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        protected int _fadeInSrcBlend = (int)BlendMode.One,
            _fadeInDstBlend = (int)BlendMode.Zero,
            _fadeInZWrite = 1,
            _fadeInSurface = 0,
            _fadeInRenderQueue = (int) RenderQueue.Geometry;

        protected bool _fadeInDepthOnly = true,
            _fadeOutRetainShadows = true;
    }
}