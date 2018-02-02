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
        bool markOn = true;

        public Material material;   // 얼굴 점들을 보여줄 Material. 
                                    // 예제에서는 CaptureMat을 사용한다.
        public GameObject model;    // Animation 인자를 사용할 Model 개체
        Quaternion modelRot;        // 최초 모델 회전
        Vector4 modelPos;           // 최초 모델 위치
        Matrix4x4 correction;       // 측정된 얼굴 위치의 보정
                                    // Correction of detected face's postion 

        public bool UseReprojectMouth;
        public AnimationCurve ReprojectionCurve;
        float[] anime;

        public Text VersionLabel;   // Library 버전을 표시할 레이블
        // public Text ParamLabel;     // Animation 인자를 표시할 레이블
        public Button MarkOnOff;    // Landmark On/Off
        public Text TimeLabel;      // Elapsed time to process per frame
        float elapsed;

        /// <summary>
        /// 모델의 크기, 위치 보정
        /// </summary>
        /// <remarks>
        /// 모델 혹은 라이브러리 사용자가 원하는 공간 크기에 따라 위치 값이 보정되어야 한다.
        /// 때문에 외부에서 Transform Matrix를 사용하도록 설계.
        /// </remarks>
        void Start()
        {
#if UNITY_STANDALONE_OSX
            correction  = Matrix4x4.Scale(new Vector3(3.5f,3.5f,5.3f));
            model.transform.localScale = Vector3.one * 100;
#elif UNITY_STANDALONE_WIN
            correction  = Matrix4x4.Scale(new Vector3(3,3,4.9f));
            model.transform.localScale = Vector3.one * 90;
#elif UNITY_IPHONE
            correction  = Matrix4x4.Scale(new Vector3(8.1f, 8.1f, 5.1f));
            model.transform.localScale = Vector3.one * 270;
#elif UNITY_ANDROID
            correction = Matrix4x4.Scale(new Vector3(20.7f, 22.7f, 8.3f));
            model.transform.localScale = Vector3.one * 540;
#else
#error "Platform not supported."
#endif

            // Face Detection 모듈 생성. 실패할 경우 예외를 던진다.
            detector = Alchera.Module.Face();

            // 모듈 생성 이후 버전 표시
			VersionLabel.text = detector.Version;

            // Notice that input image is flipped(rotated 180). 
            // So we will rotate the model in advance to save computation
            modelRot = Quaternion.AngleAxis(180, Vector3.back) 
                                      * model.transform.localRotation;
            modelPos = model.transform.localPosition;

            MarkOnOff.onClick.AddListener(() =>
            {
                markOn = !markOn;
            });

            // App 테이블에 등록
            AppData.Set<IFrameProcessor>(this);
        }

        /// <summary>
        /// App 테이블에 등록 해제
        /// 점유한 메모리, 임시 파일 등 자원들을 파기
        /// </summary>
        void OnDestroy()
        {
            AppData.Set<IFrameProcessor>(null);
            detector.Dispose();
        }


        /// <summary>
        /// 소모한 시간을 표시한다
        /// </summary>
        /// <remarks>
        /// 기존에 사용하던 Animation 인자를 표시하는 내용은 Format 처리에 시간이 많이 소모되기에 제거하였음
        /// </remarks>
        void OnGUI()
        {
            TimeLabel.text = string.Format("OnFrame: {0:F2} ms", elapsed * 1000);
        }

        /// <summary>
        /// 매 프레임마다 Face를 검출한다
        /// </summary>
        public void OnFrame(FrameData frame, float cameraAngle, bool isFront)
        {
            float begin = Time.realtimeSinceStartup;

            // 내부 엔진과 인터페이스를 맞추는 중. 향후 사라질 부분
            float detectAngle = 540 - cameraAngle;

            // Revoke rotation of Camera & Detection
            Quaternion revokeCamera = Quaternion.AngleAxis(
                (isFront) ? cameraAngle : -cameraAngle, Vector3.back);
            Quaternion revokeDetect = Quaternion.AngleAxis(
                (isFront) ? detectAngle : -detectAngle, Vector3.back);

            // 검출되지 않았을 때 보이지 않도록 처리
            model.SetActive(false);
            foreach (IFaceData face in detector.Faces(ref frame, (uint)detectAngle))
            {
                // 처리 루틴으로 전달
                OnFace(face, isFront, ref revokeCamera, ref revokeDetect);

                // 검출 성공. 오직 1개만 처리하기 위해 바로 break
                model.SetActive(true);
                break;
            }

            // Elapsed time: Multiple Face Detection + Processing 1 Face   
            elapsed = Time.realtimeSinceStartup - begin;
        }

        /// <summary>
        /// Face 처리 루틴
        /// </summary>
        void OnFace(IFaceData face, bool isFront, 
                    ref Quaternion revokeCamera, ref Quaternion revokeDetect)
        {
            // Face ID와 Landmark는 기본으로 사용할 수 있다.
            if(markOn)
                ApplyMarks(face);

            // 추가적인 처리가 필요한 경우, Track()을 호출하여야 한다.
            // Plugin 내부적으로 Face의 3D Position, Rotation, Animation 인자가 계산된다.
            face.Track();

			// Mirroring for front camera
            var facePosition = Mirror(face.Position, isFront);
            var faceRotation = Mirror(face.Rotation, isFront);

            // 검출된 Face의 '위치'를 모델에 적용하는 방법
            //  - 위치 초기값 + (카메라 회전 * 위치 보정 * 입력(이미지) 위치 추정값)
            model.transform.localPosition =
                    modelPos + (Matrix4x4.Rotate(revokeDetect) * correction * facePosition);

            // 검출된 Face의 '회전'을 모델에 적용하는 방법
            //  - 회전 초기값 * 카메라 회전 * 입력(이미지)에서의 회전 추정값ㄴ
            model.transform.localRotation =
                    modelRot * revokeCamera * faceRotation;

            // Blendshape parameter 적용
            ApplyAnimation(face);
        }

        static Quaternion Mirror(Quaternion q, bool isFront)
        {
            // YZ Plane mirroring of quaternion
            if (isFront)
            {
                q.y = -q.y;
                q.z = -q.z;
            }
            return q;
        }
        static Vector3 Mirror(Vector3 v, bool isFront)
        {
            // YZ Plane mirroring of position vector
            if(isFront)
            {
                v.x = -v.x;
            }
            return v;
        }

        /// <summary>
        /// Applies the marks.
        /// </summary>
        /// <param name="face">Face.</param>
        void ApplyMarks(IFaceData face)
        {
            var tex2d = material.mainTexture as Texture2D;
            Marker.Points(tex2d, face.Landmark, Color.white);
            tex2d.Apply(false);
        }

        /// <summary>
        /// Applies the animation.
        /// </summary>
        /// <param name="face">Face.</param>
        void ApplyAnimation(IFaceData face)
        {
            // GUI에 표시하기 위해 별도로 기록
            anime = face.Animation;

            if (UseReprojectMouth)
            {
                // Reproject Mouth params
                float val = anime[25] / 100.0f;
                float res = ReprojectionCurve.Evaluate(val);
                anime[25] = res * 100.0f;
            }

            // ..
            // 애니메이션 인자를 사용하는 코드
            var renderer = model.GetComponentInChildren<SkinnedMeshRenderer>();
            renderer.SetBlendShapeWeight(8, anime[0]);  // Brow_Outer_Up_Left   (왼쪽 눈썹의 바깥쪽 부분 올림)
            renderer.SetBlendShapeWeight(20, anime[1]);  // Brow_Outer_Up_Right  (오른쪽 눈썹의 바깥쪽 부분 올림)
            renderer.SetBlendShapeWeight(9, anime[2]);  // Brow_Down_Left       (왼쪽 눈썹의 바깥쪽 부분 내림)
            renderer.SetBlendShapeWeight(1, anime[3]);  // Brow_Down_Right      (오른쪽 눈썹의 바깥쪽 부분 내림)
            renderer.SetBlendShapeWeight(15, anime[4]);  // Brow_Inner_Up        (좌, 우 눈썹의 안쪽 부분 모두 올림)
            renderer.SetBlendShapeWeight(13, anime[5]);  // Eye_Wide_Left        (왼쪽 눈 크게 뜸)
            renderer.SetBlendShapeWeight(12, anime[6]);  // Eye_Wide_Right       (오른쪽 눈 크게 뜸)
            renderer.SetBlendShapeWeight(11, anime[7]);  // Eye_Blink_Left       (왼쪽 눈 감음)
            renderer.SetBlendShapeWeight(3, anime[8]);  // Eye_Blink_Right      (오른쪽 눈 감음)
            renderer.SetBlendShapeWeight(0, anime[9]);  // Mouth_Left           (위, 아랫입술 모두 왼쪽으로 움직임)
            renderer.SetBlendShapeWeight(21, anime[10]); // Mouth_Right          (위, 아랫입술 모두 오른쪽으로 움직임)
            renderer.SetBlendShapeWeight(2, anime[11]); // Mouth_Frown_Left     (입 왼쪽 끝 아래로 내림)
            renderer.SetBlendShapeWeight(16, anime[12]); // Mouth_Frown_Right    (입 오른쪽 끝 아래로 내림)
            renderer.SetBlendShapeWeight(4, anime[13]); // Mouth_Smile_Left     (입 왼쪽 끝 위로 올림)
            renderer.SetBlendShapeWeight(17, anime[14]); // Mouth_Smile_Right    (입 오른쪽 끝 위로 올림)
            renderer.SetBlendShapeWeight(14, anime[15]); // Mouth_Stretch_Left   (입 왼쪽 끝 왼쪽으로 움직임)
            renderer.SetBlendShapeWeight(18, anime[16]); // Mouth_Stretch_Right  (입 오른쪽 끝 오른쪽으로 움직임)
            //renderer.SetBlendShapeWeight(, anime[17]); // Mouth_Lower_Down_Left    (아랫입술의 왼쪽 부분 내림)
            //renderer.SetBlendShapeWeight(, anime[18]); // Mouth_Lower_Down_Right   (아랫입술의 오른쪽 부분 내림)
            //renderer.SetBlendShapeWeight(, anime[19]); // Mouth_Upper_Up_Left  (윗입술의 왼쪽 부분 올림)
            //renderer.SetBlendShapeWeight(, anime[20]); // Mouth_Upper_Up_Right (윗입술의 오른쪽 부분 올림)
            renderer.SetBlendShapeWeight(10, anime[21]); // Mouth_Pucker         (입을 다문 상태에서 입술 오므림)
            renderer.SetBlendShapeWeight(6, anime[22]); // Mouth_Funnel         (입을 연 모양으로 입술 오므림)
            renderer.SetBlendShapeWeight(5, anime[23]); // Jaw_Left             (아랫 턱을 왼쪽으로 움직임)
            renderer.SetBlendShapeWeight(19, anime[24]); // Jaw_Right            (아랫 턱을 오른쪽으로 움직임)
            renderer.SetBlendShapeWeight(7, anime[25]); // Jaw_Open             (아랫 턱을 연 모양)
            // ...
        }

    }

}
