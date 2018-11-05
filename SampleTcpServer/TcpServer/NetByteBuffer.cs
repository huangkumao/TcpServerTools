using System;
using System.Threading;

namespace TcpServer
{
    public class NetByteBuffer
    {
        //字节缓存区
        private readonly byte[] buf;
        private readonly byte[] byfTemp;
        //读取索引
        private int readIndex = 0;
        //写入索引
        private int writeIndex = 0;
        //读取索引标记
        private int markReadIndex = 0;
        //写入索引标记
        private int markWirteIndex = 0;

        //构造方法
        private NetByteBuffer(int capacity)
        {
            buf = new byte[capacity];
            byfTemp = new byte[capacity];
        }

        //构建一个capacity长度的字节缓存区NetByteBuffer对象
        public static NetByteBuffer Allocate(int capacity)
        {
            return new NetByteBuffer(capacity);
        }

        //将bytes字节数组从startIndex开始的length字节写入到此缓存区
        public NetByteBuffer WriteBytes(byte[] bytes, int startIndex, int length)
        {
            try
            {
                Monitor.Enter(this);
                int offset = length - startIndex;
                int total = offset + writeIndex;
                Buffer.BlockCopy(bytes, startIndex, buf, writeIndex, length);
                writeIndex = total;
            }
            catch (Exception exp)
            {
                Console.WriteLine($"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   接收缓冲区爆炸了! Exp = {exp}");
            }
            finally
            {
                Monitor.Exit(this);
            }
            return this;
        }

        //将字节数组中从0到length的元素写入缓存区
        public NetByteBuffer WriteBytes(byte[] bytes, int length)
        {
            return WriteBytes(bytes, 0, length);
        }

        //将字节数组全部写入缓存区
        public NetByteBuffer WriteBytes(byte[] bytes)
        {
            return WriteBytes(bytes, 0, bytes.Length);
        }

        //读取一个字节
        public byte ReadByte()
        {
            byte b = buf[readIndex];
            readIndex++;
            return b;
        }

        //读取一个int32数据
        readonly byte[] intBytes = new byte[4];
        public int ReadInt()
        {
            Buffer.BlockCopy(buf, readIndex, intBytes, 0, 4);
            readIndex += 4;

            return BitConverter.ToInt32(intBytes, 0);
        }

        //从读取索引位置开始读取len长度的字节到disbytes目标字节数组中
        public void ReadBytes(byte[] disbytes, int disstart, int len)
        {
            int size = disstart + len;
            for (int i = disstart; i < size; i++)
            {
                disbytes[i] = buf[readIndex];
                readIndex++;
            }
        }

        //清空此对象
        public void Clear(bool pReset0 = false)
        {
            if(pReset0)
                Array.Clear(buf, 0, buf.Length);
            readIndex = 0;
            writeIndex = 0;
            markReadIndex = 0;
            markWirteIndex = 0;
        }

        //粘包发生时 先把当前数据拷贝到临时缓冲, 然后重置之后 再次进行处理
        public void ReSetBuf()
        {
            var L = ReadableBytes();
            if (L > 0)
            {
                ReadBytes(byfTemp, 0, ReadableBytes()); //拷贝临时缓冲
                Clear(); //清理
                WriteBytes(byfTemp, L); //重置
                MarkReaderIndex();
            }
            else
                Clear();
        }

        //设置开始读取的索引
        public void SetReaderIndex(int index)
        {
            if (index < 0) return;
            readIndex = index;
        }

        //标记读取的索引位置
        public int MarkReaderIndex()
        {
            markReadIndex = readIndex;
            return markReadIndex;
        }

        //标记写入的索引位置
        public void MarkWriterIndex()
        {
            markWirteIndex = writeIndex;
        }

        //将读取的索引位置重置为标记的读取索引位置
        public void ResetReaderIndex()
        {
            readIndex = markReadIndex;
        }

        //将写入的索引位置重置为标记的写入索引位置
        public void ResetWriterIndex()
        {
            writeIndex = markWirteIndex;
        }

        //可读的有效字节数
        public int ReadableBytes()
        {
            return writeIndex - readIndex;
        }
    }
}
