using System;

namespace FingerprintLibrary
{
    using FingerprintLibrary.model;
    using System.Drawing;
    using System.IO;

    public class FingerPrintEvent
    {
        #region Fields
        Device accessor = new Device();
        bool isFingerprintScanDone = false;
        bool isFingerDetected = false;
        Bitmap fingerImage = null;
        #endregion

        #region Properties
        /// <summary>
        /// Property : Result if fingerprint scan has been completed.
        /// </summary>
        public bool isScanDone => isFingerprintScanDone;

        /// <summary>
        /// Property : Result if fingerprint scan has been completed.
        /// </summary>
        public bool fingerDetected => isFingerDetected;

        /// <summary>
        /// Property : Result of scanned fingerprint.
        /// </summary>
        public Bitmap fingerprintImageScanned => fingerImage;
        #endregion

        #region Methods
        public void scanFingerprint()
        {
            using (var device = accessor.AccessFingerprintDevice())
            {
                device.SwitchLedState(false, false);

                device.FingerDetected += (sender, eventArgs) =>
                {
                    Console.WriteLine("Finger Detected!");
                    isFingerDetected = true;

                    device.SwitchLedState(true, false);

                    // Save fingerprint to temporary folder
                    var fingerprint = device.ReadFingerprint();
                    var tempFile = Path.GetTempFileName();
                    var tmpBmpFile = Path.ChangeExtension(tempFile, "bmp");
                    fingerImage = (Bitmap) Image.FromFile(tmpBmpFile);
                    fingerprint.Save(tmpBmpFile);

                    Console.WriteLine("Saved to " + tmpBmpFile);
                    isFingerprintScanDone = true;
                };

                device.FingerReleased += (sender, eventArgs) =>
                {
                    Console.WriteLine("Finger Released!");
                    isFingerDetected = false;
                    device.SwitchLedState(false, true);
                };

                Console.WriteLine("FingerprintDevice Opened");

                device.StartFingerDetection();
                device.SwitchLedState(false, true);

                Console.ReadLine();

                Console.WriteLine("Exiting...");

                device.SwitchLedState(false, false);
            }
        }

        #endregion
    }
}
