using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RK.Common.GraphicsEngine.Objects.Construction
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AnimationData
    {
        private uint m_boneIndex01;
        private uint m_boneIndex02;
        private uint m_boneIndex03;
        private uint m_boneIndex04;
        private float m_boneWeight01;
        private float m_boneWeight02;
        private float m_boneWeight03;
        private float m_boneWeight04;

        /// <summary>
        /// Gets or sets the first bone index.
        /// </summary>
        public uint BoneIndex01
        {
            get { return m_boneIndex01; }
            set { m_boneIndex01 = value; }
        }

        /// <summary>
        /// Gets or sets the second bone index.
        /// </summary>
        public uint BoneIndex02
        {
            get { return m_boneIndex02; }
            set { m_boneIndex02 = value; }
        }

        /// <summary>
        /// Gets or sets the third bone index.
        /// </summary>
        public uint BoneIndex03
        {
            get { return m_boneIndex03; }
            set { m_boneIndex03 = value; }
        }

        /// <summary>
        /// Gets or sets the fourth bone index.
        /// </summary>
        public uint BoneIndex04
        {
            get { return m_boneIndex04; }
            set { m_boneIndex04 = value; }
        }

        /// <summary>
        /// Gets or sets the first bone weight.
        /// </summary>
        public float BoneWeight01
        {
            get { return m_boneWeight01; }
            set { m_boneWeight01 = value; }
        }

        /// <summary>
        /// Gets or sets the second bone weight.
        /// </summary>
        public float BoneWeight02
        {
            get { return m_boneWeight02; }
            set { m_boneWeight02 = value; }
        }

        /// <summary>
        /// Gets or sets the third bone weight.
        /// </summary>
        public float BoneWeight03
        {
            get { return m_boneWeight03; }
            set { m_boneWeight03 = value; }
        }

        /// <summary>
        /// Gets or sets the fourth bone weight.
        /// </summary>
        public float BoneWeight04
        {
            get { return m_boneWeight04; }
            set { m_boneWeight04 = value; }
        }
    }
}
