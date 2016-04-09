﻿using System.IO;

namespace MultiagentAlgorithm
{
    public class FileWriter : IDataWriter
    {
        private readonly string _fileName;

        public FileWriter(string fileName)
        {
            _fileName = fileName;
        }

        public void WriteData(string data)
        {
            File.WriteAllText(_fileName, data);
        }
    }
}