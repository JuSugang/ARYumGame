// ---------------------------------------------------------------------------
//
// Copyright (c) 2018 Alchera, Inc. - All rights reserved.
//
// This example script is under BSD-3-Clause licence.
//
// Author
//       Park DongHa     | dh.park@alcherainc.com
//
// ---------------------------------------------------------------------------
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Alchera
{
    /// <summary>
    /// 프레임을 ReadWebcam 으로부터 전달받아 얼굴을 찾아내고, 
    /// Blend Shape 인자를 활용하는 예제
    /// </summary>
    public class GetBlendShape : MonoBehaviour, IFrameProcessor
    {
        // Face 인식 모듈
        Alchera.IFaceDetector detector;

        public Material material;   // 얼굴 점들을 보여줄 Material. 
                                    // 예제에서는 CaptureMat을 사용한다.
        public GameObject model;    // Animation 인자를 사용할 Model 개체
        Quaternion modelRotation;   // 최초 모델 회전
        Vector4 modelPosition;      // 최초 모델 위치
        float[] anim = null;        // 갱신된 Animation 인자 배열. 총 26개

        public Text VersionLabel;   // Library 버전을 표시할 레이블
        public Text ParamLabel;     // Animation 인자를 표시할 레이블

        void Start()
        {
            // Face Detection 모듈 생성. 실패할 경우 예외를 던진다.
            detector = Alchera.Module.Face();
            // 모듈 생성 이후 버전 표시
			VersionLabel.text = detector.Version;

            model.transform.localScale = Vector3.one * 230;
            modelRotation = model.transform.localRotation;
            modelPosition = model.transform.localPosition;
            // App 테이블에 등록
            AppData.Set<IFrameProcessor>(this);
        }

        void OnDestroy()
        {
            // App 테이블에 등록 해제
            AppData.Set<IFrameProcessor>(null);
            // 점유한 메모리, 임시 파일 등 자원들을 전부 파기한다.
            detector.Dispose();
        }

        void OnGUI()
        {
            if (anim == null)
                return;

            ParamLabel.text = string.Format(
                "Animation: \n"
                + string.Format("{1, -25}\t{0,5}\n", anim[0], "Brow_Outer_Up_Left")
                + string.Format("{1, -25}\t{0,5}\n", anim[1], "Brow_Outer_Up_Right")
                + string.Format("{1, -25}\t{0,5}\n", anim[2], "Brow_Down_Left")
                + string.Format("{1, -25}\t{0,5}\n", anim[3], "Brow_Down_Right")
                + string.Format("{1, -25}\t{0,5}\n", anim[4], "Brow_Inner_Up")
                + string.Format("{1, -25}\t{0,5}\n", anim[5], "Eye_Wide_Left")
                + string.Format("{1, -25}\t{0,5}\n", anim[6], "Eye_Wide_Right")
                + string.Format("{1, -25}\t{0,5}\n", anim[7], "Eye_Blink_Left")
                + string.Format("{1, -25}\t{0,5}\n", anim[8], "Eye_Blink_Right")
                + string.Format("{1, -25}\t{0,5}\n", anim[9], "Mouth_Left")
                + string.Format("{1, -25}\t{0,5}\n", anim[10], "Mouth_Right")
                + string.Format("{1, -25}\t{0,5}\n", anim[21], "Mouth_Pucker")
                + string.Format("{1, -25}\t{0,5}\n", anim[22], "Mouth_Funnel")
                + string.Format("{1, -25}\t{0,5}\n", anim[23], "Jaw_Left")
                + string.Format("{1, -25}\t{0,5}\n", anim[24], "Jaw_Right")
                + string.Format("{1, -25}\t{0,5}\n", anim[25], "Jaw_Open")
            );
        }

        /// <summary>
        /// Frame callback. Try to detect face from it
        /// </summary>
		/// <param name="frame">Frame for detection</param>
        /// <param name="degree">Degree of current webcam</param>
        public void OnFrame(FrameData frame, uint degree)
        {
            model.SetActive(false);
            foreach (var facedata in detector.Faces(ref frame, (uint)degree))
            {
                OnFace(facedata, degree);
                model.SetActive(true);
                break; // use only 1 face
            }
        }

        void OnFace(IFaceData facedata, float degree)
        {

            // This function calculates 
            //      face's position, rotation, and animation parameters
            //facedata.Track();

            var cameraRotation = Quaternion.AngleAxis(degree, Vector3.back);

            model.transform.localRotation = 
                modelRotation * cameraRotation * facedata.Rotation;

            model.transform.localPosition = 
                modelPosition + (Matrix4x4.Rotate(cameraRotation) * facedata.Position);

            //anim = facedata.Animation;

            //// ..
            //// Use animation params here...
            //var renderer = model.GetComponentInChildren<SkinnedMeshRenderer>();
            //renderer.SetBlendShapeWeight(4,  anim[0]);  // Brow_Outer_Up_Left (왼쪽 눈썹의 바깥쪽 부분 올림)
            //renderer.SetBlendShapeWeight(3,  anim[1]);  // Brow_Outer_Up_Right(오른쪽 눈썹의 바깥쪽 부분 올림)
            //renderer.SetBlendShapeWeight(2,  anim[2]);  // Brow_Down_Left(왼쪽 눈썹의 바깥쪽 부분 내림)
            //renderer.SetBlendShapeWeight(1,  anim[3]);  // Brow_Down_Right(오른쪽 눈썹의 바깥쪽 부분 내림)
            //renderer.SetBlendShapeWeight(0,  anim[4]);  // Brow_Inner_Up(좌, 우 눈썹의 안쪽 부분 모두 올림)
            //renderer.SetBlendShapeWeight(8,  anim[5]);  // Eye_Wide_Left(왼쪽 눈 크게 뜸)
            //renderer.SetBlendShapeWeight(7,  anim[6]);  // Eye_Wide_Right(오른쪽 눈 크게 뜸)
            //renderer.SetBlendShapeWeight(6,  anim[7]);  // Eye_Blink_Left(왼쪽 눈 감음)
            //renderer.SetBlendShapeWeight(5,  anim[8]);  // Eye_Blink_Right(오른쪽 눈 감음)
            //renderer.SetBlendShapeWeight(20, anim[9]);  // Mouth_Left(위, 아랫입술 모두 왼쪽으로 움직임)
            //renderer.SetBlendShapeWeight(19, anim[10]); // Mouth_Right(위, 아랫입술 모두 오른쪽으로 움직임)
            //renderer.SetBlendShapeWeight(18, anim[11]); // Mouth_Frown_Left(입 왼쪽 끝 아래로 내림)
            //renderer.SetBlendShapeWeight(17, anim[12]); // Mouth_Frown_Right(입 오른쪽 끝 아래로 내림)
            //renderer.SetBlendShapeWeight(16, anim[13]); // Mouth_Smile_Left(입 왼쪽 끝 위로 올림)
            //renderer.SetBlendShapeWeight(15, anim[14]); // Mouth_Smile_Right(입 오른쪽 끝 위로 올림)
            //renderer.SetBlendShapeWeight(25, anim[15]); // Mouth_Stretch_Left(입 왼쪽 끝 왼쪽으로 움직임)
            //renderer.SetBlendShapeWeight(24, anim[16]); // Mouth_Stretch_Right(입 오른쪽 끝 오른쪽으로 움직임)
            //renderer.SetBlendShapeWeight(14, anim[17]); // Mouth_Lower_Down_Left(아랫입술의 왼쪽 부분 내림)
            //renderer.SetBlendShapeWeight(13, anim[18]); // Mouth_Lower_Down_Right(아랫입술의 오른쪽 부분 내림)
            //renderer.SetBlendShapeWeight(12, anim[19]); // Mouth_Upper_Up_Left(윗입술의 왼쪽 부분 올림)
            //renderer.SetBlendShapeWeight(11, anim[20]); // Mouth_Upper_Up_Right(윗입술의 오른쪽 부분 올림)
            //renderer.SetBlendShapeWeight(10, anim[21]); // Mouth_Pucker(입을 다문 상태에서 입술 오므림)
            //renderer.SetBlendShapeWeight(9,  anim[22]); // Mouth_Funnel(입을 연 모양으로 입술 오므림)
            //renderer.SetBlendShapeWeight(23, anim[23]); // Jaw_Left(아랫 턱을 왼쪽으로 움직임)
            //renderer.SetBlendShapeWeight(22, anim[24]); // Jaw_Right(아랫 턱을 오른쪽으로 움직임)
            //renderer.SetBlendShapeWeight(21, anim[25]); // Jaw_Open(아랫 턱을 연 모양)

            // Show the landmark points
            var tex2d = material.mainTexture as Texture2D;
            Marker.Points(tex2d, facedata.Landmark, Color.blue);    
            tex2d.Apply(false);
        }
    } // GetBlendShape

}
