using System;
using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class FileCoverageModel : IFileCoverageModel
    {
        private readonly string filePath;
        private readonly Dictionary<string,ICoveragePoint> coveragePoints = new Dictionary<string,ICoveragePoint>();
        private readonly Dictionary<string, IBranchPoint> branchPoints = new Dictionary<string, IBranchPoint>();
        public FileCoverageModel(string filePath)
        {
            this.filePath = filePath;
        }

        public string FullPath
        {
            get
            {
                return filePath;
            }
        }

        public IList<ICoveragePoint> SequencePoints
        {
            get
            {
                List<ICoveragePoint> points = coveragePoints.Values.ToList();
                points.Sort((pair1, pair2) => pair1.SourceLine.CompareTo(pair2.SourceLine));
                return points;
            }
        }

        public void AddSequencePoint(string sourceLine, string visitedCount)
        {
            coveragePoints.Add(sourceLine, new CoveragePoint(sourceLine, visitedCount));
        }

        public void AddBranchPoint(string sourceLine, string visitedCount, string path)
        {
            IBranchPoint branchPoint = new BranchPoint(int.Parse(visitedCount), int.Parse(path));
            if(!branchPoints.ContainsKey(sourceLine)) {
                branchPoints[sourceLine] = branchPoint;
            } else
            {
                branchPoints[sourceLine] = branchPoints[sourceLine].Add(branchPoint);
            }
        }

        public IBranchPoint BranchPoint(string sourceLine)
        {
            return branchPoints.ContainsKey(sourceLine)?branchPoints[sourceLine]:null;
        }
    }
}