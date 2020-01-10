﻿using System;
using System.Reflection;
using LiteNetLib;
using LiteNetLib.Utils;

namespace LiteNetLibManager
{
    public enum FunctionReceivers : byte
    {
        Target,
        All,
        Server,
    }

    public class LiteNetLibFunction : LiteNetLibElement
    {
        public readonly Type[] ParameterTypes;
        public readonly object[] Parameters;
        protected Delegate callback;

        public LiteNetLibFunction() : this(0)
        {
        }

        public LiteNetLibFunction(int parameterCount)
        {
            Parameters = new object[parameterCount];
            ParameterTypes = new Type[parameterCount];
        }

        public LiteNetLibFunction(Type[] parameterTypes, Delegate callback)
        {
            Parameters = new object[parameterTypes.Length];
            ParameterTypes = parameterTypes;
            this.callback = callback;
        }

        public LiteNetLibFunction(Delegate callback) : this()
        {
            this.callback = callback;
        }

        public void HookCallback()
        {
            callback.DynamicInvoke(Parameters);
        }

        protected void ServerSendCall(long connectionId, DeliveryMethod deliveryMethod, FunctionReceivers receivers, long targetConnectionId)
        {
            SendingConnectionId = connectionId;
            Manager.ServerSendPacket(connectionId, deliveryMethod, LiteNetLibGameManager.GameMsgTypes.CallFunction, (writer) => SerializeForSend(writer));
        }

        protected void ClientSendCall(DeliveryMethod deliveryMethod, FunctionReceivers receivers, long targetConnectionId)
        {
            Manager.ClientSendPacket(deliveryMethod, LiteNetLibGameManager.GameMsgTypes.CallFunction, (writer) => SerializeForClient(writer, receivers, targetConnectionId));
        }

        protected void SendCall(DeliveryMethod deliveryMethod, FunctionReceivers receivers, long targetConnectionId)
        {
            LiteNetLibGameManager manager = Manager;

            if (manager.IsServer)
            {
                switch (receivers)
                {
                    case FunctionReceivers.Target:
                        if (Identity.IsSubscribedOrOwning(targetConnectionId) && manager.ContainsConnectionId(targetConnectionId))
                            ServerSendCall(targetConnectionId, deliveryMethod, receivers, targetConnectionId);
                        break;
                    case FunctionReceivers.All:
                        foreach (long connectionId in manager.GetConnectionIds())
                        {
                            if (Identity.IsSubscribedOrOwning(connectionId))
                                ServerSendCall(connectionId, deliveryMethod, receivers, targetConnectionId);
                        }
                        if (!Manager.IsClientConnected)
                            HookCallback();
                        break;
                    case FunctionReceivers.Server:
                        HookCallback();
                        break;
                }
            }
            else if (manager.IsClientConnected)
                ClientSendCall(deliveryMethod, receivers, targetConnectionId);
        }

        public void SetParameters(params object[] parameterValues)
        {
            for (int i = 0; i < Parameters.Length; ++i)
            {
                if (i >= parameterValues.Length)
                    break;
                Parameters[i] = parameterValues[i];
            }
        }

        public void Call(DeliveryMethod deliveryMethod, FunctionReceivers receivers, params object[] parameterValues)
        {
            if (!ValidateBeforeAccess())
                return;

            SetParameters(parameterValues);
            SendCall(deliveryMethod, receivers, ConnectionId);
        }

        public void Call(long connectionId, params object[] parameterValues)
        {
            if (!ValidateBeforeAccess())
                return;

            SetParameters(parameterValues);
            SendCall(DeliveryMethod.ReliableOrdered, FunctionReceivers.Target, connectionId);
        }

        public void CallWithoutParametersSet(DeliveryMethod deliveryMethod, FunctionReceivers receivers)
        {
            if (!ValidateBeforeAccess())
                return;

            SendCall(deliveryMethod, receivers, ConnectionId);
        }

        public void CallWithoutParametersSet(long connectionId)
        {
            if (!ValidateBeforeAccess())
                return;

            SendCall(DeliveryMethod.ReliableOrdered, FunctionReceivers.Target, connectionId);
        }

        protected void SerializeForClient(NetDataWriter writer, FunctionReceivers receivers, long connectionId)
        {
            writer.Put((byte)receivers);
            if (receivers == FunctionReceivers.Target)
                writer.PutPackedULong((ulong)connectionId);
            SerializeForSend(writer);
        }

        protected void SerializeForSend(NetDataWriter writer)
        {
            LiteNetLibElementInfo.SerializeInfo(GetInfo(), writer);
            SerializeParameters(writer);
        }

        public void DeserializeParameters(NetDataReader reader)
        {
            if (Parameters == null || Parameters.Length == 0)
                return;
            for (int i = 0; i < Parameters.Length; ++i)
            {
                Parameters[i] = reader.GetValue(ParameterTypes[i]);
            }
        }

        public void SerializeParameters(NetDataWriter writer)
        {
            if (Parameters == null || Parameters.Length == 0)
                return;
            for (int i = 0; i < Parameters.Length; ++i)
            {
                writer.PutValue(ParameterTypes[i], Parameters[i]);
            }
        }
    }

    #region Implement for multiple parameter usages
    public class LiteNetLibFunction<T1> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(1)
        {
            ParameterTypes[0] = typeof(T1);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1> callback) : this()
        {
            this.callback = callback;
        }
    }

    public class LiteNetLibFunction<T1, T2> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(2)
        {
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2> callback) : this()
        {
            this.callback = callback;
        }
    }

    public class LiteNetLibFunction<T1, T2, T3> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(3)
        {
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3> callback) : this()
        {
            this.callback = callback;
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(4)
        {
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4> callback) : this()
        {
            this.callback = callback;
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(5)
        {
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5> callback) : this()
        {
            this.callback = callback;
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(6)
        {
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            ParameterTypes[5] = typeof(T6);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6> callback) : this()
        {
            this.callback = callback;
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6, T7> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(7)
        {
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            ParameterTypes[5] = typeof(T6);
            ParameterTypes[6] = typeof(T7);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7> callback) : this()
        {
            this.callback = callback;
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6, T7, T8> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(8)
        {
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            ParameterTypes[5] = typeof(T6);
            ParameterTypes[6] = typeof(T7);
            ParameterTypes[7] = typeof(T8);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7, T8> callback) : this()
        {
            this.callback = callback;
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(9)
        {
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            ParameterTypes[5] = typeof(T6);
            ParameterTypes[6] = typeof(T7);
            ParameterTypes[7] = typeof(T8);
            ParameterTypes[8] = typeof(T9);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback) : this()
        {
            this.callback = callback;
        }
    }

    public class LiteNetLibFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : LiteNetLibFunction
    {
        public LiteNetLibFunction() : base(10)
        {
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            ParameterTypes[5] = typeof(T6);
            ParameterTypes[6] = typeof(T7);
            ParameterTypes[7] = typeof(T8);
            ParameterTypes[8] = typeof(T9);
            ParameterTypes[9] = typeof(T10);
        }

        public LiteNetLibFunction(NetFunctionDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback) : this()
        {
            this.callback = callback;
        }
    }
    #endregion
}
