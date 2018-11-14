using System;
using System.IO;

namespace ReportageSelector
{
    public class TempFile: IDisposable
    {
        // Track whether Dispose has been called.
        private bool disposed = false;

        public string TempFolder { get; set; }
        public string TempFileName { get; set; }

        public bool Error { get; set; }
        public string ErrorMessage { get; set; }

        public TempFile(string _filename, string _tempFolder)
        {
            TempFolder = _tempFolder;

            Error = true;

            if (File.Exists(_filename)) {
                FileInfo finfo = new FileInfo(_filename);
                TempFileName = Path.Combine(TempFolder, finfo.Name);

                if (_filename == TempFileName)
                {
                    ErrorMessage = "Запуск на файле из временного каталога запрещено";
                    return;
                }

                if (!Directory.Exists(TempFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(TempFolder);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        ErrorMessage = string.Format("Ошибка при создании временного каталога: {0}", TempFolder);
                        return;
                    }
                    catch (Exception e)
                    {
                        ErrorMessage = e.Message;
                        return;
                    }
                }

                if (File.Exists(TempFileName))
                {
                    try
                    {
                        File.Delete(TempFileName);
                    }
                    catch (IOException)
                    {
                        ErrorMessage = string.Format("Ошибка - временный файл уже используется операционной системой: {0}", TempFileName);
                        return;
                    }                    
                    catch (UnauthorizedAccessException)
                    {
                        ErrorMessage = string.Format("У вас нет прав на операции с файлом: {0}", TempFileName);
                        return;
                    }
                }

                try
                {
                    File.Copy(_filename, TempFileName);
                }
                catch (UnauthorizedAccessException)
                {
                    ErrorMessage = string.Format("У вас недостаточно прав для копирования файла во временный каталог: {0}", _filename);
                    return;
                }
                catch (PathTooLongException)
                {
                    ErrorMessage = string.Format("Слишком длинное имя файла или путь: {0}, {1}", _filename, TempFileName);
                    return;
                }                
                catch (Exception)
                {
                    ErrorMessage = string.Format("Не удалось скопировать файл во временный каталог: ", _filename);
                    return;
                }
                Error = false;
            }

            ErrorMessage = "Файл не существует!";
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    //component.Dispose();
                }
                if (File.Exists(TempFileName))
                    File.Delete(TempFileName);

                TempFileName = null;
                Error = false;
                ErrorMessage = null;

                // Note disposing has been done.
                disposed = true;
            }
        }



    }
}
