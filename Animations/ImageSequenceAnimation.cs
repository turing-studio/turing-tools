using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turing.Tools.Animations
{
    public class ImageSequenceAnimation : MonoBehaviour
    {
        public Action onCompleteAction;
        
        public bool Reverse { get; set; }
        
        [SerializeField] private List<Texture2D> _sequence;
        [SerializeField] private float _fps;
        [SerializeField] private Renderer _renderer;
        
        private Material _material;
        private float _waitTime;
        private int _index;
        private static readonly int _rootsTexture = Shader.PropertyToID("_Texture");

        private Coroutine _updateAnimation;

        private void Start()
        {
            _material = _renderer.material;
            _waitTime = 1 / _fps;
        }

        private IEnumerator UpdateAnimation()
        {
            if (Reverse)
            {
                _index = _sequence.Count;
                while (_index > 0)
                {
                    yield return new WaitForSeconds(_waitTime);
                    _index--;
                    _material.SetTexture(_rootsTexture, _sequence[_index]);
                }
                
                onCompleteAction?.Invoke();
                yield break;
            }
            
            while (_index < _sequence.Count - 1)
            {
                yield return new WaitForSeconds(_waitTime);
                _index++;
                _material.SetTexture(_rootsTexture, _sequence[_index]);
            }

            onCompleteAction?.Invoke();
        }

        public void Activate()
        {
            if(_updateAnimation != null)
                StopCoroutine(_updateAnimation);
                
            _updateAnimation = StartCoroutine(UpdateAnimation());
        }
    }
}
