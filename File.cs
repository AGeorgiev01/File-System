using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace opit4
{
    [Serializable]
    class File
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public bool IsAppend { get; set; }
        public int BlockSize { get; set; }

        public File(string name, byte[] content, int blockSize, bool isAppend = false)
        {
            Name = name;
            Content = content;
            BlockSize = blockSize;
            IsAppend = isAppend;
        }

        public void Write(string newContent, bool isAppend = false)
        {
            byte[] newContentBytes = Encoding.ASCII.GetBytes(newContent);
            if (isAppend && IsAppend)
            {
                byte[] combinedContent = new byte[Content.Length + newContentBytes.Length];
                Array.Copy(Content, 0, combinedContent, 0, Content.Length);
                Array.Copy(newContentBytes, 0, combinedContent, Content.Length, newContentBytes.Length);
                Content = combinedContent;
            }
            else
            {
                Content = newContentBytes;
                IsAppend = isAppend;
            }
        }

        public byte[] GetContent()
        {
            return Content;
        }

        public void SetContent(byte[] newContent)
        {
            Content = newContent;
        }
    }
}
