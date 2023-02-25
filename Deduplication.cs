using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace opit4
{
    [Serializable]
    class Deduplication
    {
        //blocks dictionary that will store the unique blocks of file, key is the hash of the block and block is val.
        private Dictionary<int, byte[]> blocks;
        private int blockSize;

        public Deduplication(int blockSize)
        {
            this.blockSize = blockSize;
            this.blocks = new Dictionary<int, byte[]>();
        }
        
        public void AddFile(File file) //reads File obj and splitz it into blocks; (stored in "block" dict.)
        {
            byte[] content = file.GetContent();
            
            HashSet<int> blockHashes = new HashSet<int>(); //checks if block is alr in dict. (add if not)
            int remainingBytes = content.Length;
            int startIndex = 0;
            while (remainingBytes > 0)
            {
                int currentBlockSize = Math.Min(blockSize, remainingBytes);
                byte[] block = new byte[currentBlockSize];
                Array.Copy(content, startIndex, block, 0, currentBlockSize);

                int hash = block.GetHashCode();
                if (!blockHashes.Contains(hash))
                {
                    blockHashes.Add(hash);
                    blocks.Add(hash, block);
                }

                remainingBytes -= currentBlockSize;
                startIndex += currentBlockSize;
            }
        }

        public void RemoveFile(File file) //reads File obj and splitz it into blocks; (stored in "block" dict.)
        {
            byte[] content = file.GetContent();
            for (int i = 0; i < content.Length; i += blockSize)
            {
                byte[] block = new byte[Math.Min(blockSize, content.Length - i)];
                Array.Copy(content, i, block, 0, block.Length);

                int hash = block.GetHashCode();
                if (blocks.ContainsKey(hash))
                {
                    blocks.Remove(hash); //remove if block is in dict.
                }
            }
        }
}