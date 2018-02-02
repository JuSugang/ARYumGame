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
using UnityEngine;
using NUnit.Framework;
using System.IO;

namespace Alchera
{
    public class ResourceTest
    {
        void CleanFile(string name)
        {
            var path = Path.Combine(Application.persistentDataPath, name + ".bin");
            File.Delete(path);
        }

        void CheckFile(string name, int length)
        {
            FileStream stream = null;
            var path = Path.Combine(Application.persistentDataPath, name + ".bin");
            var asset = Resources.Load(name) as TextAsset;

            Assert.NotNull(asset);
            Assert.NotNull(asset.bytes);
            Assert.IsTrue(asset.bytes.Length == length);

            try{
                // Write and open it
                File.WriteAllBytes(path, asset.bytes);
                Debug.Log(path);

                stream = File.Open(path, FileMode.Open);
                Assert.NotNull(stream);

                Assert.IsTrue(stream.CanRead);
                Assert.IsTrue(asset.bytes.Length == stream.Length);
            }
            finally{
                if(stream != null)
                    stream.Close();
            }
        }

        [Test]
        public void CheckIris(){
            var name = "alIris1205";
            CleanFile(name);
            CheckFile(name, length: 48436);
        }

        [Test]
        public void CheckFaceModel()
        {
            var name = "facemodel";
            CleanFile(name);
            CheckFile(name, length: 2977996);
        }

        [Test]
        public void CheckNewFaceModel()
        {
            var name = "facemodel_n";
            CleanFile(name);
            CheckFile(name, length: 3805516);
        }

        [Test]
        public void CheckTotal()
        {
            var name = "alTotal";
            CleanFile(name);
            CheckFile(name, length: 1893999);
        }

    }

}
