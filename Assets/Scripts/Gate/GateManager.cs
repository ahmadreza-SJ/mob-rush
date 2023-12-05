using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gate
{
    [Serializable]
    public class GateManager
    {
        public static Action<GateController> GateSubscribed;

        private List<GateController> _gates;

        public static void SubscribeGate(GateController gate)
        {
            GateSubscribed?.Invoke(gate);
        }
        
        public void Initialize()
        {
            _gates = new List<GateController>();
            GateSubscribed += GateSubscribedCallback;
        }

        public void ReInitialize()
        {
            foreach (GateController gate in _gates)
            {
                gate.ReInitialize();
            }
        }

        public void Dispose()
        {
            GateSubscribed -= GateSubscribedCallback;
        }
 
        
        
        private void GateSubscribedCallback(GateController gate)
        {
            gate.Initialize();
            _gates.Add(gate);
        }
    }
}
