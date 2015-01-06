using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDirectoryContents
{
    public static class CompareMaps
    {
        public static IComparer _comparer;
        public static FileInfoWrapper[] _oldMapFileInfo;
        public static FileInfoWrapper[] _currentMapFileInfo;
        public static IComparer Comparer
        {
            get { return _comparer ?? new TextBasedComparer(_oldMapFileInfo, _currentMapFileInfo); }
            set { _comparer = value; }
        }

        public static FileInfoWrapper[] CompareAsFileInfo(FileInfoWrapper[] oldMap, FileInfoWrapper[] currentMap)
        {
            _oldMapFileInfo = oldMap;
            _currentMapFileInfo = currentMap;
            return Comparer.CompareAsFileInfo(_oldMapFileInfo, _currentMapFileInfo);
        }
    }

    public interface IComparer
    {
        FileInfoWrapper[] CompareAsFileInfo(FileInfoWrapper[] oldMap, FileInfoWrapper[] currentMap);
    }

    public class TextBasedComparer : IComparer
    {
        private FileInfoWrapper[] _oldMapFileInfo;
        private FileInfoWrapper[] _currentMapFileInfo;

        public TextBasedComparer(FileInfoWrapper[] oldMapFileInfo, FileInfoWrapper[] currentMapFileInfo)
        {
            _oldMapFileInfo = oldMapFileInfo;
            _currentMapFileInfo = currentMapFileInfo;
        }


        public FileInfoWrapper[] CompareAsFileInfo(FileInfoWrapper[] oldMap, FileInfoWrapper[] currentMap)
        {
            IEnumerable<FileInfoWrapper> AddedOrChanged = currentMap.Except(oldMap, new FileEntryWrapperComparer());
            if (AddedOrChanged.Count() > 0)
                foreach (var entry in AddedOrChanged)
                    entry.Modified = "Changed.";

            IEnumerable<FileInfoWrapper> NewFileNames = currentMap.Except(oldMap, new FileNameComparer());
            if (NewFileNames.Count() > 0)
                foreach (var entry in NewFileNames)
                    entry.Modified = "New file name found.";

            IEnumerable<FileInfoWrapper> MissingFileNames = oldMap.Except(currentMap, new FileNameComparer());
            if (MissingFileNames.Count() > 0)
                foreach (var entry in MissingFileNames)
                    entry.Modified = "File name not found.";

            IEnumerable<FileInfoWrapper> combined = AddedOrChanged.Union(MissingFileNames.Union(NewFileNames));
            return combined.ToArray();

        }
    }


    public class FileEntryWrapperComparer : IEqualityComparer<FileInfoWrapper>
    {
        public bool Equals(FileInfoWrapper a, FileInfoWrapper b)
        {
            if (Object.ReferenceEquals(a, b))
                return true;
            if (Object.ReferenceEquals(a, null) || Object.ReferenceEquals(b, null))
                return false;
            if (a.Hash == b.Hash)
            {
                if (a.Name != b.Name)
                { 
                    b.Modified = String.Format("Nafni hefur verið breytt í {0} úr {1}.", b.Name, a.Name); 
                    return false;                
                }

                if (a.Name == b.Name && a.FullName != b.FullName)
                { 
                    b.Modified = String.Format("Skrá hefur verið færð, er núna {0}.", b.FullName); 
                    return false;                
                }

                if (a.LastWriteTime != b.LastWriteTime)
                { 
                     b.Modified = String.Format("Búið að vista skrána aftur, dagsetning er {0} en var {1}.", b.LastWriteTime, a.LastWriteTime); 
                    return false;               
                }
            }
            if (a.Length != b.Length)
            {
                b.Modified = String.Format("Stærð hefur breyst, er {0} en var {1}.", b.Length, a.Length);
                return false;
            }
            return true;
        }

        public int GetHashCode(FileInfoWrapper fileEntry)
        {
            return fileEntry.Name.GetHashCode();// fileEntry.Hash.GetHashCode() ^ fileEntry.Name.GetHashCode() ^ fileEntry.FullName.GetHashCode() ^ fileEntry.Length.GetHashCode();
        }
    }

    public class FileNameComparer : IEqualityComparer<FileInfoWrapper>
    {
        public bool Equals(FileInfoWrapper a, FileInfoWrapper b)
        {
            return a.Name == b.Name;
        }

        public int GetHashCode(FileInfoWrapper fileEntry)
        {
            return fileEntry.Name == null ? 0 : fileEntry.Name.GetHashCode();
            //return fileEntry.Name == null ? 0 : fileEntry.Name.GetHashCode();
        }
    }
}
