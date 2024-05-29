using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.IO;

/// <summary>
/// Summary description for RawPrinterHelper
/// </summary>

namespace PRNPrintFile
{
   
    public class RawPrinterHelper
    {
        
        
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PRINTER_INFO_9
        {
            public IntPtr DevMode;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);



        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            try
            {
            
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";
            string path = @"E:\Project\Bosch\PublishWebSite\New\PPTS17\PPTS08\ErrorLog\errorlog.txt";

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("$Message: before print    ==="+ OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero));
            }

         
                // Open the printer.
                //if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter,IntPtr.Zero))
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("$Message: Step1");
                }
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLine("$Message: step2");
                        }
                        // Start a page.
                        if (StartPagePrinter(hPrinter))
                    {
                            // Write your bytes.
                            using (StreamWriter sw = File.AppendText(path))
                            {
                                sw.WriteLine("$Message: step3");
                            }
                            bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("$Message: last "+ dwError);
                    }
                }
            return bSuccess;
            }
            catch (Exception)
            {
                string path = @"E:\Project\Bosch\PublishWebSite\New\PPTS17\PPTS08\ErrorLog\errorlog.txt";

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("$Message: error");
                }
                return false;

            }
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            try
            {

                string path = @"E:\Project\Bosch\PublishWebSite\New\PPTS17\PPTS08\ErrorLog\errorlog.txt";

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("$Message: before read"+ szFileName);
                }
                // Open the file.
                FileStream fs = new FileStream(szFileName, FileMode.Open);
               // string path = @"E:\Project\Bosch\PublishWebSite\PPTS1633\ErrorLog\errorlog.txt";

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("$Message: After read"+fs.Length.ToString());
                }
                // Create a BinaryReader on the file.
                BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(700);//Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
                // Send the unmanaged bytes to the printer.
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("$Message:" + br.ToString() + bSuccess+ szPrinterName);
                }
                bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
               // string path = @"E:\Project\Bosch\PublishWebSite\PPTS1633\ErrorLog\errorlog.txt";

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("$Message:"+ br.ToString()+ bSuccess);
                }
                // Free the unmanaged memory that you allocated earlier.
                Marshal.FreeCoTaskMem(pUnmanagedBytes);
                if (br != null)
                {
                    br.Close();
                }
                if (fs!=null)
                {   
                    
                    fs.Close();
                }
                

                return bSuccess;
            }
            catch (Exception ex)
            {
                string path = @"E:\Project\Bosch\PublishWebSite\New\PPTS17\PPTS08\ErrorLog\errorlog.txt";

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("$Message:" + ex.Message + "\t" + "Error" + "\t" + "$Datetime:" + DateTime.Now);
                }
                return true; ;
            }
        }
        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

    }
}
