/*
    THIS GATEWAY CODE IS PROVIDED FOR SAMPLE PURPOSE ONLY.
    THIS CODE IS SPECIALLY DEVELOPED FOR THIS PROJECT ONLY.

    I'm developing a robust gateway module that will enable communication between RPi2
    and Arduino with following arduino capabilities directly from RPi2:
        + pinMode
        + digitalWrite
        + digitalRead
        + analogWrite
        + analogRead
        + Servo Library

    It means you can control and access ADC, PWM and Serial pins resides on Arduino directly from RPi2.

    I have already developed gateway module that is available with project:
    https://www.hackster.io/AnuragVasanwala/windows-10-iot-core-hydroflyer

    But at current stage, the gateway module is in early stage and there are several issues with stability of Arduino.
    Thus, I'm improving module. In advance, I'm going to provide gateway module for Arduino Wiring Sketch and Universal Windows App.

    Stay tuned :)
*/
using System;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Windows_10_IoT_Core___UltraSonic_Distance_Mapper__UWP_.Library.Communication
{
    static class ArduinoGateway
    {
        private static string AQS;
        private static DeviceInformationCollection DIS;

        /// <summary>
        /// Moves servo to specified angle
        /// </summary>
        /// <param name="Angle">Angle to be moved</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task<byte[]> MoveServo(byte Angle)
        {
            byte[] Response = new byte[1];

            /* Gateway's I2C SLAVE address */
            int SlaveAddress = 64;              // 0x40

            try
            {
                // Initialize I2C
                var Settings = new I2cConnectionSettings(SlaveAddress);
                Settings.BusSpeed = I2cBusSpeed.StandardMode;

                if (AQS == null || DIS == null)
                {
                    AQS = I2cDevice.GetDeviceSelector("I2C1");
                    DIS = await DeviceInformation.FindAllAsync(AQS);
                }

                using (I2cDevice Device = await I2cDevice.FromIdAsync(DIS[0].Id, Settings))
                {
                    /* Send byte to the gateway to move servo at specified position */
                    Device.Write(new byte[] { Angle });
                }
            }
            catch (Exception)
            {
                // SUPPRESS ANY ERROR
            }
            
            /* Return dummy or ZERO */
            return Response;
        }
    }
}
