using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCMSecurityScan
{
    public class Automazione
    {
        static List<string> AllFullPathFiles = new List<string>();
        public void Start()
        {
            List<string> SpecialFolders = new List<string>();
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.Favorites));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
            SpecialFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.Recent));

            string[] drives = System.Environment.GetLogicalDrives();

            foreach (string dr in drives)
            {
                System.IO.DriveInfo di = new System.IO.DriveInfo(dr);

                // Here we skip the drive if it is not ready to be read. This
                // is not necessarily the appropriate action in all scenarios.
                if (!di.IsReady)
                {
                    Console.WriteLine("The drive {0} could not be read", di.Name);
                    continue;
                }
                System.IO.DirectoryInfo rootDir = di.RootDirectory;
                WalkDirectoryTree(rootDir);
            }            

            foreach(var r in SpecialFolders)
            {
                System.IO.DirectoryInfo rootDir = new System.IO.DirectoryInfo(r);
                WalkDirectoryTree(rootDir);
            }

            System.IO.File.WriteAllLines("allSpec.txt",AllFullPathFiles);


        }

        public void Stop()
        {

        }

        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        static void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    AllFullPathFiles.Add(fi.FullName);
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                //foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                //{
                //    // Resursive call for each subdirectory.
                //    WalkDirectoryTree(dirInfo);
                //}
            }
        }
    }
}
