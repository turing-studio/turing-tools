using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Object = System.Object;

namespace Turing.Tools.Extensions
{
    public static class ExtensionMethods
    {
        public static List<T> ToList<T>(this T[] self)
        {
            int size = self.Length;
            List<T> result = new List<T>(size);
            for (int i = 0; i < size; i++)
                result.Add(self[i]);
            return result;
        }

        #region Vectors
        
        public static Vector2 ToVector2(this Vector3 self) => new Vector2(self.x, self.y);
        
        public static Vector3 ToVector3(this Vector2 self) => new Vector3(self.x, self.y, 0f);

        public static bool IsNaN(this Vector2 self) => float.IsNaN(self.x) || float.IsNaN(self.y);

        public static bool IsNaN(this Vector3 self) => float.IsNaN(self.x) || float.IsNaN(self.y) || float.IsNaN(self.z);

        public static Vector2 Inverse(this Vector2 self) => new Vector2(1 / self.x, 1 / self.y);

        public static Vector3 Inverse(this Vector3 self) => new Vector3(1 / self.x, 1 / self.y, 1 / self.y);

        #endregion

        #region Coroutines
        
        public static IEnumerator Timer<T>(this T self, float delay, Action<T> action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke(self);
        }

        /// <summary>
        /// Waits for given number of seconds in unscaled time and then invokes given action
        /// </summary>
        public static IEnumerator UnscaledTimer<T>(this T self, float delay, Action<T> action)
        {
            yield return new WaitForSecondsRealtime(delay);
            action.Invoke(self);
        }

        public static IEnumerator InvokeAfterDelay<T>(this T self, float delay, Action<T> action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke(self);
            yield return null;
        }

        private static IEnumerator WaitAndInvoke(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static IEnumerator InvokeAfterDelay(this MonoBehaviour self, float delay, Action action)
        {
            var enumerator = WaitAndInvoke(delay, action);
            self.StartCoroutine(enumerator);
            return enumerator;
        }

        /// <summary>
        /// Invokes method after time and stops previously invoked coroutine
        /// </summary>
        public static void SafeInvoke(this MonoBehaviour self, ref IEnumerator coroutine, Action action,
            float delay = 0f)
        {
            if (coroutine != null)
                self.StopCoroutine(coroutine);
            coroutine = self.InvokeAfterDelay(delay, action);
        }

        public static void SafeStopCoroutine(this MonoBehaviour self, ref IEnumerator coroutine)
        {
            if (coroutine != null)
                self.StopCoroutine(coroutine);
        }
        
        #endregion

        #region Components
        
        public static T GetComponentOnlyInChildren<T>(this Component self) where T : Component
        {
            foreach (Transform obj in self.transform)
            {
                var comp = obj.GetComponentInChildren<T>();
                if (comp != null)
                    return comp;
            }

            return null;
        }

        public static T GetComponentOnlyInChildren<T>(this GameObject self) where T : Component
            => self.transform.GetComponentOnlyInChildren<T>();

        public static List<T> GetComponentsOnlyInChildren<T>(this Component self)
        {
            var ret = new List<T>();
            foreach (Transform obj in self.transform)
                ret.AddRange(obj.GetComponentsInChildren<T>());
            return ret;
        }
        
        public static List<T> GetComponentsOnlyInChildren<T>(this GameObject self)
            => self.transform.GetComponentsOnlyInChildren<T>();

        public static GameObject[] FindGameObjectsInChildrenWithTag(this Transform self, string tag)
            => self.TraverseThisAndChildren(x => x.CompareTag(tag) ? x.gameObject : null).ToArray();

        public static GameObject[] FindGameObjectsInChildrenWithTag(this GameObject self, string tag)
            => self.transform.FindGameObjectsInChildrenWithTag(tag);

        public static GameObject[] FindGameObjectsInChildrenWithTags(this Transform self, List<string> tags)
            => self.TraverseThisAndChildren(x => x.CompareTags(tags) ? x.gameObject : null).ToArray();

        public static GameObject[] FindGameObjectsInChildrenWithTags(this GameObject self, List<string> tags)
            => self.transform.FindGameObjectsInChildrenWithTags(tags);
        
        public static bool CompareTags(this Transform self, List<string> tags) => tags.Any(self.CompareTag);
        
        public static List<Transform> Children(this Transform self)
        {
            var children = new List<Transform>();
            foreach (Transform tr in self)
                children.Add(tr);
            return children;
        }

        public static Transform GetLastChild(this Transform self)
            => self.childCount > 0 ? self.GetChild(self.childCount - 1) : null;

        public static bool IsLastSibling(this Transform self)
            => self.parent == null || self.GetSiblingIndex() == self.parent.childCount - 1;

        #endregion

        public static void SafeKill(this Tween tween)
        {
            if (!tween.IsActive())
                return;
            tween.Kill();
        }

        public static T Null<T>(this T self) where T : Component => self == null ? null : self;

        public static GameObject Null(this GameObject self) => self == null ? null : self;

        private static List<T> TraverseThisAndChildren<T>(this Transform self, Func<Transform, T> predicate)
        {
            var objs = new List<T>();
            void Add(T x) { if (x != null) objs.Add(x); }
            Add(predicate(self));
            foreach (Transform t in self)
            {
                Add(predicate(t));
                objs.AddRange(t.TraverseThisAndChildren(predicate));
            }

            return objs;
        }
    }
}