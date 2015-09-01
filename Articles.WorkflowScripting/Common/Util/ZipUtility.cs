using System;
using System.IO;
using System.IO.Packaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Util
{
    public static class ZipUtility
    {
        /// <summary>
        /// Starts to compress the given file.
        /// </summary>
        /// <param name="fileName">The file to compress.</param>
        public static void CompressFile(string fileName)
        {
            //Check given file name
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(
                    "Unable to find the file that should be compressed!", fileName);
            }

            //Generate name for new zip file
            string zipFileName = GenerateNewZipName(fileName);
            if (File.Exists(zipFileName))
            {
                throw new ApplicationException(
                    "Zip-File does already exist (" + zipFileName + ")!");
            }

            //Generate zip package and a part for our file
            using (ZipPackage zipPackage = ZipPackage.Open(zipFileName, FileMode.Create)
                as ZipPackage)
            {
                PackagePart part = zipPackage.CreatePart(
                    new Uri("/" + Path.GetFileName(fileName), UriKind.Relative),
                    "text/text",
                    CompressionOption.Normal);

                //Copy file into zip part
                using (Stream fileStream = part.GetStream(FileMode.Create))
                {
                    using (Stream sourceFileStream = File.OpenRead(fileName))
                    {
                        CopyBytes(sourceFileStream, fileStream);
                        sourceFileStream.Close();
                    }

                    //Close generated zip file
                    fileStream.Close();
                }
                zipPackage.Close();
            }
        }

        /// <summary>
        /// Adds the given file to the given zip file.
        /// </summary>
        /// <param name="sourceFile">File to add to the zip file.</param>
        /// <param name="targetZip">Target zip file to write the given file in.</param>
        public static void AddToZip(string sourceFile, string targetZip)
        {
            //Check given file name
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException(
                    "Unable to find the file that should be compressed!", sourceFile);
            }
            string sourceFileName = Path.GetFileName(sourceFile);

            //Open the zip package
            using (ZipPackage zipPackage = ZipPackage.Open(targetZip, FileMode.Open)
                as ZipPackage)
            {
                //Check if there is already a file with this name
                if (zipPackage.PartExists(new Uri("/" + sourceFileName, UriKind.Relative)))
                {
                    throw new ApplicationException(
                        "There is already a file with this name within the zip!");
                }

                //Create the new part
                PackagePart part = zipPackage.CreatePart(
                    new Uri("/" + Path.GetFileName(sourceFileName), UriKind.Relative),
                    "text/text",
                    CompressionOption.Normal);

                //Copy file into zip part
                using (Stream fileStream = part.GetStream(FileMode.Create))
                {
                    using (Stream sourceFileStream = File.OpenRead(sourceFile))
                    {
                        CopyBytes(sourceFileStream, fileStream);
                        sourceFileStream.Close();

                        //Close generated zip file
                        fileStream.Close();
                    }
                }
                zipPackage.Close();
            }
        }

        /// <summary>
        /// Extracts all files from the given zip-file to the given directory.
        /// </summary>
        /// <param name="sourceZipFile">The source zip file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public static void ExtractAllFiles(string sourceZipFile, string targetDirectory)
        {
            using (ZipPackage zipPackage = ZipPackage.Open(sourceZipFile, FileMode.Open)
                as ZipPackage)
            {
                foreach (PackagePart actPart in zipPackage.GetParts())
                {
                    //Open part for reading
                    using (Stream partReadStream = actPart.GetStream(FileMode.Open))
                    {
                        //Generate and write the file on the file-system
                        string newFileName = Path.Combine(
                            targetDirectory,
                            actPart.Uri.OriginalString.TrimStart('/'));
                        string directoryName = Path.GetDirectoryName(newFileName);
                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        using (Stream fileStream = File.Create(newFileName))
                        {
                            CopyBytes(partReadStream, fileStream);
                            fileStream.Close();
                        }

                        //Close part stream
                        partReadStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Generates a new zip-file name for the given file name.
        /// </summary>
        /// <param name="fileName">Name of the file to generate a zip-file name for.</param>
        private static string GenerateNewZipName(string fileName)
        {
            string zipFileName = Path.Combine(
                Path.GetDirectoryName(fileName),
                Path.GetFileNameWithoutExtension(fileName) + ".zip");
            int loop = 1;
            while (File.Exists(zipFileName))
            {
                zipFileName = Path.Combine(
                    Path.GetDirectoryName(fileName),
                    Path.GetFileNameWithoutExtension(fileName) + "(" + loop + ").zip");
                loop++;
            }
            return zipFileName;
        }

        /// <summary>
        /// Copies all bytes from source stream to target stream.
        /// </summary>
        /// <param name="source">Stream where to read bytes from.</param>
        /// <param name="target">Stream where to write bytes to.</param>
        private static void CopyBytes(Stream source, Stream target)
        {
            CopyBytes(source, target, 100);
        }

        /// <summary>
        /// Copies all bytes from source stream to target stream.
        /// </summary>
        /// <param name="source">Stream where to read bytes from.</param>
        /// <param name="target">Stream where to write bytes to.</param>
        /// <param name="bufferSize">Total size of the buffer used for copy.</param>
        private static void CopyBytes(Stream source, Stream target, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int actLocation = 0;
            int byteCount = 0;
            while ((byteCount = source.Read(buffer, actLocation, buffer.Length)) > 0)
            {
                target.Write(buffer, 0, byteCount);
            }
        }
    }
}
