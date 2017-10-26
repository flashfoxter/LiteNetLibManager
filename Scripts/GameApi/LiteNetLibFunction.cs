﻿using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

namespace LiteNetLibHighLevel
{
    public struct SyncFunctionInfo
    {
        public uint objectId;
        public int behaviourIndex;
        public ushort functionId;
        public SyncFunctionInfo(uint objectId, int behaviourIndex, ushort functionId)
        {
            this.objectId = objectId;
            this.behaviourIndex = behaviourIndex;
            this.functionId = functionId;
        }
    }

    public class LiteNetLibFunction
    {
        private NetFunctionDelegate callback;

        [ReadOnly, SerializeField]
        protected LiteNetLibBehaviour behaviour;
        public LiteNetLibBehaviour Behaviour
        {
            get { return behaviour; }
        }

        [ReadOnly, SerializeField]
        protected ushort functionId;
        public ushort FunctionId
        {
            get { return functionId; }
        }

        [ReadOnly, SerializeField]
        protected LiteNetLibFunctionParameter[] parameters;
        public LiteNetLibFunctionParameter[] Parameters
        {
            get { return parameters; }
        }

        public LiteNetLibFunction()
        {
            Debug.LogError("LiteNetLibFunction error - You're calling empty constructor.");
        }

        public LiteNetLibFunction(NetFunctionDelegate callback)
        {
            this.callback = callback;
        }

        public LiteNetLibGameManager Manager
        {
            get { return behaviour.Manager; }
        }

        public virtual void OnRegister(LiteNetLibBehaviour behaviour, ushort functionId)
        {
            this.behaviour = behaviour;
            this.functionId = functionId;
        }

        public SyncFunctionInfo GetSyncFunctionInfo()
        {
            return new SyncFunctionInfo(Behaviour.ObjectId, Behaviour.BehaviourIndex, FunctionId);
        }

        public void Deserialize(NetDataReader reader)
        {
            if (Parameters == null || Parameters.Length == 0)
                return;
            for (var i = 0; i < Parameters.Length; ++i)
            {
                Parameters[i].Deserialize(reader);
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            if (Parameters == null || Parameters.Length == 0)
                return;
            for (var i = 0; i < Parameters.Length; ++i)
            {
                Parameters[i].Serialize(writer);
            }
        }

        public virtual void HookCallback()
        {
            callback();
        }

        public void Call(SendOptions sendOptions, params object[] parameterValues)
        {
            for (var i = 0; i < parameterValues.Length; ++i)
            {
                Parameters[i].Value = parameterValues[i];
            }
            Manager.SendNetworkFunction(sendOptions, this);
        }
    }

    public class LiteNetLibFunction<T1> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[1];
            parameters[0] = new T1();
        }

        public override void HookCallback()
        {
            callback(Parameters[0] as T1);
        }
    }

    public class LiteNetLibFunction<T1, T2> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
        where T2 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1, T2> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[2];
            parameters[0] = new T1();
            parameters[1] = new T2();
        }

        public override void HookCallback()
        {
            callback(
                Parameters[0] as T1, 
                Parameters[1] as T2);
        }
    }
    
    public class LiteNetLibFunction<T1, T2, T3> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
        where T2 : LiteNetLibFunctionParameter, new()
        where T3 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1, T2, T3> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[3];
            parameters[0] = new T1();
            parameters[1] = new T2();
            parameters[2] = new T3();
        }

        public override void HookCallback()
        {
            callback(
                Parameters[0] as T1, 
                Parameters[1] as T2, 
                Parameters[2] as T3);
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
        where T2 : LiteNetLibFunctionParameter, new()
        where T3 : LiteNetLibFunctionParameter, new()
        where T4 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1, T2, T3, T4> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[4];
            parameters[0] = new T1();
            parameters[1] = new T2();
            parameters[2] = new T3();
            parameters[3] = new T4();
        }

        public override void HookCallback()
        {
            callback(
                Parameters[0] as T1, 
                Parameters[1] as T2, 
                Parameters[2] as T3, 
                Parameters[3] as T4);
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
        where T2 : LiteNetLibFunctionParameter, new()
        where T3 : LiteNetLibFunctionParameter, new()
        where T4 : LiteNetLibFunctionParameter, new()
        where T5 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1, T2, T3, T4, T5> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[5];
            parameters[0] = new T1();
            parameters[1] = new T2();
            parameters[2] = new T3();
            parameters[3] = new T4();
            parameters[4] = new T5();
        }

        public override void HookCallback()
        {
            callback(
                Parameters[0] as T1, 
                Parameters[1] as T2, 
                Parameters[2] as T3, 
                Parameters[3] as T4, 
                Parameters[4] as T5);
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
        where T2 : LiteNetLibFunctionParameter, new()
        where T3 : LiteNetLibFunctionParameter, new()
        where T4 : LiteNetLibFunctionParameter, new()
        where T5 : LiteNetLibFunctionParameter, new()
        where T6 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1, T2, T3, T4, T5, T6> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[6];
            parameters[0] = new T1();
            parameters[1] = new T2();
            parameters[2] = new T3();
            parameters[3] = new T4();
            parameters[4] = new T5();
            parameters[5] = new T6();
        }

        public override void HookCallback()
        {
            callback(
                Parameters[0] as T1,
                Parameters[1] as T2,
                Parameters[2] as T3,
                Parameters[3] as T4,
                Parameters[4] as T5,
                Parameters[5] as T6);
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6, T7> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
        where T2 : LiteNetLibFunctionParameter, new()
        where T3 : LiteNetLibFunctionParameter, new()
        where T4 : LiteNetLibFunctionParameter, new()
        where T5 : LiteNetLibFunctionParameter, new()
        where T6 : LiteNetLibFunctionParameter, new()
        where T7 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[7];
            parameters[0] = new T1();
            parameters[1] = new T2();
            parameters[2] = new T3();
            parameters[3] = new T4();
            parameters[4] = new T5();
            parameters[5] = new T6();
            parameters[6] = new T7();
        }

        public override void HookCallback()
        {
            callback(
                Parameters[0] as T1,
                Parameters[1] as T2,
                Parameters[2] as T3,
                Parameters[3] as T4,
                Parameters[4] as T5,
                Parameters[5] as T6,
                Parameters[6] as T7);
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6, T7, T8> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
        where T2 : LiteNetLibFunctionParameter, new()
        where T3 : LiteNetLibFunctionParameter, new()
        where T4 : LiteNetLibFunctionParameter, new()
        where T5 : LiteNetLibFunctionParameter, new()
        where T6 : LiteNetLibFunctionParameter, new()
        where T7 : LiteNetLibFunctionParameter, new()
        where T8 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7, T8> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7, T8> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[8];
            parameters[0] = new T1();
            parameters[1] = new T2();
            parameters[2] = new T3();
            parameters[3] = new T4();
            parameters[4] = new T5();
            parameters[5] = new T6();
            parameters[6] = new T7();
            parameters[7] = new T8();
        }

        public override void HookCallback()
        {
            callback(
                Parameters[0] as T1,
                Parameters[1] as T2,
                Parameters[2] as T3,
                Parameters[3] as T4,
                Parameters[4] as T5,
                Parameters[5] as T6,
                Parameters[6] as T7,
                Parameters[7] as T8);
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
        where T2 : LiteNetLibFunctionParameter, new()
        where T3 : LiteNetLibFunctionParameter, new()
        where T4 : LiteNetLibFunctionParameter, new()
        where T5 : LiteNetLibFunctionParameter, new()
        where T6 : LiteNetLibFunctionParameter, new()
        where T7 : LiteNetLibFunctionParameter, new()
        where T8 : LiteNetLibFunctionParameter, new()
        where T9 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[9];
            parameters[0] = new T1();
            parameters[1] = new T2();
            parameters[2] = new T3();
            parameters[3] = new T4();
            parameters[4] = new T5();
            parameters[5] = new T6();
            parameters[6] = new T7();
            parameters[7] = new T8();
            parameters[8] = new T9();
        }

        public override void HookCallback()
        {
            callback(
                Parameters[0] as T1,
                Parameters[1] as T2,
                Parameters[2] as T3,
                Parameters[3] as T4,
                Parameters[4] as T5,
                Parameters[5] as T6,
                Parameters[6] as T7,
                Parameters[7] as T8,
                Parameters[8] as T9);
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : LiteNetLibFunction
        where T1 : LiteNetLibFunctionParameter, new()
        where T2 : LiteNetLibFunctionParameter, new()
        where T3 : LiteNetLibFunctionParameter, new()
        where T4 : LiteNetLibFunctionParameter, new()
        where T5 : LiteNetLibFunctionParameter, new()
        where T6 : LiteNetLibFunctionParameter, new()
        where T7 : LiteNetLibFunctionParameter, new()
        where T8 : LiteNetLibFunctionParameter, new()
        where T9 : LiteNetLibFunctionParameter, new()
        where T10 : LiteNetLibFunctionParameter, new()
    {
        private NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback;

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
        {
            this.callback = callback;
            parameters = new LiteNetLibFunctionParameter[10];
            parameters[0] = new T1();
            parameters[1] = new T2();
            parameters[2] = new T3();
            parameters[3] = new T4();
            parameters[4] = new T5();
            parameters[5] = new T6();
            parameters[6] = new T7();
            parameters[7] = new T8();
            parameters[8] = new T9();
            parameters[9] = new T10();
        }

        public override void HookCallback()
        {
            callback(
                Parameters[0] as T1,
                Parameters[1] as T2,
                Parameters[2] as T3,
                Parameters[3] as T4,
                Parameters[4] as T5,
                Parameters[5] as T6,
                Parameters[6] as T7,
                Parameters[7] as T8,
                Parameters[8] as T9,
                Parameters[9] as T10);
        }
    }
}
