using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XofFileFormatConverter
    {
        public static void Convert(string? inputFilename, string? outputFilename, XofFileFormats format)
        {
            if (string.IsNullOrEmpty(inputFilename))
            {
                throw new ArgumentNullException(nameof(inputFilename));
            }

            if (string.IsNullOrEmpty(outputFilename))
            {
                throw new ArgumentNullException(nameof(outputFilename));
            }

            using var enumObject = XofFile.OpenEnumObject(inputFilename);
            using var saveObject = XofFile.OpenSaveObject(outputFilename, format);
            ConvertObject(enumObject, saveObject);

            saveObject.Save();
        }

        public static void Convert(byte[]? inputBytes, string? outputFilename, XofFileFormats format)
        {
            if (inputBytes == null)
            {
                throw new ArgumentNullException(nameof(inputBytes));
            }

            if (inputBytes.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputBytes));
            }

            if (string.IsNullOrEmpty(outputFilename))
            {
                throw new ArgumentNullException(nameof(outputFilename));
            }

            using var enumObject = XofFile.OpenEnumObject(inputBytes);
            using var saveObject = XofFile.OpenSaveObject(outputFilename, format);
            ConvertObject(enumObject, saveObject);

            saveObject.Save();
        }

        private static void ConvertObject(XofFileEnumObject enumObject, XofFileSaveObject saveObject)
        {
            int count = enumObject.GetChildrenCount();

            for (int i = 0; i < count; i++)
            {
                using var enumData = enumObject.GetChild(i);

                if (enumData is null)
                {
                    continue;
                }

                using var saveData = saveObject.AddData(enumData);
                ConvertData(enumData, saveData);
            }
        }

        private static void ConvertData(XofFileEnumData enumData, XofFileSaveData saveData)
        {
            int count = enumData.GetChildrenCount();

            for (int i = 0; i < count; i++)
            {
                using var child = enumData.GetChild(i);

                if (child is null)
                {
                    continue;
                }

                if (child.IsReference())
                {
                    saveData.AddDataReference(child);
                }
                else
                {
                    using var saveChild = saveData.AddData(child);
                    ConvertData(child, saveChild);
                }
            }
        }
    }
}
