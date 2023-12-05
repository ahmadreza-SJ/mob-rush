using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mob;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gate
{
    public class GateController : MonoBehaviour
    {
        
        public static Action<GateController, MobController, int> MobPassed;

        [SerializeField] private TMP_Text factorTxt;
        [SerializeField] [Range(2, 100)] private int multiplyFactor;
        [SerializeField] [Range(0.5f, 10)] private int forgetMobIgnoranceSeconds = 1;

        private HashSet<MobController> _mobsToIgnore;

        private void OnValidate()
        {
            factorTxt.text = $"x {multiplyFactor}";
        }

        private void Start()
        {
            
            GateManager.SubscribeGate(this);
        }


        public void Initialize()
        {
            _mobsToIgnore = new HashSet<MobController>();
        }

        public void ReInitialize()
        {
            _mobsToIgnore.Clear();
        }

        public void IgnoreMob(MobController mob)
        {
            _mobsToIgnore.Add(mob);
        }
        
        public void IgnoreMob(List<MobController> mob)
        {
            _mobsToIgnore.AddRange(mob);
        }

        private async UniTask ForgetMobIgnorance(MobController mob)
        {
            await UniTask.WaitForSeconds(forgetMobIgnoranceSeconds);
            _mobsToIgnore.Remove(mob);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // I had 2 tags to compare, and also hat to use GetComponent at last. so I preferred to use TryGetComponent performance wise
            if (other.TryGetComponent(out MobController mobController))
            {
                if (_mobsToIgnore.Contains(mobController))
                {
                    return;
                }
                
                MobPassed?.Invoke(this, mobController, multiplyFactor);       
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out MobController mobController))
            {
                ForgetMobIgnorance(mobController).Forget();
            }
        }
        
        
    }
}
