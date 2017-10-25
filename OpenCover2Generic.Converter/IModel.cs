﻿using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface IModel
    {
        void AddFile(string fileId, string filePath);
        IList<ISourceFileCoverageModel> GetCoverage();
        void AddSequencePoint(string fileId, string sourceLine, string visitedCount);
        void Clear();
        void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited);
    }
}