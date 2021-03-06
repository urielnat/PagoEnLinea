﻿
using System;
using System.Threading.Tasks;
using AVFoundation;
using CoreGraphics;
using Foundation;
using FullCameraPage;
using FullCameraPage.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CameraPage), typeof(CameraPageRenderer))]
namespace FullCameraPage.iOS
{
    public class CameraPageRenderer : PageRenderer
    {
        AVCaptureSession captureSession;
        AVCaptureDeviceInput captureDeviceInput;
        AVCaptureStillImageOutput stillImageOutput;
        AVCapturePhotoSettings capturePhoto;

        UIPaintCodeButton takePhotoButton;
        UIPaintCodeButton cancelPhotoButton;
        UIPaintCodeButton rectangleLT, rectangleLB, rectangleRT, rectangleRB;


        UIView liveCameraStream;


        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupUserInterface();
            SetupEventHandlers();
            await AuthorizeCameraUse();
            SetupLiveCameraStream();
        }

        private void SetupUserInterface()
        {
            var centerButtonX = View.Bounds.GetMidX() - 35f;
            var bottomButtonY = View.Bounds.Bottom - 85;
            var topRightX = View.Bounds.Right - 65;
            var topLeftX = View.Bounds.X + 25;
            var topButtonY = View.Bounds.Top + 25;
            var buttonWidth = 70;
            var buttonHeight = 70;

            liveCameraStream = new UIView()
            {
                Frame = new CGRect(0f, 0f, View.Bounds.Width, View.Bounds.Height)
            };

            rectangleLT = new UIPaintCodeButton(DrawRectLT){
                Frame = new CGRect(View.Bounds.GetMidX()-200,View.Bounds.GetMaxY() / 3,60,60)
                    
            };

            rectangleLB = new UIPaintCodeButton(DrawRectLB)
            {
                Frame = new CGRect(View.Bounds.GetMidX() - 200, View.Bounds.GetMidY()+50, 60, 60)

            };


            rectangleRT = new UIPaintCodeButton(DrawRectRT)
            {
                Frame = new CGRect(View.Bounds.GetMidX() + 140, View.Bounds.GetMaxY() / 3, 60, 60)

            };



            rectangleRB = new UIPaintCodeButton(DrawRectRB)
            {
                Frame = new CGRect(View.Bounds.GetMidX() + 140, View.Bounds.GetMidY() + 50, 60, 60)

            };

            takePhotoButton = new UIPaintCodeButton(DrawTakePhotoButton)
            {
                Frame = new CGRect(centerButtonX, bottomButtonY, buttonWidth, buttonHeight)
            };

            cancelPhotoButton = new UIPaintCodeButton(DrawCancelPictureButton)
            {
                Frame = new CGRect(topLeftX, topButtonY, 37, 37)
            };

           
            View.Add(liveCameraStream);
            View.Add(rectangleLT);
            View.Add(rectangleLB);
            View.Add(rectangleRT);
            View.Add(rectangleRB);
            View.Add(takePhotoButton);

            View.Add(cancelPhotoButton);
        }


        private void SetupEventHandlers()
        {

            cancelPhotoButton.TouchUpInside += (s, e) =>
            {
                (Element as FullCameraPage.CameraPage).Cancel();
            };

            takePhotoButton.TouchUpInside += async (s, e) =>
            {
                var data = await CapturePhoto();
                UIImage imageInfo = new UIImage(data);

                (Element as FullCameraPage.CameraPage).SetPhotoResult(data.ToArray(),
                                                            (int)imageInfo.Size.Width,
                                                            (int)imageInfo.Size.Height);
            };
        }

        public AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
        {
            var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

            foreach (var device in devices)
            {
                if (device.Position == orientation)
                {
                    return device;
                }
            }
            return null;
        }

        public async Task<NSData> CapturePhoto()
        {
            //stillImageOutput.HighResolutionStillImageOutputEnabled = true;
            
            var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
          
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            return jpegImageAsNsData;
        }

        public void SetupLiveCameraStream()
        {
            captureSession = new AVCaptureSession();

            var videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
            {
                Frame = liveCameraStream.Bounds
            };
            liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

            var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);



            ConfigureCameraForDevice(captureDevice);
           

            captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            stillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };

            captureSession.AddOutput(stillImageOutput);
            captureSession.AddInput(captureDeviceInput);
            captureSession.StartRunning();
        }


        public void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            var photoSettings = AVCapturePhotoSettings.Create();
            photoSettings.FlashMode = AVCaptureFlashMode.Auto;
            photoSettings.IsHighResolutionPhotoEnabled = true;

            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            }
            else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
            {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            }
            else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
            {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }

        }


        public void CapturePhoto(AVCapturePhotoSettings settings)
        {
           

        }

        public async Task AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }


        #region Drawings

        private void DrawTakePhotoButton(CGRect frame)
        {
            var color = UIColor.White;

            var bezierPath = new UIBezierPath();
            bezierPath.MoveTo(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.08333f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.27302f * frame.Width, frame.GetMinY() + 0.15053f * frame.Height), new CGPoint(frame.GetMinX() + 0.41628f * frame.Width, frame.GetMinY() + 0.08333f * frame.Height), new CGPoint(frame.GetMinX() + 0.33832f * frame.Width, frame.GetMinY() + 0.10803f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.08333f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.15883f * frame.Width, frame.GetMinY() + 0.22484f * frame.Height), new CGPoint(frame.GetMinX() + 0.08333f * frame.Width, frame.GetMinY() + 0.35360f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.91667f * frame.Height), new CGPoint(frame.GetMinX() + 0.08333f * frame.Width, frame.GetMinY() + 0.73012f * frame.Height), new CGPoint(frame.GetMinX() + 0.26988f * frame.Width, frame.GetMinY() + 0.91667f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.91667f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.73012f * frame.Width, frame.GetMinY() + 0.91667f * frame.Height), new CGPoint(frame.GetMinX() + 0.91667f * frame.Width, frame.GetMinY() + 0.73012f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.08333f * frame.Height), new CGPoint(frame.GetMinX() + 0.91667f * frame.Width, frame.GetMinY() + 0.26988f * frame.Height), new CGPoint(frame.GetMinX() + 0.73012f * frame.Width, frame.GetMinY() + 0.08333f * frame.Height));
            bezierPath.ClosePath();
            bezierPath.MoveTo(new CGPoint(frame.GetMinX() + 1.00000f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 1.00000f * frame.Height), new CGPoint(frame.GetMinX() + 1.00000f * frame.Width, frame.GetMinY() + 0.77614f * frame.Height), new CGPoint(frame.GetMinX() + 0.77614f * frame.Width, frame.GetMinY() + 1.00000f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.00000f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.22386f * frame.Width, frame.GetMinY() + 1.00000f * frame.Height), new CGPoint(frame.GetMinX() + 0.00000f * frame.Width, frame.GetMinY() + 0.77614f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.19894f * frame.Width, frame.GetMinY() + 0.10076f * frame.Height), new CGPoint(frame.GetMinX() + 0.00000f * frame.Width, frame.GetMinY() + 0.33689f * frame.Height), new CGPoint(frame.GetMinX() + 0.07810f * frame.Width, frame.GetMinY() + 0.19203f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.00000f * frame.Height), new CGPoint(frame.GetMinX() + 0.28269f * frame.Width, frame.GetMinY() + 0.03751f * frame.Height), new CGPoint(frame.GetMinX() + 0.38696f * frame.Width, frame.GetMinY() + 0.00000f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 1.00000f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.77614f * frame.Width, frame.GetMinY() + 0.00000f * frame.Height), new CGPoint(frame.GetMinX() + 1.00000f * frame.Width, frame.GetMinY() + 0.22386f * frame.Height));
            bezierPath.ClosePath();
            color.SetFill();
            bezierPath.Fill();
            UIColor.Black.SetStroke();
            bezierPath.LineWidth = 1.0f;
            bezierPath.Stroke();

            var ovalPath = UIBezierPath.FromOval(new CGRect(frame.GetMinX() + NMath.Floor(frame.Width * 0.12500f + 0.5f), frame.GetMinY() + NMath.Floor(frame.Height * 0.12500f + 0.5f), NMath.Floor(frame.Width * 0.87500f + 0.5f) - NMath.Floor(frame.Width * 0.12500f + 0.5f), NMath.Floor(frame.Height * 0.87500f + 0.5f) - NMath.Floor(frame.Height * 0.12500f + 0.5f)));
            color.SetFill();
            ovalPath.Fill();
            UIColor.Black.SetStroke();
            ovalPath.LineWidth = 1.0f;
            ovalPath.Stroke();
        }

        private void DrawCancelPictureButton(CGRect frame)
        {
            var color2 = UIColor.White;

            var bezierPath = new UIBezierPath();
            bezierPath.MoveTo(new CGPoint(frame.GetMinX() + 0.73928f * frame.Width, frame.GetMinY() + 0.14291f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.85711f * frame.Width, frame.GetMinY() + 0.26074f * frame.Height), new CGPoint(frame.GetMinX() + 0.73926f * frame.Width, frame.GetMinY() + 0.14289f * frame.Height), new CGPoint(frame.GetMinX() + 0.85711f * frame.Width, frame.GetMinY() + 0.26074f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.61785f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.85711f * frame.Width, frame.GetMinY() + 0.26074f * frame.Height), new CGPoint(frame.GetMinX() + 0.74457f * frame.Width, frame.GetMinY() + 0.37328f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.85355f * frame.Width, frame.GetMinY() + 0.73570f * frame.Height), new CGPoint(frame.GetMinX() + 0.74311f * frame.Width, frame.GetMinY() + 0.62526f * frame.Height), new CGPoint(frame.GetMinX() + 0.85355f * frame.Width, frame.GetMinY() + 0.73570f * frame.Height));
            bezierPath.AddLineTo(new CGPoint(frame.GetMinX() + 0.73570f * frame.Width, frame.GetMinY() + 0.85355f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.61785f * frame.Height), new CGPoint(frame.GetMinX() + 0.73570f * frame.Width, frame.GetMinY() + 0.85355f * frame.Height), new CGPoint(frame.GetMinX() + 0.62526f * frame.Width, frame.GetMinY() + 0.74311f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.26785f * frame.Width, frame.GetMinY() + 0.85000f * frame.Height), new CGPoint(frame.GetMinX() + 0.37621f * frame.Width, frame.GetMinY() + 0.74164f * frame.Height), new CGPoint(frame.GetMinX() + 0.26785f * frame.Width, frame.GetMinY() + 0.85000f * frame.Height));
            bezierPath.AddLineTo(new CGPoint(frame.GetMinX() + 0.15000f * frame.Width, frame.GetMinY() + 0.73215f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.38215f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.15000f * frame.Width, frame.GetMinY() + 0.73215f * frame.Height), new CGPoint(frame.GetMinX() + 0.25836f * frame.Width, frame.GetMinY() + 0.62379f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.14645f * frame.Width, frame.GetMinY() + 0.26430f * frame.Height), new CGPoint(frame.GetMinX() + 0.25689f * frame.Width, frame.GetMinY() + 0.37474f * frame.Height), new CGPoint(frame.GetMinX() + 0.14645f * frame.Width, frame.GetMinY() + 0.26430f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.22060f * frame.Width, frame.GetMinY() + 0.19014f * frame.Height), new CGPoint(frame.GetMinX() + 0.14645f * frame.Width, frame.GetMinY() + 0.26430f * frame.Height), new CGPoint(frame.GetMinX() + 0.18706f * frame.Width, frame.GetMinY() + 0.22369f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.26430f * frame.Width, frame.GetMinY() + 0.14645f * frame.Height), new CGPoint(frame.GetMinX() + 0.24420f * frame.Width, frame.GetMinY() + 0.16655f * frame.Height), new CGPoint(frame.GetMinX() + 0.26430f * frame.Width, frame.GetMinY() + 0.14645f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.38215f * frame.Height), new CGPoint(frame.GetMinX() + 0.26430f * frame.Width, frame.GetMinY() + 0.14645f * frame.Height), new CGPoint(frame.GetMinX() + 0.37474f * frame.Width, frame.GetMinY() + 0.25689f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.73926f * frame.Width, frame.GetMinY() + 0.14289f * frame.Height), new CGPoint(frame.GetMinX() + 0.62672f * frame.Width, frame.GetMinY() + 0.25543f * frame.Height), new CGPoint(frame.GetMinX() + 0.73926f * frame.Width, frame.GetMinY() + 0.14289f * frame.Height));
            bezierPath.AddLineTo(new CGPoint(frame.GetMinX() + 0.73928f * frame.Width, frame.GetMinY() + 0.14291f * frame.Height));
            bezierPath.ClosePath();
            color2.SetFill();
            bezierPath.Fill();
            UIColor.Black.SetStroke();
            bezierPath.LineWidth = 1.0f;
            bezierPath.Stroke();
        }


        private void DrawRectLT(CGRect frame)
        {
            UIColor.Red.SetStroke();
            var context = UIGraphics.GetCurrentContext();
            context.BeginPath();
            context.MoveTo(0, 0);
            context.AddLineToPoint(60, 0);
            context.AddLineToPoint(60, 20);
            context.AddLineToPoint(0, 20);
            context.ClosePath();
            UIColor.Red.SetFill();
            context.DrawPath(CGPathDrawingMode.FillStroke);

            context.BeginPath();
            context.MoveTo(0, 0);
            context.AddLineToPoint(20, 0);
            context.AddLineToPoint(20, 60);
            context.AddLineToPoint(0, 60);
            context.ClosePath();
            UIColor.Red.SetFill();
            context.DrawPath(CGPathDrawingMode.FillStroke);


        }



        private void DrawRectLB(CGRect frame)
        {
            UIColor.Red.SetStroke();
            var context = UIGraphics.GetCurrentContext();
            context.BeginPath();
            context.MoveTo(0, 0);
            context.AddLineToPoint(20, 0);
            context.AddLineToPoint(20, 60);
            context.AddLineToPoint(0, 60);
            context.ClosePath();
            UIColor.Red.SetFill();
            context.DrawPath(CGPathDrawingMode.FillStroke);

           

            context.BeginPath();
            context.MoveTo(0, 40);
            context.AddLineToPoint(60, 40);
            context.AddLineToPoint(60, 60);
            context.AddLineToPoint(0, 60);
            context.ClosePath();
            UIColor.Red.SetFill();
            context.DrawPath(CGPathDrawingMode.FillStroke);


        }

        private void DrawRectRT(CGRect frame)
        {
            UIColor.Red.SetStroke();
            var context = UIGraphics.GetCurrentContext();
             
           
            context.BeginPath();
            context.MoveTo(0, 0);
            context.AddLineToPoint(60, 0);
            context.AddLineToPoint(60, 20);
            context.AddLineToPoint(0, 20);
            context.ClosePath();
            UIColor.Red.SetFill();
            context.DrawPath(CGPathDrawingMode.FillStroke);



            context.BeginPath();
            context.MoveTo(40, 0);
            context.AddLineToPoint(60, 0);
            context.AddLineToPoint(60, 60);
            context.AddLineToPoint(40, 60);
            context.ClosePath();
            UIColor.Red.SetFill();
            context.DrawPath(CGPathDrawingMode.FillStroke);


        }


        private void DrawRectRB(CGRect frame)
        {
            UIColor.Red.SetStroke();
            var context = UIGraphics.GetCurrentContext();
            context.BeginPath();
            context.MoveTo(40, 0);
            context.AddLineToPoint(60, 0);
            context.AddLineToPoint(60, 60);
            context.AddLineToPoint(40, 60);
            context.ClosePath();
            UIColor.Red.SetFill();
            context.DrawPath(CGPathDrawingMode.FillStroke);



            context.BeginPath();
            context.MoveTo(0, 40);
            context.AddLineToPoint(60, 40);
            context.AddLineToPoint(60, 60);
            context.AddLineToPoint(0, 60);
            context.ClosePath();
            UIColor.Red.SetFill();
            context.DrawPath(CGPathDrawingMode.FillStroke);


        }

        #endregion

    }

    internal class UIPaintCodeButton : UIButton
    {
        Action<CGRect> _drawing;
        public UIPaintCodeButton(Action<CGRect> drawing)
        {
            _drawing = drawing;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            _drawing(rect);
        }

    }

    internal class UIPaintRect : UIButton
    {
       
        Action<CGRect> _drawing;
        public UIPaintRect(Action<CGRect> drawing)
        {
            _drawing = drawing;
        }

        public override void Draw(CGRect rect)
        {
            
           
            base.Draw(rect);
            _drawing(rect);
        }

    }
}
