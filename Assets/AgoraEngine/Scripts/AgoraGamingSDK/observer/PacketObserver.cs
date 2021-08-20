using System;
using AOT;

namespace agora_gaming_rtc
{
    public abstract class IPacketObserver : IRtcEngineNative
	{

        public abstract int RegisterPacketObserver();

        public abstract int UnRegisterPacketObserver();
    }

    /** The definition of PacketObserver.
     */
    public sealed class PacketObserver : IPacketObserver
    {
         /** Occurs when the local user receives an audio packet.
         * 
         * @param packet The received audio packet. See Packet.
         * 
         * @return
         * - true: The audio packet is received successfully.
         * - false: The audio packet is discarded.
         */
        public delegate bool OnReceiveAudioPacketHandler(Packet packet);
        public OnReceiveAudioPacketHandler _OnReceiveAudioPacket;

         /** Occurs when the local user receives a video packet.
         * 
         * @param packet The received video packet. See Packet.
         * 
         * @return
         * - true: The video packet is received successfully.
         * - false: The video packet is discarded.
         */
        public delegate bool OnReceiveVideoPacketHandler(Packet packet);
        public OnReceiveVideoPacketHandler _OnReceiveVideoPacket;

         /** Occurs when the local user sends a video packet.
         * 
         * @param packet The sent video packet. See Packet.
         * 
         * @return
         * - true: The video packet is sent successfully.
         * - false: The video packet is discarded.
         */
        public delegate bool OnSendVideoPacketHandler(Packet packet);
        public OnSendVideoPacketHandler _OnSendVideoPacket;

         /** Occurs when the local user sends an audio packet.
         * 
         * @param packet The sent audio packet. See Packet.
         * 
         * @return
         * - true: The audio packet is sent successfully.
         * - false: The audio packet is discarded.
         */
        public delegate bool OnSendAudioPacketHandler(Packet packet);
        public OnSendAudioPacketHandler _OnSendAudioPacket;

        private static IRtcEngine _irtcEngine = null;
        public static PacketObserver _packetObserver = null;

        private PacketObserver(IRtcEngine irtcEngine)
        {
            _irtcEngine = irtcEngine;
        }

        public static PacketObserver GetInstance(IRtcEngine irtcEngine)
        {
            if (_packetObserver == null)
                _packetObserver = new PacketObserver(irtcEngine);

            return _packetObserver;
        }

        public static void ReleaseInstance()
		{
			_packetObserver = null;
		}

        public void SetEngine(IRtcEngine irtcEngine)
        {
            _irtcEngine = irtcEngine;
        }

        /** Registers a packet observer.
         * 
         * The Agora RTC SDK allows your application to register a packet observer to receive callbacks for voice or video packet transmission.
         * 
         * @note
         * - The size of the packet sent to the network after processing should not exceed 1200 bytes, otherwise, the packet may fail to be sent.
         * - Ensure that both receivers and senders call this method, otherwise, you may meet undefined behaviors such as no voice and black screen.
         * - When you use CDN live streaming, capturing or storage functions, Agora doesn't recommend calling this method.
         * - Call this method before joining a channel.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int RegisterPacketObserver()
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            IRtcEngineNative.initEventOnPacketCallback(OnReceiveAudioPacketCallback, OnReceiveVideoPacketCallback, OnSendAudioPacketCallback, OnSendVideoPacketCallback);
            return IRtcEngineNative.registerPacketObserver();
        }
        
         /** UnRegisters the packet observer.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int UnRegisterPacketObserver()
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            int rc = IRtcEngineNative.unRegisterPacketObserver();
            IRtcEngineNative.initEventOnPacketCallback(null, null, null, null);
            return rc;
        }

        [MonoPInvokeCallback(typeof(EngineEventOnReceiveAudioPacketHandler))]
        private static bool OnReceiveAudioPacketCallback(IntPtr buffer, IntPtr size)
        {
            if (_irtcEngine != null && _packetObserver != null && _packetObserver._OnReceiveAudioPacket != null)
            {
                Packet packet = new Packet();
                packet.buffer = buffer;
                packet.size = size;
                return _packetObserver._OnReceiveAudioPacket(packet);
            }
            return true;
        }

        [MonoPInvokeCallback(typeof(EngineEventOnReceiveVideoPacketHandler))]
        private static bool OnReceiveVideoPacketCallback(IntPtr buffer, IntPtr size)
        {
            if (_irtcEngine != null && _packetObserver != null && _packetObserver._OnReceiveVideoPacket != null)
            {
                Packet packet = new Packet();
                packet.buffer = buffer;
                packet.size = size;
                return _packetObserver._OnReceiveVideoPacket(packet);
            }
            return true;
        }

        [MonoPInvokeCallback(typeof(EngineEventOnSendAudioPacketHandler))]
        private static bool OnSendAudioPacketCallback(IntPtr buffer, IntPtr size)
        {
            if (_irtcEngine != null && _packetObserver != null && _packetObserver._OnSendAudioPacket != null)
            {
                Packet packet = new Packet();
                packet.buffer = buffer;
                packet.size = size;
                return _packetObserver._OnSendAudioPacket(packet);
            }
            return true;
        }

        [MonoPInvokeCallback(typeof(EngineEventOnSendVideoPacketHandler))]
        private static bool OnSendVideoPacketCallback(IntPtr buffer, IntPtr size)
        {
            if (_irtcEngine != null && _packetObserver != null && _packetObserver._OnSendVideoPacket != null)
            {
                Packet packet = new Packet();
                packet.buffer = buffer;
                packet.size = size;
                return _packetObserver._OnSendVideoPacket(packet);
            }
            return true;
        }
    }
}