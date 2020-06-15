namespace FingerprintLibrary.model
{
    using System;
    public class Device
    {
        public FingerprintDevice AccessFingerprintDevice()
        {
            var handle = LibScanAPI.ftrScanOpenDevice();

            if (handle != IntPtr.Zero)
            {
                return new FingerprintDevice(handle);
            }

            throw new Exception("Cannot open device");
        }

        public CardReader AccessCardReader()
        {
            var handle = LibMifareApi.ftrMFOpenDevice();

            if (handle != IntPtr.Zero)
            {
                return new CardReader(handle);
            }

            throw new Exception("Cannot open device");
        }
    }
}
